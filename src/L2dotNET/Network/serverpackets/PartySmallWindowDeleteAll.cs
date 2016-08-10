namespace L2dotNET.Network.serverpackets
{
    class PartySmallWindowDeleteAll : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0x50);
        }
    }
}