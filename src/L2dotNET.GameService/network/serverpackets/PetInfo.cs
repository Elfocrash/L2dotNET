using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PetInfo : GameserverPacket
    {
        private readonly L2Summon _pet;

        public PetInfo(L2Summon pet)
        {
            _pet = pet;
        }

        public override void Write()
        {
            WriteByte(0xb1);
            WriteInt(_pet.ObjectSummonType);
            WriteInt(_pet.ObjId);
            int npcId = _pet.Template.NpcId;
            WriteInt(npcId + 1000000);
            WriteInt(0); // 1=attackable

            WriteInt(_pet.X);
            WriteInt(_pet.Y);
            WriteInt(_pet.Z);
            WriteInt(_pet.Heading);
            WriteInt(0);

            double atkspd = _pet.CharacterStat.GetStat(EffectType.BAttackSpd);
            double spd = _pet.CharacterStat.GetStat(EffectType.PSpeed);
            double anim = (spd * 1f) / 130;
            double anim2 = (1.1 * atkspd) / 300;
            double runSpd = spd / anim;
            double walkSpd = (spd * .8) / anim;
            double cast = _pet.CharacterStat.GetStat(EffectType.BCastingSpd);
            WriteInt(cast);
            WriteInt(atkspd);
            WriteInt(runSpd);
            WriteInt(walkSpd);
            WriteInt(50);
            WriteInt(50);
            WriteInt(0);
            WriteInt(0);
            WriteInt(0);
            WriteInt(0);

            WriteDouble(anim);
            WriteDouble(anim2);

            WriteDouble(_pet.Template.CollisionRadius);
            WriteDouble(_pet.Template.CollisionHeight);
            WriteInt(0); // right hand weapon
            WriteInt(0); // body armor
            WriteInt(0); // left hand weapon

            WriteByte(_pet.Owner != null ? 1 : 0); // when pet is dead and player exit game, pet doesn't show master name
            WriteByte(_pet.IsRunning);
            WriteByte(_pet.isInCombat() ? 1 : 0); // attacking 1=true
            WriteByte(_pet.Dead ? 1 : 0);
            WriteByte(_pet.AppearMethod());
            WriteString(_pet.Name);
            WriteString(_pet.Title);
            WriteInt(1); //show title?

            WriteInt(_pet.GetPvPStatus());
            WriteInt(_pet.GetKarma());
            WriteInt(_pet.CurrentTime);
            WriteInt(_pet.MaxTime);
            WriteInt(_pet.CurHp);
            WriteInt(_pet.CharacterStat.GetStat(EffectType.BMaxHp));
            WriteInt(_pet.CurMp);
            WriteInt(_pet.CharacterStat.GetStat(EffectType.BMaxMp));

            WriteInt(_pet.StatusSp);
            WriteInt(_pet.Level);
            WriteLong(_pet.StatusExp);
            WriteLong(_pet.GetExpCurrentLevel());
            WriteLong(_pet.GetExpToLevelUp());

            WriteInt(_pet.CurrentWeight());
            WriteInt(_pet.MaxWeight());
            WriteInt(_pet.CharacterStat.GetStat(EffectType.PPhysicalAttack));
            WriteInt(_pet.CharacterStat.GetStat(EffectType.PPhysicalDefense));
            WriteInt(_pet.CharacterStat.GetStat(EffectType.PMagicalAttack));

            WriteInt(_pet.CharacterStat.GetStat(EffectType.PMagicalDefense));
            WriteInt(_pet.CharacterStat.GetStat(EffectType.BAccuracy));
            WriteInt(_pet.CharacterStat.GetStat(EffectType.BEvasion));
            WriteInt(_pet.CharacterStat.GetStat(EffectType.BCriticalRate));
            WriteInt(runSpd);
            WriteInt(atkspd);
            WriteInt(cast);

            WriteInt(_pet.AbnormalBitMask);
            WriteShort(_pet.IsMountable());

            WriteByte(0); // c2
            WriteShort(0); // ??

            WriteByte(_pet.TeamId);
            WriteInt(_pet.Template.SsCount);
            WriteInt(_pet.Template.SpsCount);
            WriteInt(_pet.GetForm());
            WriteInt(_pet.AbnormalBitMaskEx);
        }
    }
}