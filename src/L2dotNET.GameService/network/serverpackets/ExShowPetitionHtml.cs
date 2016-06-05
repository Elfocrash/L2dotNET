namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExShowPetitionHtml : GameServerNetworkPacket
    {
        private readonly string Content;

        public ExShowPetitionHtml(string text)
        {
            Content = text;
        }

        protected internal override void write()
        {
            writeC(0xFE);
            writeH(0xB1);

            writeD(0);
            writeC(0);
            writeS(Content);
        }
    }
}