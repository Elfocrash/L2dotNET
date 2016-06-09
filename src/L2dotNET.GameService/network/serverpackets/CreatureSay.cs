using L2dotNET.GameService.Model.Player.Basic;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class CreatureSay : GameServerNetworkPacket
    {
        private readonly int _objectId;
        private readonly SayIDList _type;
        private readonly string _charName;
        public string Text { get; set; }

        public CreatureSay(int id, SayIDList _type, string name, string _text)
        {
            _objectId = id;
            this._type = _type;
            _charName = name;
            Text = _text;
        }

        public CreatureSay(SayIDList _type, string _text = "")
        {
            this._type = _type;
            Text = _text;
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