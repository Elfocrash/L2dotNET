using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
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
            WriteString("");
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