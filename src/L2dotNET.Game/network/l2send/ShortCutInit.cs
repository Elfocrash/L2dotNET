using System.Collections.Generic;
using L2dotNET.Game.model.player;

namespace L2dotNET.Game.network.l2send
{
    class ShortCutInit : GameServerNetworkPacket
    {
        private List<L2Shortcut> _shortcuts;
        public ShortCutInit(L2Player player)
        {
            _shortcuts = player._shortcuts;
        }

        protected internal override void write()
        {
            writeC(0x45);
            writeD(_shortcuts.Count);

            foreach (L2Shortcut sc in _shortcuts)
            {
                writeD(sc._type);
                writeD(sc._slot + sc._page * 12);

                switch (sc._type)
                {
                    case L2Shortcut.TYPE_ITEM:
                        writeD(sc._id);
                        writeD(0x01);
                        writeD(-1); //getSharedReuseGroup
                        writeD(0x00);
                        writeD(0x00);
                        writeD(0x00);
                        break;
                    case L2Shortcut.TYPE_SKILL:
                        writeD(sc._id);
                        writeD(sc._level);
                        writeC(0x00); // C5 
                        writeD(0x01); // C6 
                        break;
                    default:
                        writeD(sc._id);
                        writeD(0x01); // C6 
                        break;
                }
            }
        }
    }
}
