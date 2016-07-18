namespace L2dotNET.GameService.Network.Serverpackets
{
    class CharDeleteOk : GameserverPacket
    {
        protected internal override void Write()
        {
            WriteByte(0x23);
        }
    }
}