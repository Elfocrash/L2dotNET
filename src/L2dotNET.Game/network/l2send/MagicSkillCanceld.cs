
namespace L2dotNET.Game.network.l2send
{
    public class MagicSkillCanceld : GameServerNetworkPacket
    {
        private int _id;
        public MagicSkillCanceld(int id)
        {
            _id = id;
        }

        protected internal override void write()
        {
            writeC(0x49);
            writeD(_id);
        }
    }
}
