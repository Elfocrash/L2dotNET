namespace L2dotNET.GameService.Network.Serverpackets
{
    class PartySmallWindowDeleteAll : GameserverPacket
    {
        protected internal override void Write()
        {
            WriteByte(0x50);
        }
    }
}