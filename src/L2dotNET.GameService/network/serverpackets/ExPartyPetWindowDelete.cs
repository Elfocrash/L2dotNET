namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExPartyPetWindowDelete : GameserverPacket
    {
        private readonly int _petId;
        private readonly int _playerId;
        private readonly string _petName;

        public ExPartyPetWindowDelete(int petId, int playerId, string petName)
        {
            _petId = petId;
            _playerId = playerId;
            _petName = petName;
        }

        protected internal override void Write()
        {
            WriteByte(0xfe);
            WriteShort(0x6a);
            WriteInt(_petId);
            WriteInt(_playerId);
            WriteString(_petName);
        }
    }
}