using System.Collections.Generic;
using L2dotNET.Game.model.inventory;

namespace L2dotNET.Game.managers
{
    public class MailManager
    {
        private static MailManager m = new MailManager();
        public static MailManager getInstance()
        {
            return m;
        }

        public SortedList<int, MailMessages> _mmessages = new SortedList<int, MailMessages>();

        public List<MailMessage> getInbox(int objId)
        {
            List<MailMessage> list = new List<MailMessage>();
            if (_mmessages.ContainsKey(objId))
            {
                MailMessages ms = _mmessages[objId];

                foreach (MailMessage mm in ms._messages.Values)
                {
                    if (mm._receiverId == objId)
                        list.Add(mm);
                }
            }

            return list;
        }

        public List<MailMessage> getOutbox(int objId)
        {
            List<MailMessage> list = new List<MailMessage>();
            if (_mmessages.ContainsKey(objId))
            {
                MailMessages ms = _mmessages[objId];

                foreach (MailMessage mm in ms._messages.Values)
                {
                    if (mm._senderId == objId)
                        list.Add(mm);
                }
            }

            return list;
        }

        public int getOutboxSize(int objId)
        {
            int count = 0;

            if (_mmessages.ContainsKey(objId))
            {
                MailMessages ms = _mmessages[objId];

                foreach (MailMessage mm in ms._messages.Values)
                {
                    if (mm._senderId == objId)
                        count++;
                }
            }

            return count;
        }

        public int getInboxSize(int objId)
        {
            int count = 0;

            if (_mmessages.ContainsKey(objId))
            {
                MailMessages ms = _mmessages[objId];

                foreach (MailMessage mm in ms._messages.Values)
                {
                    if (mm._receiverId == objId)
                        count++;
                }
            }

            return count;
        }

        public void register(MailMessage message)
        {
            if (!_mmessages.ContainsKey(message._senderId))
            {
                MailMessages mms = new MailMessages();
                mms._messages.Add(message.MailID, message);
                _mmessages.Add(message._senderId, mms);
            }
            else
            {
                _mmessages[message._senderId]._messages.Add(message.MailID, message);
            }
        }

        public MailMessage getMessageNotmy(int objId, int _msgId)
        {
            foreach (MailMessages mss in _mmessages.Values)
            {
                foreach (MailMessage mm in mss._messages.Values)
                {
                    if (mm.MailID == _msgId && mm._receiverId == objId)
                        return mm;
                }
            }

            return null;
        }

        public MailMessage getMessageMy(int objId, int _msgId)
        {
            if (_mmessages.ContainsKey(objId))
            {
                MailMessages ms = _mmessages[objId];

                foreach (MailMessage mm in ms._messages.Values)
                {
                    if (mm.MailID == _msgId)
                        return mm;
                }
            }

            return null;
        }
    }

    public class MailMessages
    {
        public SortedList<int, MailMessage> _messages = new SortedList<int, MailMessage>();
    }

    public class MailMessage
    {
        public int MailID;
        public string Title;
        public string SenderName;
        public int Trade;
        public int NotOpend;
        public int WithItem;
        public int _news;
        public string ReceiverName;
     //   public int _isUnread;
        public int ReturnAble = 1;
        public int SentBySystem = 0;

        public int _senderId, _receiverId;
        public string Content;
        public InvMail Inventory;
        public long PaymentAdena = 0;

        public int getExpirationSeconds()
        {
            return 0;
        }
    }
}
