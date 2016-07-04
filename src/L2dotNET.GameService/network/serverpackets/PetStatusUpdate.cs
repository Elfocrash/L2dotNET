using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PetStatusUpdate : GameServerNetworkPacket
    {
        private readonly L2Summon _pet;

        public PetStatusUpdate(L2Summon pet)
        {
            this._pet = pet;
        }

        protected internal override void Write()
        {
            WriteC(0xb6);
            WriteD(_pet.ObjectSummonType);
            WriteD(_pet.ObjId);
            WriteD(_pet.X);
            WriteD(_pet.Y);
            WriteD(_pet.Z);
            WriteS("");
            WriteD(_pet.CurrentTime);
            WriteD(_pet.MaxTime);
            WriteD(_pet.CurHp);
            WriteD(_pet.CharacterStat.GetStat(EffectType.BMaxHp));
            WriteD(_pet.CurMp);
            WriteD(_pet.CharacterStat.GetStat(EffectType.BMaxMp));
            WriteD(_pet.Level);
            WriteQ(_pet.StatusExp);
            WriteQ(_pet.GetExpCurrentLevel());
            WriteQ(_pet.GetExpToLevelUp());
        }
    }
}