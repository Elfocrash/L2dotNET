using L2dotNET.GameService.model.player;

namespace L2dotNET.GameService.network.l2send
{
    class ShortCutRegister : GameServerNetworkPacket
    {
        private readonly L2Shortcut cut;
        public ShortCutRegister(L2Shortcut cut)
        {
            this.cut = cut;
        }

        protected internal override void write()
        {
            writeC(0x44);

            writeD(cut.Type);
            writeD(cut.Slot + cut.Page * 12);

            switch (cut.Type)
            {
                case L2Shortcut.TYPE_ITEM:
                    writeD(cut.Id);
                    writeD(cut.CharacterType);
                    writeD(-1); //getSharedReuseGroup
                    break;
                case L2Shortcut.TYPE_SKILL:
                    writeD(cut.Id);
                    writeD(cut.Level);
                    writeC(0x00); // C5 
                    writeD(cut.CharacterType);
                    break;
                default:
                    writeD(cut.Id);
                    writeD(cut.CharacterType);
                    break;
            }
        }
    }
}
