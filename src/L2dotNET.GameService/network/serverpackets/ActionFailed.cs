namespace L2dotNET.GameService.Network.Serverpackets
{
    class ActionFailed : GameServerNetworkPacket
    {
        protected internal override void Write()
        {
            WriteC(0x25);
        }
    }
}