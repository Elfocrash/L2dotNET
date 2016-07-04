using L2dotNET.GameService.Model.Player.Basic;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class CreatureSay : GameServerNetworkPacket
    {
        private readonly int _objectId;
        private readonly SayIDList _type;
        private readonly string _charName;
        public string Text { get; set; }

        public CreatureSay(int id, SayIDList _type, string name, string text)
        {
            _objectId = id;
            this._type = _type;
            _charName = name;
            Text = text;
        }

        public CreatureSay(SayIDList _type, string text = "")
        {
            this._type = _type;
            Text = text;
        }

        protected internal override void write()
        {
            writeC(0x4a);
            writeD(_objectId);
            writeD((byte)_type);
            writeS(_charName);
            writeS(Text);
        }
    }
}