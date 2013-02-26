using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using ECU.Golf.Shared.Models;
using System.Data;
using System.Diagnostics;
using System.Windows.Media;

namespace ECU.Golf.DataAccess
{
    public class DbConnection
    {
        //public delegate void SetStatusBarMsgEventHandler(string msgText, string msgColor);

        #region Private Members

        private static int _currentRoundID;
        private static int _currentRoundNum;

        #endregion 

        #region Database

        private static MySqlConnection createConnectionMySqlStats()
        {

            MySqlConnection cn = null;

            try
            {
                cn = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlStatsDb"].ConnectionString);
                cn.Open();
            }
            catch (MySqlException ex)
            {

            }            

            return cn;
        }

        private static MySqlConnection createConnectionMySqlChat()
        {
            MySqlConnection cn = null;

            try
            {
                cn = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlChatDb"].ConnectionString);
                cn.Open();
            }
            catch (MySqlException ex)
            {

            }

            return cn;
        }

        private static MySqlConnection createConnectionMySqlTouchscreen()
        {
            MySqlConnection cn = null;

            try
            {
                cn = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySqlTouchscreenDb"].ConnectionString);
                cn.Open();
            }
            catch (MySqlException ex)
            {

            }

            return cn;
        }

        #endregion

        #region Scoring

        public static Tournament GetTournament()
        {
            Tournament tournament = null;
            DataTable tbl = null;
            int i;

            MySqlConnection cn = null;

            try
            {
                using (cn = createConnectionMySqlStats())
                {
                    using (MySqlCommand cmd = new MySqlCommand("select *, (select roundid from rounds where roundnum = currentround) as currentroundid from tournament where tournamentid = 1", cn))
                    {
                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                tbl = new DataTable();
                                tbl.Load(rdr);
                                rdr.Close();

                                DataRow row = tbl.Rows[0];

                                tournament = new Tournament();
                                tournament.TournamentName = row["tournamentname"].ToString();
                                tournament.CurrentRound = int.TryParse(row["currentround"].ToString(), out i) ? i : 0;

                                _currentRoundNum = tournament.CurrentRound;
                                _currentRoundID = int.TryParse(row["currentroundid"].ToString(), out i) ? i : 0;

                                tournament.Event = GetEvent(cn, int.Parse(row["tournamentid"].ToString()));
                            }

                            
                        } //using rdrTourney
                    } //using cmdTourney

                    cn.Close();

                } // using cn
            }
            catch (Exception ex)
            { }
            finally
            { 
                if (cn != null) cn.Close(); cn.Dispose();
            }

