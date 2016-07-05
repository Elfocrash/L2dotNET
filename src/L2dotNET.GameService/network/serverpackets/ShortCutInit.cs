using System.Collections.Generic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Player.General;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ShortCutInit : GameServerNetworkPacket
    {
        private readonly List<L2Shortcut> _shortcuts;

        public ShortCutInit(L2Player player)
        {
            _shortcuts = player.Shortcuts;
        }

        protected internal override void Write()
        {
            WriteC(0x45);
            WriteD(_shortcuts.Count);

            foreach (L2Shortcut sc in _shortcuts)
            {
                WriteD(sc.Type);
                WriteD(sc.Slot + sc.Page * 12);

                switch (sc.Type)
                {
                    case L2Shortcut.TypeItem:
                        WriteD(sc.Id);
                        WriteD(0x01);
                        WriteD(-1); //getSharedReuseGroup
                        WriteD(0x00);
                        WriteD(0x00);
                        WriteD(0x00);
                        break;
                    case L2Shortcut.TypeSkill:
                        WriteD(sc.Id);
                        WriteD(sc.Level);
                        WriteC(0x00); // C5
                        WriteD(0x01); // C6
                        break;
                    default:
                        WriteD(sc.Id);
                        WriteD(0x01); // C6
                        break;
                }
            }
        }
    }
}