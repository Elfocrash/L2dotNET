namespace L2dotNET.GameService.Network.Serverpackets
{
    class Calculator : GameserverPacket
    {
        protected internal override void Write()
        {
            WriteByte(0xe2);
            WriteInt(4393);
        }
    }
}