using L2dotNET.Game.model.player.basic;

namespace L2dotNET.Game.network.l2send
{
    class CreatureSay : GameServerNetworkPacket
    {
        private int _objectId;
        private SayIDList _type;
        private string _charName;
        public string Text { get; set; }

        public CreatureSay(int id, SayIDList _type, string name, string _text)
        {
            this._objectId = id;
            this._type = _type;
            this._charName = name;
            this.Text = _text;
        }

        public CreatureSay(SayIDList _type, string _text = "")
        {
            this._type = _type;
            this.Text = _text;
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
