using System;
using System.Data;
using L2dotNET.Game.db;
using L2dotNET.Game.managers;
using L2dotNET.Game.model.inventory;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.tables;
using MySql.Data.MySqlClient;

namespace L2dotNET.Game.network.l2recv
{
    class RequestSendPost : GameServerNetworkRequest
    {
        private static readonly int BATCH_LENGTH = 12; // length of the one item

        private static readonly int MAX_RECV_LENGTH = 16;
        private static readonly int MAX_SUBJ_LENGTH = 128;
        private static readonly int MAX_TEXT_LENGTH = 512;
        private static readonly int MAX_ATTACHMENTS = 8;
        private static readonly int INBOX_SIZE = 240;
        private static readonly int OUTBOX_SIZE = 240;

        private static readonly int MESSAGE_FEE = 100;
        private static readonly int MESSAGE_FEE_PER_SLOT = 1000; // 100 adena message fee + 1000 per each item slot

        private string _receiver;
        private bool _payment;
        private string _subject;
        private string _text;
        private long[] _items;
        private long _reqAdena;

        public RequestSendPost(GameClient client, byte[] data)
        {
            base.makeme(client, data, 2);
        }

        public override void read()
        {
            _receiver = readS();
            _payment = readD() == 0 ? false : true;
            _subject = readS();
            _text = readS();

            int attachCount = readD();

            if (attachCount > 0)
            {
                _items = new long[attachCount * 2];

                for (int i = 0; i < attachCount; i++)
                {
                    _items[i * 2] = readD();
                    _items[i * 2 + 1] = readQ();
                }
            }

            _reqAdena = readQ();
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            if (!player.isInPeace())
            {
                player.sendSystemMessage(3066); //You cannot receive or send mail with attached items in non-peace zone regions.
                player.sendActionFailed();
                return;
            }

            //if (player._name == _receiver)
            //{
            //    player.sendSystemMessage(3019); //You cannot send a mail to yourself.
            //    player.sendActionFailed();
            //    return;
            //}

            if (_receiver.Length > MAX_RECV_LENGTH)
            {
                player.sendSystemMessage(3074); //The allowed length for recipient exceeded.
                player.sendActionFailed();
                return;
            }

            if (_subject.Length > MAX_SUBJ_LENGTH)
            {
                player.sendSystemMessage(3075); //The allowed length for a title exceeded.
                player.sendActionFailed();
                return;
            }

            if (_text.Length > MAX_TEXT_LENGTH)
            {
                player.sendSystemMessage(3076); //The allowed length for a title exceeded.
                player.sendActionFailed();
                return;
            }

            if (_items != null && _items.Length > MAX_ATTACHMENTS)
            {
                player.sendSystemMessage(3016); //Item selection is possible up to 8.
                player.sendActionFailed();
                return;
            }

            if (_reqAdena < 0)
            {
                player.sendActionFailed();
                return;
            }

            if (_payment)
            {
                if (_reqAdena == 0)
                {
                    player.sendSystemMessage(3020); //When not entering the amount for the payment request, you cannot send any mail.
                    player.sendActionFailed();
                    return;
                }

                if (_items.Length == 0)
                {
                    player.sendSystemMessage(2966); //It's a Payment Request transaction. Please attach the item.
                    player.sendActionFailed();
                    return;
                }
            }

            MySqlConnection connection = SQLjec.getInstance().conn();
            MySqlCommand cmd = connection.CreateCommand();
            connection.Open();

            cmd.CommandText = "SELECT objId FROM user_data WHERE pname = '" + _receiver + "'";
            cmd.CommandType = CommandType.Text;

            MySqlDataReader reader = cmd.ExecuteReader();
            int receiverId = 0;
            while (reader.Read())
            {
                receiverId = reader.GetInt32("objId");
            }

            reader.Close();
            connection.Close();

            if (receiverId == 0)
            {
                player.sendSystemMessage(3002); //When the recipient doesn't exist or the character has been deleted, sending mail is not possible.
                player.sendActionFailed();
                return;
            }

            MailManager mm = MailManager.getInstance();

            if (mm.getOutboxSize(player.ObjID) >= OUTBOX_SIZE)
            {
                player.sendSystemMessage(2968); //The mail limit (240) has been exceeded and this cannot be forwarded.
                player.sendActionFailed();
                return;
            }

            if (mm.getInboxSize(receiverId) >= INBOX_SIZE)
            {
                player.sendSystemMessage(3077); //The mail limit (240) of the opponent's character has been exceeded and this cannot be forwarded.
                player.sendActionFailed();
                return;
            }

            long fee = MESSAGE_FEE;

            if (player.getAdena() < fee)
            {
                player.sendSystemMessage(2975); //You cannot forward because you don't have enough adena.
                player.sendActionFailed();
                return;
            }

            MailMessage message = new MailMessage();
            message._senderId = player.ObjID;
            message.SenderName = player.Name;
            message.MailID = IdFactory.getInstance().nextId();
            message.NotOpend = 1;
            message.ReceiverName = _receiver;
            message._receiverId = receiverId;
            message.Title = _subject;
            message.Content = _text;
            message.WithItem = (_items == null || _items.Length == 0) ? 0 : 1;

            if (_items != null && _items.Length > 0)
            {
                message.Inventory = new InvMail(player);
            }

            mm.register(message);
            player.sendSystemMessage(3009); //Mail successfully sent.
            player.sendPacket(new ExNoticePostSent(1));
        }
    }
}
