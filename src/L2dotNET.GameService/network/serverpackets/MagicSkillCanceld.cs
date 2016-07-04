namespace L2dotNET.GameService.Network.Serverpackets
{
    public class MagicSkillCanceld : GameServerNetworkPacket
    {
        private readonly int _id;

        public MagicSkillCanceld(int id)
        {
            _id = id;
        }

        protected internal override void Write()
        {
            WriteC(0x49);
            WriteD(_id);
        }
    }
}