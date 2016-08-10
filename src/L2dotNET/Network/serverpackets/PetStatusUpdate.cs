using L2dotNET.model.playable;
using L2dotNET.model.skills2;

namespace L2dotNET.Network.serverpackets
{
    class PetStatusUpdate : GameserverPacket
    {
        private readonly L2Summon _pet;

        public PetStatusUpdate(L2Summon pet)
        {
            _pet = pet;
        }

        public override void Write()
        {
            WriteByte(0xb6);
            WriteInt(_pet.ObjectSummonType);
            WriteInt(_pet.ObjId);
            WriteInt(_pet.X);
            WriteInt(_pet.Y);
            WriteInt(_pet.Z);
            WriteString(string.Empty);
            WriteInt(_pet.CurrentTime);
            WriteInt(_pet.MaxTime);
            WriteInt(_pet.CurHp);
            WriteInt(_pet.CharacterStat.GetStat(EffectType.BMaxHp));
            WriteInt(_pet.CurMp);
            WriteInt(_pet.CharacterStat.GetStat(EffectType.BMaxMp));
            WriteInt(_pet.Level);
            WriteLong(_pet.StatusExp);
            WriteLong(_pet.GetExpCurrentLevel());
            WriteLong(_pet.GetExpToLevelUp());
        }
    }
}