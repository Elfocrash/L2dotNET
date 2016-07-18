using L2dotNET.GameService.Model.Player.General;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ShortCutRegister : GameserverPacket
    {
        private readonly L2Shortcut _cut;

        public ShortCutRegister(L2Shortcut cut)
        {
            _cut = cut;
        }

        protected internal override void Write()
        {
            WriteByte(0x44);

            WriteInt(_cut.Type);
            WriteInt(_cut.Slot + (_cut.Page * 12));

            switch (_cut.Type)
            {
                case L2Shortcut.TypeItem:
                    WriteInt(_cut.Id);
                    WriteInt(_cut.CharacterType);
                    WriteInt(-1); //getSharedReuseGroup
                    break;
                case L2Shortcut.TypeSkill:
                    WriteInt(_cut.Id);
                    WriteInt(_cut.Level);
                    WriteByte(0x00); // C5
                    WriteInt(_cut.CharacterType);
                    break;
                default:
                    WriteInt(_cut.Id);
                    WriteInt(_cut.CharacterType);
                    break;
            }
        }
    }
}