using System.Collections.Generic;
using L2dotNET.Models.Player;
using L2dotNET.Models.Player.General;

namespace L2dotNET.Network.serverpackets
{
    class ShortCutInit : GameserverPacket
    {
        private readonly List<L2Shortcut> _shortcuts;

        public ShortCutInit(L2Player player)
        {
            _shortcuts = player.Shortcuts;
        }

        public override void Write()
        {
            WriteByte(0x45);
            WriteInt(_shortcuts.Count);

            foreach (L2Shortcut sc in _shortcuts)
            {
                WriteInt(sc.Type);
                WriteInt(sc.Slot + (sc.Page * 12));

                switch (sc.Type)
                {
                    case L2Shortcut.TypeItem:
                        WriteInt(sc.Id);
                        WriteInt(0x01);
                        WriteInt(-1); //getSharedReuseGroup
                        WriteInt(0x00);
                        WriteInt(0x00);
                        WriteInt(0x00);
                        break;
                    case L2Shortcut.TypeSkill:
                        WriteInt(sc.Id);
                        WriteInt(sc.Level);
                        WriteByte(0x00); // C5
                        WriteInt(0x01); // C6
                        break;
                    default:
                        WriteInt(sc.Id);
                        WriteInt(0x01); // C6
                        break;
                }
            }
        }
    }
}