            return tournament;
        }

        public static Event GetEvent(MySqlConnection cn, int tournamentId)
        {
            Event golfEvent = null;
            DataTable tbl = null;
            int i;

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("select * from event where tournamentid = " + tournamentId, cn))
                {
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            tbl = new DataTable();
                            tbl.Load(rdr);
                            rdr.Close();

                            DataRow row = tbl.Rows[0];

                            golfEvent = new Event();
                            golfEvent.EventID = int.TryParse(row["eventid"].ToString(), out i) ? i : 0;

                            golfEvent.Courses = GetCourses(cn, golfEvent.EventID);       
                        }

                        
                    } //using rdr
                } //using cmd

            }
            catch (Exception ex)
            { }
            finally
            { }

            return golfEvent;
        }

        public static List<Course> GetCourses(MySqlConnection cn, int eventId)
        {
            List<Course> courses = null;
            Course course = null;
            DataTable tbl = null;
            int i;

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("select * from course where eventid = " + eventId, cn))
                {
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            courses = new List<Course>();

                            tbl = new DataTable();
                            tbl.Load(rdr);
                            rdr.Close();

                            foreach (DataRow row in tbl.Rows)
                            {
                                course = new Course();

                                course.CourseID = int.TryParse(row["courseid"].ToString(), out i) ? i : 0;
                                course.CourseName = row["coursename"].ToString();
                                course.CourseGpsLocation = row["coursegpslocation"].ToString();
                                course.CourseGpsHeading = double.Parse(row["coursegpsheading"].ToString());

                                course.Holes = GetHoles(cn, course.CourseID);

                                courses.Add(course);
                            }
                                    
                        }
                    } //using rdr
                } //using cmd

            }
            catch (Exception ex)
            { }
            finally
            { }

            return courses;
        }

        public static List<Hole> GetHoles(MySqlConnection cn, int courseId)
        {
            List<Hole> holes = null;
            Hole hole = null;
            DataTable tbl = null;
            int i;

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("select * from holes where courseid = " + courseId, cn))
                {
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            holes = new List<Hole>();

                            tbl = new DataTable();
                            tbl.Load(rdr);
                            rdr.Close();

                            foreach (DataRow row in tbl.Rows)
                            {
                                hole = new Hole();

                                hole.HoleID = int.TryParse(row["holeid"].ToString(), out i) ? i : 0;
                                hole.Par = int.TryParse(row["par"].ToString(), out i) ? i : 0;
                                hole.Yardage = int.TryParse(row["yardage"].ToString(), out i) ? i : 0;
                                hole.HoleNumber = int.TryParse(row["holenum"].ToString(), out i) ? i : 0;
                                hole.TeeLocation = row["gpstee"].ToString();
                                hole.PinLocation = row["gpspin"].ToString();
                                hole.ApproachLocation = row["gpsapproach"].ToString();

                                holes.Add(hole);                                
                            }

                            for (var round = 0; round <= _currentRoundNum; round++)
                            {
                                GetHoleStatLine(ref holes, cn, round);
                            }
                        }                        
                    } //using rdr
                } //using cmd

            }
            catch (Exception ex)
            { }
            finally
            { }

            return holes;
        }

        public static void GetHoleStatLine(ref List<Hole> holes, MySqlConnection cn, int roundNum)
        {
            DataTable tbl = null;

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("CALL sp_GetHoleStats(" + roundNum + ")", cn))
                {
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            tbl = new DataTable();
                            tbl.Load(rdr);
                            rdr.Close();

                            foreach (DataRow row in tbl.Rows)
                            {                                
                                Hole hole = holes.SingleOrDefault(h => h.HoleID == Convert.ToInt16(row["holeid"]));

                                if (hole != null)
                                {
                                    if (hole.HoleStats == null)
                                    {
                                        hole.HoleStats = new List<HoleStatLine>();
                                    }                                   

                                    HoleStatLine holeStatLine = new HoleStatLine();

                                    holeStatLine.Round = roundNum;

                                    holeStatLine.Aces = Convert.ToInt16(row["aces"]);
                                    holeStatLine.Eagles = Convert.ToInt16(row["eagles"]);
                                    holeStatLine.Birdies = Convert.ToInt16(row["birdies"]);
                                    holeStatLine.Pars = Convert.ToInt16(row["pars"]);
                                    holeStatLine.Bogeys = Convert.ToInt16(row["bogeys"]);
                                    holeStatLine.Others = Convert.ToInt16(row["others"]);

                                    if (row["scoreavg"] != DBNull.Value)
                                    {
                                        holeStatLine.ScoreAvg = Math.Round(Convert.ToDecimal(row["scoreavg"]), 2);
                                        holeStatLine.ScoreDiff = Convert.ToDecimal(row["scorediff"]);

                                        int rankNum;

                                        if (row["scoringrank"].ToString().ToUpper().IndexOf("T") > -1)
                                        {
                                            rankNum = Convert.ToInt16(row["scoringrank"].ToString().Substring(1));
                                        }
                                        else
                                        {
                                            rankNum = Convert.ToInt16(row["scoringrank"].ToString());
                                        }

                                        switch (rankNum)
                                        {
                                            case 1:
                                                holeStatLine.ScoreRank = row["scoringrank"].ToString() + "st";
                                                break;
                                            case 2:
                                                holeStatLine.ScoreRank = row["scoringrank"].ToString() + "nd";
                                                break;
                                            case 3:
                                                holeStatLine.ScoreRank = row["scoringrank"].ToString() + "rd";
                                                break;
                                            default:
                                                holeStatLine.ScoreRank = row["scoringrank"].ToString() + "th";
                                                break;
                                        }

                                        if (hole.Par == 3)
                                        {
                                            holeStatLine.FairwaysAvg = "-";
                                        }
                                        else
                                        {
                                            //hole.FairwaysAvg = Convert.ToDecimal(row["fairwaysavg"]).ToString();
                                            holeStatLine.FairwaysAvg = Math.Round(Convert.ToDecimal(row["fairwaysavg"]) * 100, 1).ToString() + "%";
                                        }

                                        holeStatLine.GIR = Math.Round(Convert.ToDecimal(row["greensavg"]) * 100, 1).ToString() + "%";
                                        holeStatLine.PuttsAvg = Math.Round(Convert.ToDecimal(row["puttsavg"]), 2);

                                        hole.HoleStats.Add(holeStatLine);
                                    }           
                                }         

                            }

                        }
                    } //using rdr
                } //using cmd

            }
            catch (Exception ex)
            { }
            finally
            { }
        }

        public static List<Player> GetPlayers(int? pairingId = null)
        {
            MySqlConnection cn = null;
            MySqlCommand cmd = null;
            MySqlDataReader rdr = null;
            DataTable tbl = null;
            Int16 number;
            int status;
            //bool parseResult;

            List<Player> players = null;

            //Stopwatch swAll = new Stopwatch();

            //swAll.Start();

            try
            {
                cn = createConnectionMySqlStats();

                if (cn != null)
                {
                    string sql = "";
                    string currentTimestamp = "";

                    try
                    {
                        sql = "select CURRENT_TIMESTAMP as currenttime from dual";
                        cmd = new MySqlCommand(sql, cn);
                        rdr = cmd.ExecuteReader();

                        tbl = new DataTable();

                        tbl.Load(rdr);
                        rdr.Close();
                        rdr.Dispose();

                        if (tbl.Rows.Count > 0)
                        {
                            currentTimestamp = tbl.Rows[0]["currenttime"].ToString();
                        }
                    }
                    finally
                    {
                        tbl.Dispose();
                        cmd.Dispose();
                    }
                    
                    sql = "select b.PlayerID as ID, a.*, b.*,"; // e.*, ";
                    sql += "(select count(*) from playerholes where playerid = b.playerid and roundid = (select roundid from rounds where roundnum = (select currentround from tournament)) and (score > 0 or score != null)) as ThruNum, ";
                    sql += "(select count(*) from playerholes where playerid = b.playerid and (score > 0 or score != null)) as TotalHolesPlayed, ";
                    sql += "(select starttime from pairings p join playerpairings pp on p.pairingid = pp.pairingid ";
                    sql += "where pp.playerid = b.playerid and p.roundid = (select roundid from rounds where roundnum = (select currentround from tournament))) as StartTime ";

                    sql += "from leaderboard a ";
                    sql += "right join players b on a.playerid = b.playerid ";
                    sql += "join countries c on b.countryid = c.countryid ";

                    //sql += "join playerpairings d on b.playerid = d.playerid ";
                    //sql += "join pairings e on d.pairingid = e.pairingid ";

                    //sql += "where e.roundid = (select currentround from tournament) ";

                    //if (pairingId != null)
                    //{
                    //    sql += "where e.pairingid = " + pairingId + " ";
                    //}

                    sql += "order by coalesce(rankorder, 99999) asc, priority, lastname, firstname";

                    cmd = new MySqlCommand(sql, cn);
                    rdr = cmd.ExecuteReader();

                    tbl = new DataTable();

                    tbl.Load(rdr);
                    rdr.Close();
                    rdr.Dispose();

                    if (tbl.Rows.Count > 0)
                    {
                        players = new List<Player>();

                        foreach (DataRow row in tbl.Rows)
                        {
                            //Stopwatch swPlayer = new Stopwatch();

                            //swPlayer.Start();

                            Player player = new Player();

                            player.PlayerID = Convert.ToInt32(row["ID"]);
                            player.FirstName = row["firstname"].ToString();
                            player.LastName = row["lastname"].ToString();
                            player.TvName = row["tvname"].ToString();
                            player.Headshot = new Uri(ConfigurationManager.AppSettings["HeadshotFolder"].ToString() + "\\" + row["headshot"].ToString() + ".png");
                            player.Country = getCountry(Convert.ToInt16(row["countryid"]));
                            player.Status = int.TryParse(row["status"].ToString(), out status) ? status : 0;
                            player.Position = Int16.TryParse(row["rankorder"].ToString(), out number) ? number : 0;
                                                        
                            player.PositionStr = row["rank"].ToString();
                            player.DisplayPosition = row["rank"].ToString();

                            player.TotalScore = Int16.TryParse(row["totalscore"].ToString(), out number) ? number : 0;
                                                       
                            player.TotalScoreStr = row["total"].ToString();

                            decimal todayScore;
                            Decimal.TryParse(row["today"].ToString(), out todayScore);                            
                            player.TodayScore = todayScore;

                            player.TodayScoreStr = row["today"].ToString();

                            if ((Convert.ToInt16(row["totalholesplayed"]) == 0 || Convert.ToInt16(row["thrunum"].ToString()) == 0) && row["status"].ToString() == "1")
                            {
                                try
                                {
                                    DateTime currentTime = DateTime.Parse(currentTimestamp);
                                    DateTime teeTime = DateTime.Parse(row["starttime"].ToString());

                                    string currentTimeStr = currentTime.ToString("H:mm");
                                    string teeTimeStr = teeTime.ToString("H:mm");
                                    string teeTimeStrh = teeTime.ToString("h:mm");
                                    string currentDate = DateTime.Now.ToString("MM/dd/yyyy");

                                    currentTime = DateTime.Parse(currentDate + " " + currentTimeStr);
                                    teeTime = DateTime.Parse(currentDate + " " + teeTimeStr);

                                    TimeSpan timeDiff = teeTime.Subtract(currentTime);

                                    if (timeDiff.TotalSeconds < 0)
                                    {
                                        player.ThruStr = "-";
                                    }
                                    else
                                    {
                                        player.ThruStr = "-" + string.Format("{0:%h\\:mm}", timeDiff);
                                    }
                                }
                                catch (Exception ex)
                                { }
                            }
                            else
                            {
                                player.ThruStr = row["hole"].ToString();                                                              
                            }
                           
                            number = Convert.ToInt16(row["thrunum"].ToString());
                                                      
                            player.Thru = number;

                            if (number == 18)
                            {
                                player.ThruColor = "#c5eac9";
                            }
                            else
                            {
                                player.ThruColor = "#ffffff";
                            }

                            player.BestTourneyFinish = row["besttourneyfinish"].ToString();
                            player.BestMajorFinish = row["bestmajorfinish"].ToString();
                            player.Age = Int16.TryParse(row["age"].ToString(), out number) ? number : 0;
                            player.EuroTourWins = Int16.TryParse(row["pgatourwins"].ToString(), out number) ? number : 0;
                            player.MajorWins = Int16.TryParse(row["majorwins"].ToString(), out number) ? number : 0;
                            player.WorldRank = Int16.TryParse(row["worldrank"].ToString(), out number) ? number : 0;

                            if (row["notable"] != DBNull.Value)
                            {
                                player.Notable = Convert.ToBoolean(row["notable"]);
                            }
                                                      
                            player.Clothing = row["clothing"].ToString();

                            Int16 pos = 0;

                            try
                            {
                                Int16.TryParse(player.PositionStr.Replace("T", ""), out pos);
                            }
                            catch (Exception ex)
                            {

                            }                            

                            if (pos == 1)
                            {
                                player.HighlightColor = "#00FF00";
                            }
                            else if (pos <= 10 && pos > 0)
                            {
                                player.HighlightColor = "#FAC859";
                            }
                            else
                            {
                                try
                                {
                                    //if (row["highlightcolor"].ToString().Trim() == "")
                                    //{
                                    //    player.HighlightColor = "#FFFFFF";
                                    //}
                                    //else
                                    //{
                                    //    player.HighlightColor = "#" + row["highlightcolor"].ToString();
                                    //}
                                }
                                catch (Exception ex)
                                { }
                            }

                            player.Scorecards = new ObservableCollection<Scorecard>();

                            //for (int i = 1; i <= 4; i++)
                            //{
                            //    Scorecard card = getScorecard(Convert.ToInt32(row["ID"]), i);
                            //    player.Scorecards.Add(card);
                            //}

                            //swPlayer.Stop();

                            //Debug.WriteLine("Got " + player.LastName + " in " + swPlayer.Elapsed);

                            players.Add(player);
                        }
                        
                    }                    
                } 
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (rdr != null) rdr.Dispose();
                if (tbl != null) tbl.Dispose();
                if (cn != null) cn.Close(); cn.Dispose();
            }

            //swAll.Stop();

            //Debug.WriteLine("Got Players in: " + swAll.Elapsed);

            return players;
        }

        public static List<Pairing> GetPairings(List<Player> players)
        {
            //Stopwatch sw = new Stopwatch();

            //sw.Start();

            MySqlConnection cn = null;
            MySqlCommand cmd = null;
            MySqlDataReader rdr = null;
            DataTable tbl = null;

            List<Pairing> pairings = null;

            try
            {
                if (players != null & players.Count > 0)
                {
                    cn = createConnectionMySqlStats();

                    if (cn != null)
                    {
                        string sql = "select * from pairings a join rounds b on a.roundid = b.roundid order by a.roundid, a.starttime, a.starthole";

                        cmd = new MySqlCommand(sql, cn);
                        rdr = cmd.ExecuteReader();

                        tbl = new DataTable();

                        tbl.Load(rdr);
                        rdr.Close();
                        rdr.Dispose();

                        if (tbl.Rows.Count > 0)
                        {
                            pairings = new List<Pairing>();

                            foreach (DataRow row in tbl.Rows)
                            {
                                Pairing pairing = new Pairing();

                                pairing.PairingID = Convert.ToInt32(row["pairingid"]);
                                pairing.Round = Convert.ToInt16(row["roundnum"]);
                                pairing.TeeTime = Convert.ToDateTime(row["starttime"]);
                                pairing.FormattedTeeTime = Convert.ToDateTime(row["starttime"]).ToString("h:mm");
                                pairing.StartHole = Convert.ToInt16(row["starthole"]);
                                pairing.CourseID = Convert.ToInt16(row["courseid"]);
                                pairing.OnCourse = row["oncourse"].ToString();

                                pairing.Players = new List<Player>();

                                DataTable tblPlayers = new DataTable();

                                try
                                {
                                    sql = "select * from playerpairings where pairingid = " + row["pairingid"].ToString();
                                    cmd = new MySqlCommand(sql, cn);
                                    rdr = cmd.ExecuteReader();

                                    tblPlayers.Load(rdr);
                                    rdr.Close();
                                    rdr.Dispose();

                                    foreach (DataRow rowPlayer in tblPlayers.Rows)
                                    {
                                        pairing.Players.Add(players.FirstOrDefault(p => p.PlayerID == Convert.ToInt32(rowPlayer["playerid"])));
                                    }
                                }
                                finally
                                {
                                    if (cmd != null) cmd.Dispose();
                                    if (tblPlayers != null) tblPlayers.Dispose();
                                }

                                //pairing.Players = GetPlayers(pairing.PairingID);                                  

                                pairings.Add(pairing);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (rdr != null) rdr.Dispose();
                if (tbl != null) tbl.Dispose();
                if (cn != null) cn.Close(); cn.Dispose();
            }

            //sw.Stop();

            //Debug.WriteLine("GetPairings: " + sw.Elapsed.Milliseconds);

            return pairings;
        }

        public static Scorecard GetScorecard(Int32 playerId, int round)
        {
            MySqlConnection cn = null;
            MySqlCommand cmd = null;
            MySqlDataReader rdr = null;
            DataTable tbl = null;

            Scorecard card = null;

            try
            {
                cn = createConnectionMySqlStats();

                string sql = "CALL sp_GetPlayerScorecard(" + playerId + ", " + round + ")";

                cmd = new MySqlCommand(sql, cn);
                rdr = cmd.ExecuteReader();

                tbl = new DataTable();

                tbl.Load(rdr);
                rdr.Close();
                rdr.Dispose();

                if (tbl.Rows.Count > 0)
                {
                    card = new Scorecard();

                    card.Round = round;

                    card.Holes = new ObservableCollection<PlayerHole>();

                    foreach (DataRow row in tbl.Rows)
                    {
                        PlayerHole hole = new PlayerHole();

                        hole.HoleNumber = Convert.ToInt16(row["holenum"]);
                        hole.Par = Convert.ToInt16(row["par"]);
                        hole.Yardage = Convert.ToInt16(row["yardage"]);

                        if (row["hold"] != DBNull.Value)
                        {
                            if (Convert.ToInt16(row["hold"]) == 0) //no hold on this score
                            {
                                Int16 number;

                                bool result = Int16.TryParse(row["score"].ToString(), out number);

                                if (result)
                                {
                                    if (number > 0)
                                    {
                                        hole.Score = number;
                                        hole.Fairway = Convert.ToBoolean(row["fairwayhit"]);
                                        hole.GIR = Convert.ToBoolean(row["gir"]);
                                        hole.Putts = Convert.ToInt16(row["putts"]);

                                        if (row["drivedistance"] != DBNull.Value)
                                        {
                                            hole.DriveDistance = Convert.ToDecimal(row["drivedistance"]);
                                        }                                        
                                    }
                                }

                            }
                        }
                    
                        card.Holes.Add(hole);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (rdr != null) rdr.Dispose();
                if (tbl != null) tbl.Dispose();
                if (cn != null) cn.Close(); cn.Dispose();
            }

            return card;
        }

        public static List<Shot> GetShots(Int32 playerId, int round)
        {
            List<Shot> shots;

            using (MySqlConnection cn = createConnectionMySqlStats())
            {
                using (MySqlCommand cmd = new MySqlCommand("select * from playershots a join clubs b on a.clubid = b.clubid join holes c on a.holeid = c.holeid where a.playerid = " + playerId + " and a.roundid = (select roundid from rounds where roundnum = " + round + ")", cn))
                {
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        DataTable tbl = new DataTable();

                        tbl.Load(rdr);                        
                        rdr.Close();

                        shots = new List<Shot>();

                        foreach (DataRow row in tbl.Rows)
                        {
                            int i;

                            Shot shot = new Shot();

                            shot.ShotID = int.Parse(row["shotid"].ToString());
                            shot.ShotNum = int.Parse(row["shot"].ToString());
                            shot.HoleNum = int.Parse(row["holenum"].ToString());
                            shot.ShotLocation = row["gpslocation"].ToString();
                            shot.DistanceTraveled = int.TryParse(row["distancetraveled"].ToString(), out i) ? i : 0;
                            shot.DistanceToPin = int.TryParse(row["distancetopin"].ToString(), out i) ? i : 0;
                            
                            shot.Club = new Club();
                            shot.Club.ClubID = int.Parse(row["clubid"].ToString());
                            shot.Club.Description = row["description"].ToString();

                            shots.Add(shot);
                        }
                    }
                }

                cn.Close();
            }

            return shots;
        }

        private static Country getCountry(int countryId)
        {
            MySqlConnection cn = null;
            MySqlCommand cmd = null;
            MySqlDataReader rdr = null;
            DataTable tbl = null;

            Country country = null;

            try
            {
                cn = createConnectionMySqlStats();

                string sql = "select * from countries where countryid = " + countryId;

                cmd = new MySqlCommand(sql, cn);
                rdr = cmd.ExecuteReader();

                tbl = new DataTable();

                tbl.Load(rdr);
                rdr.Close();
                rdr.Dispose();

                if (tbl.Rows.Count == 1)
                {
                    DataRow row = tbl.Rows[0];
                    country = new Country();
                    country.CountryID = countryId;
                    country.CountryName = row["country"].ToString();
                    country.Abbrev = row["abbrev"].ToString();
                    country.Flag = new Uri(ConfigurationManager.AppSettings["FlagFolder"].ToString() + "\\" + row["vizflag"].ToString() + ".png");
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (rdr != null) rdr.Dispose();
                if (tbl != null) tbl.Dispose();
                if (cn != null) cn.Close(); cn.Dispose();
            }

            return country;
        }

        public static Dictionary<Int16, string> GetHistoryCategories()
        {
            MySqlConnection cn = null;
            MySqlCommand cmd = null;
            MySqlDataReader rdr = null;
            DataTable tbl = null;

            Dictionary<Int16, string> categories = null;

            try
            {
                cn = createConnectionMySqlTouchscreen();

                if (cn != null)
                {
                    string sql = "select * from golfhistorycategories";

                    cmd = new MySqlCommand(sql, cn);
                    rdr = cmd.ExecuteReader();

                    tbl = new DataTable();

                    tbl.Load(rdr);
                    rdr.Close();
                    rdr.Dispose();

                    if (tbl.Rows.Count > 0)
                    {
                        categories = new Dictionary<Int16, string>();

                        foreach (DataRow row in tbl.Rows)
                        {
                            categories.Add(Convert.ToInt16(row["categoryid"]), row["title"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (rdr != null) rdr.Dispose();
                if (tbl != null) tbl.Dispose();
                if (cn != null) cn.Close(); cn.Dispose();
            }

            return categories;
        }

        public static List<string> GetHistoryItems(Int16 categoryID)
        {
            MySqlConnection cn = null;
            MySqlCommand cmd = null;
            MySqlDataReader rdr = null;
            DataTable tbl = null;

            List<string> historyItems = null;

            try
            {
                cn = createConnectionMySqlTouchscreen();

                if (cn != null)
                {
                    string sql = "select * from golfhistoryitems where categoryid = " + categoryID.ToString() + " order by itemid asc";

                    cmd = new MySqlCommand(sql, cn);
                    rdr = cmd.ExecuteReader();

                    tbl = new DataTable();

                    tbl.Load(rdr);
                    rdr.Close();
                    rdr.Dispose();

                    if (tbl.Rows.Count > 0)
                    {
                        historyItems = new List<string>();

                        foreach (DataRow row in tbl.Rows)
                        {
                            historyItems.Add(row["text"].ToString().Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (rdr != null) rdr.Dispose();
                if (tbl != null) tbl.Dispose();
                if (cn != null) cn.Close(); cn.Dispose();
            }

            return historyItems;
        }

        public static List<string> GetShotLogItems()
        {
            MySqlConnection cn = null;
            MySqlCommand cmd = null;
            MySqlDataReader rdr = null;
            DataTable tbl = null;

            List<string> shotLogItems = null;

            try
            {
                cn = createConnectionMySqlStats();

                if (cn != null)
                {
                    string sql = "(select TIME_FORMAT(Timecode, '%H:%i:%s') AS formattedtimecode, timecode, messageid, tagged, messagetext ";
                    sql += "from messages where timecode > (select timecode from messages where tagged = '1') limit 7) union ";
                    sql += "(select TIME_FORMAT(Timecode, '%H:%i:%s') AS formattedtimecode, timecode, messageid, tagged, messagetext from messages where tagged = '1') union ";
                    sql += "(select TIME_FORMAT(Timecode, '%H:%i:%s') AS formattedtimecode, timecode, messageid, tagged, messagetext from messages where timecode < (select timecode from messages where tagged = '1') limit 20) ";
                    sql += "order by timecode desc";

                    cmd = new MySqlCommand(sql, cn);
                    rdr = cmd.ExecuteReader();

                    tbl = new DataTable();

                    tbl.Load(rdr);
                    rdr.Close();
                    rdr.Dispose();

                    if (tbl.Rows.Count > 0)
                    {
                        shotLogItems = new List<string>();

                        foreach (DataRow row in tbl.Rows)
                        {
                            shotLogItems.Add(row["text"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (rdr != null) rdr.Dispose();
                if (tbl != null) tbl.Dispose();
                if (cn != null) cn.Close(); cn.Dispose();
            }

            return shotLogItems;
        }

        public static string GetResearchNote()
        {
            MySqlConnection cn = null;
            MySqlCommand cmd = null;
            MySqlDataReader rdr = null;
            DataTable tbl = null;

            string note = "";

            try
            {
                cn = createConnectionMySqlTouchscreen();

                if (cn != null)
                {
                    string sql = "select * from golfnotes where noteid = 1";

                    cmd = new MySqlCommand(sql, cn);
                    rdr = cmd.ExecuteReader();

                    tbl = new DataTable();

                    tbl.Load(rdr);
                    rdr.Close();
                    rdr.Dispose();

                    if (tbl.Rows.Count > 0)
                    {
                        note = tbl.Rows[0]["notetext"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (rdr != null) rdr.Dispose();
                if (tbl != null) tbl.Dispose();
                if (cn != null) cn.Close(); cn.Dispose();
            }

            return note;
        }

        public static List<Tweet> GetTweets()
        {
            MySqlConnection cn = createConnectionMySqlTouchscreen();
            String sql = "select * from tweets where ts_app = '" + ConfigurationManager.AppSettings["AppName"] + "' order by createdat desc ";
            MySqlCommand cmd = new MySqlCommand(sql, cn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            DataTable tbl = new DataTable();

            Tweet tweet;
            List<Tweet> tweets = new List<Tweet>();

            try
            {
                tbl.Load(rdr);

                rdr.Close();
                rdr.Dispose();

                foreach (DataRow row in tbl.Rows)
                {
                    tweet = new Tweet();
                    tweet.TweetID = row["TweetID"].ToString();

                    System.DateTime createdAt = (System.DateTime)row["CreatedAt"];
                    System.DateTime currentDate = DateTime.Now;
                    System.TimeSpan diff1 = currentDate.Subtract(createdAt);
                    tweet.CreatedAt = row["TweetedAt"].ToString();

                    tweet.Text = row["Text"].ToString();
                    tweet.User = row["User"].ToString();
                    tweet.UserImage = row["UserImage"].ToString();
                    tweet.TweetType = row["TweetType"].ToString();
                    tweet.ScreenName = "@" + row["ScreenName"].ToString();

                    tweets.Add(tweet);
                }

                cn.Close();
                cn.Dispose();

                cn.Close();
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (tbl != null) tbl.Dispose();
                if (cn != null) cn.Close(); cn.Dispose();
            }
            
            return tweets;
        }

        public static bool SaveResearchNote(string noteText)
        {
            MySqlConnection cn = null;
            MySqlCommand cmd = null;
            MySqlDataAdapter adp = null;
            MySqlCommandBuilder bldr = null;
            DataTable tbl = null;
            DataRow row = null;

            bool saved = false;

            try
            {
                cn = createConnectionMySqlTouchscreen();

                if (cn != null)
                {
                    string sql = "select * from golfnotes where noteid = 1";

                    cmd = new MySqlCommand(sql, cn);
                    adp = new MySqlDataAdapter(cmd);
                    bldr = new MySqlCommandBuilder(adp);

                    tbl = new DataTable();

                    adp.Fill(tbl);

                    if (tbl.Rows.Count == 0)
                    {
                        row = tbl.Rows.Add();
                    }
                    else
                    {
                        row = tbl.Rows[0];
                    }

                    row["notetext"] = noteText;

                    adp.Update(tbl.GetChanges());
                    tbl.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (adp != null) adp.Dispose();
                if (bldr != null) bldr.Dispose();
                if (tbl != null) tbl.Dispose();
                if (cn != null) cn.Close(); cn.Dispose();
            }

            return saved;
        }

        public static bool SavePlayerClothing(Player player, string clothing)
        {
            MySqlConnection cn = null;
            MySqlCommand cmd = null;
            MySqlDataAdapter adp = null;
            MySqlCommandBuilder bldr = null;
            DataTable tbl = null;
            DataRow row = null;

            bool saved = false;

            try
            {
                cn = createConnectionMySqlStats();

                if (cn != null)
                {
                    string sql = "select * from players where playerid = " + player.PlayerID;

                    cmd = new MySqlCommand(sql, cn);
                    adp = new MySqlDataAdapter(cmd);
                    bldr = new MySqlCommandBuilder(adp);

                    tbl = new DataTable();

                    adp.Fill(tbl);

                    if (tbl.Rows.Count == 1)
                    {
                        row = tbl.Rows[0];
                        row["clothing"] = clothing;

                        adp.Update(tbl.GetChanges());
                        tbl.AcceptChanges();

                        saved = true;
                    }
                }

            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (adp != null) adp.Dispose();
                if (bldr != null) bldr.Dispose();
                if (tbl != null) tbl.Dispose();
                if (cn != null) cn.Close(); cn.Dispose();
            }

            return saved;
        }

        #endregion

        #region Chat

        public static List<ChatUser> GetChatUsers(bool all)
        {
            List<ChatUser> users = null;
            MySqlConnection cn = null;
            MySqlCommand cmd = null;
            MySqlDataReader rdr = null;
            DataTable tbl = null;

            int i;

            try
            {
                cn = createConnectionMySqlChat();

                string sql = "";
                
                if (all)
                {
                    sql = "select a.*, (select 1 from dual) as usertype from users a join applicationusers b on a.userid = b.userid where b.applicationid = " + ConfigurationManager.AppSettings["ChatApplicationID"].ToString();
                    sql += " union all select *, (select 2 from dual) as usertype from groups order by username asc";
                }
                else
                {
                    sql = "select *, (select 1 from dual) as usertype from users a join applicationusers b on a.userid = b.userid where b.applicationid = " + ConfigurationManager.AppSettings["ChatApplicationID"].ToString() + " order by username asc";
                }

                cmd = new MySqlCommand(sql, cn);
                rdr = cmd.ExecuteReader();

                tbl = new DataTable();

                tbl.Load(rdr);
                rdr.Close();
                rdr.Dispose();

                users = new List<ChatUser>();

                foreach (DataRow row in tbl.Rows)
                {
                    ChatUser user = new ChatUser();

                    user.UserId = Convert.ToInt32(row["userid"].ToString());
                    user.UserName = row["username"].ToString();
                    user.UserType = Convert.ToInt32(row["usertype"].ToString());

                    users.Add(user); 
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (rdr != null) rdr.Dispose();
                if (tbl != null) tbl.Dispose();
                if (cn != null) cn.Close(); cn.Dispose();
            }

            return users;
        }

        public static ChatUser GetChatUser(int userId, int userType)
        {
            ChatUser chatUser = null;

            MySqlConnection cn = null;
            MySqlCommand cmd = null;
            MySqlDataReader rdr = null;
            DataTable tbl = null;

            try
            {
                cn = createConnectionMySqlChat();

                string sql = "";
                
                if (userType == 1)
                {
                    sql = "select * from users where userid = " + userId;
                }
                else
                {
                    sql = "select * from groups where groupid = " + userId;
                }

                cmd = new MySqlCommand(sql, cn);
                rdr = cmd.ExecuteReader();

                tbl = new DataTable();

                tbl.Load(rdr);
                rdr.Close();
                rdr.Dispose();
                  
                if (tbl.Rows.Count > 0)
                {
                    chatUser = new ChatUser();

                    if (userType == 1)
                    {
                        chatUser.UserId = Convert.ToInt32(tbl.Rows[0]["userid"]);
                        chatUser.UserName = tbl.Rows[0]["username"].ToString();
                    }
                    else
                    {
                        chatUser.UserId = Convert.ToInt32(tbl.Rows[0]["groupid"]);
                        chatUser.UserName = tbl.Rows[0]["desc"].ToString();
                    }

                    chatUser.UserType = userType;
                }
                
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (rdr != null) rdr.Dispose();
                if (tbl != null) tbl.Dispose();
                if (cn != null) cn.Close(); cn.Dispose();
            }

            return chatUser;
        }

        public static List<ChatMessage> GetChatMessages(int recipientId)
        {
            List<ChatMessage> messages = null;
            MySqlConnection cn = null;
            MySqlCommand cmd = null;
            MySqlDataReader rdr = null;
            DataTable tbl = null;

            try
            {
                cn = createConnectionMySqlChat();

                string sql = "select * from messages where (senderid = " + recipientId + ") or (recipientid = " + recipientId + " and recipienttype = 1) or (recipientid in (select groupid from groupusers where userid = " + recipientId + ") and recipienttype = 2) order by timestamp asc";

                cmd = new MySqlCommand(sql, cn);
                rdr = cmd.ExecuteReader();

                tbl = new DataTable();

                tbl.Load(rdr);
                rdr.Close();
                rdr.Dispose();

                messages = new List<ChatMessage>();

                foreach (DataRow row in tbl.Rows)
                {
                    ChatMessage message = new ChatMessage();

                    message.MessageId = Convert.ToInt32(row["messageid"].ToString());
                    message.Recipient = GetChatUser(Convert.ToInt32(row["recipientid"].ToString()), Convert.ToInt32(row["recipienttype"].ToString()));
                    //message.RecipientType = Convert.ToInt32(row["recipienttype"].ToString());
                    message.Sender = GetChatUser(Convert.ToInt32(row["senderid"]), 1);
                    message.MessageText = row["messagetext"].ToString();
                    message.Timestamp = Convert.ToDateTime(row["timestamp"].ToString());

                    messages.Add(message);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (rdr != null) rdr.Dispose();
                if (tbl != null) tbl.Dispose();
                if (cn != null) cn.Close(); cn.Dispose();
            }

            return messages;
        }

        public static bool SendChatMessage(int senderId, int recipientId, int recipientType, string message)
        {
            bool sent = false;

            MySqlConnection cn = null;
            MySqlCommand cmd = null;
            MySqlDataReader rdr = null;
            DataTable tbl = null;

            int i;

            try
            {
                cn = createConnectionMySqlChat();

                cmd = cn.CreateCommand();

                cmd.CommandText = "sp_InsertMessage";
                cmd.CommandType = CommandType.StoredProcedure;

                MySqlCommandBuilder.DeriveParameters(cmd);

                cmd.Parameters["@p_recipientId"].Value = recipientId;
                cmd.Parameters["@p_recipientType"].Value = recipientType;
                cmd.Parameters["@p_senderId"].Value = senderId;
                cmd.Parameters["@p_message"].Value = message;
                cmd.Parameters["@p_messageTag"].Value = "";
                cmd.Parameters["@p_applicationId"].Value = int.TryParse(ConfigurationManager.AppSettings["ChatApplicationID"].ToString(), out i) ? i : 1;
                
                cmd.ExecuteNonQuery();

                sent = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (rdr != null) rdr.Dispose();
                if (tbl != null) tbl.Dispose();
                if (cn != null) cn.Close(); cn.Dispose();
            }

            return sent;
        }

        #endregion

    }
}
