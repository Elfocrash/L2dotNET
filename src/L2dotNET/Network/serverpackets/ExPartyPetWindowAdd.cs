namespace L2dotNET.Network.serverpackets
{
    class ExPartyPetWindowAdd : GameserverPacket
    {
        
        public ExPartyPetWindowAdd()
        {
        }

        public override void Write()
        {
            WriteByte(0xfe);
            WriteShort(0x18);
            
        }
    }
}