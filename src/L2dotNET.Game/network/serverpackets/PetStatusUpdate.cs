using L2dotNET.GameService.model.playable;
using L2dotNET.GameService.model.skills2;

namespace L2dotNET.GameService.network.l2send
{
    class PetStatusUpdate : GameServerNetworkPacket
    {
        private readonly L2Summon pet;
        public PetStatusUpdate(L2Summon pet)
        {
            this.pet = pet;
        }

        protected internal override void write()
        {
            writeC(0xb6);
            writeD(pet.ObjectSummonType);
            writeD(pet.ObjID);
            writeD(pet.X);
            writeD(pet.Y);
            writeD(pet.Z);
            writeS("");
            writeD(pet.CurrentTime);
            writeD(pet.MaxTime);
            writeD(pet.CurHP);
            writeD(pet.CharacterStat.getStat(TEffectType.b_max_hp));
            writeD(pet.CurMP);
            writeD(pet.CharacterStat.getStat(TEffectType.b_max_mp));
            writeD(pet.Level);
            writeQ(pet.StatusExp);
            writeQ(pet.getExpCurrentLevel());
            writeQ(pet.getExpToLevelUp());
        }
    }
}
