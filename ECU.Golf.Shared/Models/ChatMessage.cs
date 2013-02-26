using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECU.Golf.Shared.Models
{
    public class ChatMessage
    {
        #region Private Members

        private int _messageId;
        private ChatUser _recipient;
        //private int _recipientType;
        private ChatUser _sender;
        private string _messageText;
        private DateTime _timestamp;

        #endregion

        #region Properties

        public int MessageId
        {
            get { return _messageId; }
            set { _messageId = value; }
        }

        public ChatUser Recipient
        {
            get { return _recipient; }
            set { _recipient = value; }
        }

        //public int RecipientType
        //{
        //    get { return _recipientType; }
        //    set { _recipientType = value; }
        //}

        public ChatUser Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        public string MessageText
        {
            get { return _messageText; }
            set { _messageText = value; }
        }

        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        #endregion

    }
}
