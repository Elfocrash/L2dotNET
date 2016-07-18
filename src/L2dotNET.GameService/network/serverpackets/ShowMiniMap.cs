using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ShowMiniMap : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0x9d);
            WriteInt(1665);
            WriteInt(0); //SevenSigns.getInstance().getCurrentPeriod());
        }
    }
}