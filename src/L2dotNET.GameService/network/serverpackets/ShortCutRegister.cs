using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ShortCutRegister : GameServerNetworkPacket
    {
        private readonly L2Shortcut _cut;

        public ShortCutRegister(L2Shortcut cut)
        {
            this._cut = cut;
        }

        protected internal override void Write()
        {
            WriteC(0x44);

            WriteD(_cut.Type);
            WriteD(_cut.Slot + _cut.Page * 12);

            switch (_cut.Type)
            {
                case L2Shortcut.TypeItem:
                    WriteD(_cut.Id);
                    WriteD(_cut.CharacterType);
                    WriteD(-1); //getSharedReuseGroup
                    break;
                case L2Shortcut.TypeSkill:
                    WriteD(_cut.Id);
                    WriteD(_cut.Level);
                    WriteC(0x00); // C5
                    WriteD(_cut.CharacterType);
                    break;
                default:
                    WriteD(_cut.Id);
                    WriteD(_cut.CharacterType);
                    break;
            }
        }
    }
}