namespace L2dotNET.Network.serverpackets
{
    class ExShowPetitionHtml : GameserverPacket
    {
        private readonly string _content;

        public ExShowPetitionHtml(string text)
        {
            _content = text;
        }

        public override void Write()
        {
            WriteByte(0xFE);
            WriteShort(0xB1);

            WriteInt(0);
            WriteByte(0);
            WriteString(_content);
        }
    }
}