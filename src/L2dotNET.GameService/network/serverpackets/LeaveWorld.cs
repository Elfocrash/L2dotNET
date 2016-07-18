using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class LeaveWorld : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0x7e);
        }
    }
}