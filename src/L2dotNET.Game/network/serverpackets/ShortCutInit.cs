using System.Collections.Generic;
using L2dotNET.GameService.model.player;

namespace L2dotNET.GameService.network.l2send
{
    class ShortCutInit : GameServerNetworkPacket
    {
        private readonly List<L2Shortcut> _shortcuts;

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
                writeD(sc.Type);
                writeD(sc.Slot + sc.Page * 12);

                switch (sc.Type)
                {
                    case L2Shortcut.TYPE_ITEM:
                        writeD(sc.Id);
                        writeD(0x01);
                        writeD(-1); //getSharedReuseGroup
                        writeD(0x00);
                        writeD(0x00);
                        writeD(0x00);
                        break;
                    case L2Shortcut.TYPE_SKILL:
                        writeD(sc.Id);
                        writeD(sc.Level);
                        writeC(0x00); // C5 
                        writeD(0x01); // C6 
                        break;
                    default:
                        writeD(sc.Id);
                        writeD(0x01); // C6 
                        break;
                }
            }
        }
    }
}