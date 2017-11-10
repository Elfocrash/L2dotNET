namespace L2dotNET.Network.serverpackets
{
    class PetInfo : GameserverPacket
    {
        
        public PetInfo()
        {
        }

        public override void Write()
        {
            WriteByte(0xb1);
        }
    }
}