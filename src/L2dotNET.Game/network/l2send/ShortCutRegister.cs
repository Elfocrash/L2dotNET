using L2dotNET.Game.model.player;

namespace L2dotNET.Game.network.l2send
{
    class ShortCutRegister : GameServerNetworkPacket
    {
        private L2Shortcut cut;
        public ShortCutRegister(L2Shortcut cut)
        {
            this.cut = cut;
        }

        protected internal override void write()
        {
            writeC(0x44);

            writeD(cut._type);
            writeD(cut._slot + cut._page * 12);

            switch (cut._type)
            {
                case L2Shortcut.TYPE_ITEM:
                    writeD(cut._id);
                    writeD(cut._characterType);
                    writeD(-1); //getSharedReuseGroup
                    writeD(0);
                    writeD(0);
                    writeD(0); // item augment id
                    break;
                case L2Shortcut.TYPE_SKILL:
                    writeD(cut._id);
                    writeD(cut._level);
                    writeC(0x00); // C5 
                    writeD(cut._characterType);
                    break;
                default:
                    writeD(cut._id);
                    writeD(cut._characterType);
                    break;
            }
        }
    }
}
