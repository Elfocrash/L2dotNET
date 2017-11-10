namespace L2dotNET.Network.serverpackets
{
    class PetStatusUpdate : GameserverPacket
    {
        public PetStatusUpdate()
        {
        }

        public override void Write()
        {
            WriteByte(0xb6);
        }
    }
}