using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
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
            writeD(pet.ObjId);
            writeD(pet.X);
            writeD(pet.Y);
            writeD(pet.Z);
            writeS("");
            writeD(pet.CurrentTime);
            writeD(pet.MaxTime);
            writeD(pet.CurHp);
            writeD(pet.CharacterStat.getStat(TEffectType.b_max_hp));
            writeD(pet.CurMp);
            writeD(pet.CharacterStat.getStat(TEffectType.b_max_mp));
            writeD(pet.Level);
            writeQ(pet.StatusExp);
            writeQ(pet.getExpCurrentLevel());
            writeQ(pet.getExpToLevelUp());
        }
    }
}