namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExShowPetitionHtml : GameserverPacket
    {
        private readonly string _content;

        public ExShowPetitionHtml(string text)
        {
            _content = text;
        }

        protected internal override void Write()
        {
            WriteByte(0xFE);
            WriteShort(0xB1);

            WriteInt(0);
            WriteByte(0);
            WriteString(_content);
        }
    }
}