using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class PetInfo : GameServerNetworkPacket
    {
        private readonly L2Summon _pet;

        public PetInfo(L2Summon pet)
        {
            _pet = pet;
        }

        protected internal override void Write()
        {
            WriteC(0xb1);
            WriteD(_pet.ObjectSummonType);
            WriteD(_pet.ObjId);
            int npcId = _pet.Template.NpcId;
            WriteD(npcId + 1000000);
            WriteD(0); // 1=attackable

            WriteD(_pet.X);
            WriteD(_pet.Y);
            WriteD(_pet.Z);
            WriteD(_pet.Heading);
            WriteD(0);

            double atkspd = _pet.CharacterStat.GetStat(EffectType.BAttackSpd);
            double spd = _pet.CharacterStat.GetStat(EffectType.PSpeed);
            double anim = (spd * 1f) / 130;
            double anim2 = (1.1 * atkspd) / 300;
            double runSpd = spd / anim;
            double walkSpd = (spd * .8) / anim;
            double cast = _pet.CharacterStat.GetStat(EffectType.BCastingSpd);
            WriteD(cast);
            WriteD(atkspd);
            WriteD(runSpd);
            WriteD(walkSpd);
            WriteD(50);
            WriteD(50);
            WriteD(0);
            WriteD(0);
            WriteD(0);
            WriteD(0);

            WriteF(anim);
            WriteF(anim2);

            WriteF(_pet.Template.CollisionRadius);
            WriteF(_pet.Template.CollisionHeight);
            WriteD(0); // right hand weapon
            WriteD(0); // body armor
            WriteD(0); // left hand weapon

            WriteC(_pet.Owner != null ? 1 : 0); // when pet is dead and player exit game, pet doesn't show master name
            WriteC(_pet.IsRunning);
            WriteC(_pet.isInCombat() ? 1 : 0); // attacking 1=true
            WriteC(_pet.Dead ? 1 : 0);
            WriteC(_pet.AppearMethod());
            WriteS(_pet.Name);
            WriteS(_pet.Title);
            WriteD(1); //show title?

            WriteD(_pet.GetPvPStatus());
            WriteD(_pet.GetKarma());
            WriteD(_pet.CurrentTime);
            WriteD(_pet.MaxTime);
            WriteD(_pet.CurHp);
            WriteD(_pet.CharacterStat.GetStat(EffectType.BMaxHp));
            WriteD(_pet.CurMp);
            WriteD(_pet.CharacterStat.GetStat(EffectType.BMaxMp));

            WriteD(_pet.StatusSp);
            WriteD(_pet.Level);
            WriteQ(_pet.StatusExp);
            WriteQ(_pet.GetExpCurrentLevel());
            WriteQ(_pet.GetExpToLevelUp());

            WriteD(_pet.CurrentWeight());
            WriteD(_pet.MaxWeight());
            WriteD(_pet.CharacterStat.GetStat(EffectType.PPhysicalAttack));
            WriteD(_pet.CharacterStat.GetStat(EffectType.PPhysicalDefense));
            WriteD(_pet.CharacterStat.GetStat(EffectType.PMagicalAttack));

            WriteD(_pet.CharacterStat.GetStat(EffectType.PMagicalDefense));
            WriteD(_pet.CharacterStat.GetStat(EffectType.BAccuracy));
            WriteD(_pet.CharacterStat.GetStat(EffectType.BEvasion));
            WriteD(_pet.CharacterStat.GetStat(EffectType.BCriticalRate));
            WriteD(runSpd);
            WriteD(atkspd);
            WriteD(cast);

            WriteD(_pet.AbnormalBitMask);
            WriteH(_pet.IsMountable());

            WriteC(0); // c2
            WriteH(0); // ??

            WriteC(_pet.TeamId);
            WriteD(_pet.Template.SsCount);
            WriteD(_pet.Template.SpsCount);
            WriteD(_pet.GetForm());
            WriteD(_pet.AbnormalBitMaskEx);
        }
    }
}