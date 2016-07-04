namespace L2dotNET.GameService.Network.Serverpackets
{
    class PartySmallWindowDeleteAll : GameServerNetworkPacket
    {
        protected internal override void Write()
        {
            WriteC(0x50);
        }
    }
}