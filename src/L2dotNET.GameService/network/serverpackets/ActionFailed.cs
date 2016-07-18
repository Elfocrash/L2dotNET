namespace L2dotNET.GameService.Network.Serverpackets
{
    class ActionFailed : GameserverPacket
    {
        protected internal override void Write()
        {
            WriteByte(0x25);
        }
    }
}