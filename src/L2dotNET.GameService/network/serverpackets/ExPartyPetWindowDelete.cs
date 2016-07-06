namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExPartyPetWindowDelete : GameServerNetworkPacket
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
            WriteC(0xfe);
            WriteH(0x6a);
            WriteD(_petId);
            WriteD(_playerId);
            WriteS(_petName);
        }
    }
}