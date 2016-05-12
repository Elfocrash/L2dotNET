
namespace L2dotNET.GameService.network.l2send
{
    class ExPartyPetWindowDelete : GameServerNetworkPacket
    {
        private int petId;
        private int playerId;
        private string petName;

        public ExPartyPetWindowDelete(int petId, int playerId, string petName)
        {
            this.petId = petId;
            this.playerId = playerId;
            this.petName = petName;
        }

        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0x6a);
            writeD(petId);
            writeD(playerId);
            writeS(petName);
        }
    }
}
