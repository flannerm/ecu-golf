using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;

using Microsoft.Maps.MapControl;
using Microsoft.Maps.MapControl.WPF;

using ECU.Golf.Shared.Models;
using ECU.Golf.Commands;
using ECU.Golf.DataAccess;
using System.Windows.Threading;
using System.Windows;
using System.Collections.ObjectModel;

namespace ECU.Golf.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        #region Private Members

        private ApplicationIdCredentialsProvider _credProvider;

        private ObservableCollection<PlayerViewModelBase> _playerVMs;

        private Point _centerPoint;

        private List<MapPoint> _teesLayer;
        private List<MapPoint> _pinsLayer;
        private List<MapPolyline> _linesLayer;
        private List<MapPoint> _playerShotsLayer;
        private List<MapPolyline> _playerShotsLinesLayer;

        private List<PlayerShotTrackingViewModel> _selectedPlayerVMs;

        private string _showTees = "Hide Tees";
        private bool _teesShowing = true;

        private string _showPins = "Hide Pins";
        private bool _pinsShowing = true;

        private string _showLines = "Hide Lines";
        private bool _linesShowing = true;

        private bool _playerShotsShowing = true;

        private DelegateCommand _showTeesCommand;
        private DelegateCommand _showPinsCommand;
        private DelegateCommand _showLinesCommand;

        private List<Color> _chosenColors;
             
        #endregion

        #region Properties

        public bool IsSelected { get; set; }

        public ObservableCollection<PlayerViewModelBase> PlayerVMs
        {
            get { return _playerVMs; }
            set { _playerVMs = value; OnPropertyChanged("PlayerVMs"); }
        }

        public Point CenterPoint
        {
            get { return _centerPoint; }
            set { _centerPoint = value; OnPropertyChanged("CenterPoint"); }
        }

        public ApplicationIdCredentialsProvider CredProvider
        {
            get { return _credProvider; }
            set { _credProvider = value; OnPropertyChanged("CredProvider"); }
        }

        public List<MapPoint> TeesLayer
        {
            get { return _teesLayer; }
            set { _teesLayer = value; OnPropertyChanged("TeesLayer"); }
        }

        public List<MapPoint> PinsLayer
        {
            get { return _pinsLayer; }
            set { _pinsLayer = value; OnPropertyChanged("PinsLayer"); }
        }

        public List<MapPolyline> LinesLayer
        {
            get { return _linesLayer; }
            set { _linesLayer = value; OnPropertyChanged("LinesLayer"); }
        }

        public List<MapPoint> PlayerShotsLayer
        {
            get { return _playerShotsLayer; }
            set { _playerShotsLayer = value; OnPropertyChanged("PlayerShotsLayer"); }
        }

        public List<MapPolyline> PlayerShotsLinesLayer
        {
            get { return _playerShotsLinesLayer; }
            set { _playerShotsLinesLayer = value; OnPropertyChanged("PlayerShotsLinesLayer"); }
        }

        public string ShowTees
        {
            get { return _showTees; }
            set { _showTees = value; OnPropertyChanged("ShowTees"); }
        }

        public string ShowPins
        {
            get { return _showPins; }
            set { _showPins = value; OnPropertyChanged("ShowPins"); }
        }

        public string ShowLines
        {
            get { return _showLines; }
            set { _showLines = value; OnPropertyChanged("ShowLines"); }
        }

        public bool TeesShowing
        {
            get { return _teesShowing; }
            set { _teesShowing = value; OnPropertyChanged("TeesShowing"); }
        }

        public bool PinsShowing
        {
            get { return _pinsShowing; }
            set { _pinsShowing = value; OnPropertyChanged("PinsShowing"); }
        }

        public bool PlayerShotsShowing
        {
            get { return _playerShotsShowing; }
            set { _playerShotsShowing = value; OnPropertyChanged("PlayerShotsShowing"); }
        }

        public bool LinesShowing
        {
            get { return _linesShowing; }
            set { _linesShowing = value; OnPropertyChanged("LinesShowing"); }
        }

        //public List<PlayerViewModelBase> SelectedPlayerVMs
        //{
        //    get { return _selectedPlayerVMs; }
        //    set
        //    {
        //        if (value != null && value != _selectedPlayerVMs)
        //        {
        //            _selectedPlayerVMs = value;
        //            OnPropertyChanged("SelectedPlayerVMs");

        //            drawPlayerShots();
        //        }
        //    }
        //}

        #endregion

        #region Constructor

        public MapViewModel()
        {
            _credProvider = new ApplicationIdCredentialsProvider(ConfigurationManager.AppSettings["BingMapsKey"].ToString());

            _selectedPlayerVMs = new List<PlayerShotTrackingViewModel>();

            _chosenColors = new List<Color>();

            loadPlayerVMs();

            drawTees();
            drawPins();
            drawLines();
        }

        #endregion

        #region Private Methods

        private void test()
        {
            MapTileLayer tileLayer = new MapTileLayer();
            
            TileSource tileSource = new TileSource("{UriScheme}://www.microsoft.com/maps/isdk/ajax/layers/lidar/{quadkey}.png",
                                                            new LocationRect(new Location(48.06282, -122.43773), new Location(47.999973, -122.37490)),
                                                            new Range<double>(10, 16));
        }

        private void loadPlayerVMs()
        {
            ObservableCollection<PlayerViewModelBase> playerVMs;

            string lastPos = "";

            if (AppData.Players != null && AppData.Players.Count > 0)
            {
                playerVMs = new ObservableCollection<PlayerViewModelBase>();

                foreach (Player player in AppData.Players)
                {
                    if (player.Status == 1)
                    {
                        if (lastPos == player.DisplayPosition)
                        {
                            player.DisplayPosition = "";
                        }
                        else
                        {
                            lastPos = player.DisplayPosition;
                        }

                        PlayerShotTrackingViewModel playerVM = new PlayerShotTrackingViewModel(player);
                        playerVM.CheckEvent += new PlayerShotTrackingViewModel.CheckEventHandler(playerVM_CheckEvent);

                        playerVMs.Add(playerVM);
                    }
                }

                PlayerVMs = playerVMs;
            }
        }

        private void playerVM_CheckEvent(PlayerShotTrackingViewModel player)
        {
            if (player.IsChecked)
            {
                _selectedPlayerVMs.Add(player);
            }
            else
            {
                player.IndicatorColor = new SolidColorBrush(Colors.Black);
                _selectedPlayerVMs.Remove(player);
            }

            drawPlayerShots();
        }

        private void drawTees()
        {
            if (AppData.Tournament.Event.Courses[0].Holes != null)
            {
                _teesLayer = new List<MapPoint>();

                foreach (Hole hole in AppData.Tournament.Event.Courses[0].Holes)
                {
                    if (hole.TeeLocation != null && hole.TeeLocation.Trim() != "")
                    {
                        string[] coords = hole.TeeLocation.Split(',');
                        double latitude = double.Parse(coords[0]);
                        double longitude = double.Parse(coords[1]);

                        _teesLayer.Add(new MapPoint(hole.HoleNumber.ToString(), new Location(latitude, longitude), new SolidColorBrush(Colors.Red)));
                    }
                }

                OnPropertyChanged("TeesLayer");
            }            
        }

        private void drawPins()
        {
            if (AppData.Tournament.Event.Courses[0].Holes != null)
            {
                _pinsLayer = new List<MapPoint>();

                foreach (Hole hole in AppData.Tournament.Event.Courses[0].Holes)
                {
                    if (hole.PinLocation != null && hole.PinLocation.Trim() != "")
                    {
                        string[] coords = hole.PinLocation.Split(',');
                        double latitude = double.Parse(coords[0]);
                        double longitude = double.Parse(coords[1]);

                        _pinsLayer.Add(new MapPoint(hole.HoleNumber.ToString(), new Location(latitude, longitude), new SolidColorBrush(Colors.Green)));
                    }
                }

                OnPropertyChanged("PinsLayer");
            }
        }

        private void drawLines()
        {
            if (AppData.Tournament.Event.Courses[0].Holes != null)
            {
                _linesLayer = new List<MapPolyline>();

                foreach (Hole hole in AppData.Tournament.Event.Courses[0].Holes)
                {
                    if (hole.PinLocation != null && hole.PinLocation.Trim() != "" && hole.TeeLocation != null && hole.TeeLocation.Trim() != "")
                    {
                        string[] coords = hole.PinLocation.Split(',');
                        double latitude = double.Parse(coords[0]);
                        double longitude = double.Parse(coords[1]);

                        Location pin = new Location(latitude, longitude);

                        coords = hole.TeeLocation.Split(',');
                        latitude = double.Parse(coords[0]);
                        longitude = double.Parse(coords[1]);

                        Location tee = new Location(latitude, longitude);

                        string[] approachLocs = hole.ApproachLocation.Split(';');
                        List<Location> approach = new List<Location>();

                        for (var i = 0; i < approachLocs.Length; i++)
                        {
                            if (approachLocs[i].Trim() != "")
                            {
                                coords = approachLocs[i].Split(',');
                                latitude = double.Parse(coords[0]);
                                longitude = double.Parse(coords[1]);

                                Location loc = new Location(latitude, longitude);

                                approach.Add(loc);
                            }
                        }

                        MapPolyline line = new MapPolyline();

                        LocationCollection points = new LocationCollection();

                        points.Add(tee);

                        foreach (Location loc in approach)
                        {
                            points.Add(loc);
                        }

                        points.Add(pin);

                        line.Locations = points;
                        //line.Fill = new SolidColorBrush(Colors.Blue);
                        line.Stroke = new SolidColorBrush(Colors.Yellow);
                        line.StrokeThickness = 5;
                        line.Opacity = 0.8;

                        _linesLayer.Add(line);
                    }
                }

                OnPropertyChanged("LinesLayer");
            }
        }

        private void drawPlayerShots()
        {
            double outVal = 0;

            if (_playerShotsLayer == null)
            {
                _playerShotsLayer = new List<MapPoint>();
            }

            if (_playerShotsLinesLayer == null)
            {
                _playerShotsLinesLayer = new List<MapPolyline>();
            }

            List<MapPoint> tempPlayerShotsLayer = new List<MapPoint>();
            List<MapPolyline> tempPlayerShotsLinesLayer = new List<MapPolyline>();

            int playerCount = 0;

            foreach (PlayerShotTrackingViewModel playerVM in _selectedPlayerVMs)
            {
                //placeholder for picking random color
                SolidColorBrush lineColor  = new SolidColorBrush(Colors.Black);

                switch (playerCount)
                {
                    case 0:
                        lineColor = new SolidColorBrush(Colors.Blue);
                        break;
                    case 1:
                        lineColor = new SolidColorBrush(Colors.Purple);
                        break;
                    case 2:
                        lineColor = new SolidColorBrush(Colors.White);
                        break;
                    case 3:
                        lineColor = new SolidColorBrush(Colors.DarkTurquoise);
                        break;
                    case 4:
                        lineColor = new SolidColorBrush(Colors.DeepPink);
                        break;
                }

                playerVM.IndicatorColor = lineColor;

                playerCount++;

                List<Shot> shots = DbConnection.GetShots(playerVM.Player.PlayerID, AppData.Tournament.CurrentRound);

                for (int i = 1; i <= 18; i++)
                {
                    List<Shot> holeShots = shots.Where(s => s.HoleNum == i).ToList<Shot>();

                    if (holeShots != null && holeShots.Count > 0)
                    {
                        LocationCollection points = new LocationCollection();

                        foreach (Shot shot in holeShots)
                        {
                            string[] coords = shot.ShotLocation.Split(',');
                            double latitude = double.TryParse(coords[0].ToString(), out outVal) ? outVal : 0;
                            double longitude = double.TryParse(coords[1], out outVal) ? outVal : 0;

                            Location loc = new Location(latitude, longitude);

                            tempPlayerShotsLayer.Add(new MapPoint(shot.ShotNum.ToString(), loc, lineColor));

                            points.Add(loc);
                        }

                        MapPolyline line = new MapPolyline();

                        line.Locations = points;
                        line.Stroke = lineColor;
                        line.StrokeThickness = 5;
                        line.Opacity = 0.8;

                        tempPlayerShotsLinesLayer.Add(line);
                    }
                }
            }

            PlayerShotsLayer = tempPlayerShotsLayer;
            PlayerShotsLinesLayer = tempPlayerShotsLinesLayer;          
        }

        private void showTees()
        {
            if (_teesShowing)
            {
                TeesShowing = false;
                ShowTees = "Show Tees";
            }
            else
            {
                TeesShowing = true;
                ShowTees = "Hide Tees";
            }
        }

        private void showPins()
        {
            if (_pinsShowing)
            {
                PinsShowing = false;
                ShowPins = "Show Pins";
            }
            else
            {
                PinsShowing = true;
                ShowPins = "Hide Pins";
            }
        }

        private void showLines()
        {
            if (_linesShowing)
            {
                LinesShowing = false;
                ShowLines = "Show Lines";
            }
            else
            {
                LinesShowing = true;
                ShowLines = "Hide Lines";
            }
        }

        private Color getColor()
        {
            const int DELTA_PERCENT = 10;
            
            // initialize the random generator
            Random r = new Random();

            Color tmpColor = new Color();

            for (int i = 0; i < 20; i++)
            {
                bool chooseAnotherColor = true;
                while (chooseAnotherColor)
                {
                    // create a random color by generating three random channels
                    //
                    int redColor = r.Next(0, 255);
                    int greenColor = r.Next(0, 255);
                    int blueColor = r.Next(0, 255);

                    tmpColor = Color.FromRgb((Byte)redColor, (Byte)greenColor, (Byte)blueColor);

                    // check if a similar color has already been created
                    //
                    chooseAnotherColor = false;
                    foreach (Color c in _chosenColors)
                    {
                        int delta = c.R * DELTA_PERCENT / 100;
                        if (c.R - delta <= tmpColor.R && tmpColor.R <= c.R + delta)
                        {
                            chooseAnotherColor = true;
                            break;
                        }

                        delta = c.G * DELTA_PERCENT / 100;
                        if (c.G - delta <= tmpColor.G && tmpColor.G <= c.G + delta)
                        {
                            chooseAnotherColor = true;
                            break;
                        }

                        delta = c.B * DELTA_PERCENT / 100;
                        if (c.B - delta <= tmpColor.B && tmpColor.B <= c.B + delta)
                        {
                            chooseAnotherColor = true;
                            break;
                        }
                    }
                }

                _chosenColors.Add(tmpColor);
            }

            return tmpColor;
        }

        private void update()
        {
            drawPlayerShots();
            drawTees();
            drawPins();
            drawLines();
        }

        #endregion

        #region Public Methods

        public override void Update()
        {
            if (_selectedPlayerVMs != null)
            {
                App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => update()));
            }
        }

        #endregion

        #region Commands

        public ICommand ShowTeesCommand
        {
            get
            {
                if (_showTeesCommand == null)
                {
                    _showTeesCommand = new DelegateCommand(showTees);
                }
                return _showTeesCommand;
            }
        }

        public ICommand ShowPinsCommand
        {
            get
            {
                if (_showPinsCommand == null)
                {
                    _showPinsCommand = new DelegateCommand(showPins);
                }
                return _showPinsCommand;
            }
        }

        public ICommand ShowLinesCommand
        {
            get
            {
                if (_showLinesCommand == null)
                {
                    _showLinesCommand = new DelegateCommand(showLines);
                }
                return _showLinesCommand;
            }
        }

        #endregion

    }

    public class MapPoint
    {
        #region Private Members

        private string _description;
        private Location _location;
        private SolidColorBrush _fill;

        #endregion

        #region Properties
        
        public string Description 
        { 
            get { return _description; }
            set { _description = value; } 
        }

        public Location Location 
        {
            get { return _location; }
            set { _location = value; } 
        }

        public SolidColorBrush Fill
        {
            get { return _fill; }
            set { _fill = value; }
        }

        #endregion

        #region Constructor

        public MapPoint(string description, Location location, SolidColorBrush fill)
        {
            _description = description;
            _location = location;
            _fill = fill;
        }

        #endregion

    }

}
