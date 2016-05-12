using L2dotNET.GameService.model.playable;
using L2dotNET.GameService.model.skills2;

namespace L2dotNET.GameService.network.l2send
{
    class PetInfo : GameServerNetworkPacket
    {
        private L2Summon pet;
        public PetInfo(L2Summon pet)
        {
            this.pet = pet;
        }

        protected internal override void write()
        {
            writeC(0xb1);
            writeD(pet.ObjectSummonType);
            writeD(pet.ObjID);
            int npcId = pet.Template.NpcId;
            writeD(npcId + 1000000);
            writeD(0);    // 1=attackable

            writeD(pet.X);
            writeD(pet.Y);
            writeD(pet.Z);
            writeD(pet.Heading);
            writeD(0);

            double atkspd = pet.CharacterStat.getStat(TEffectType.b_attack_spd);
            double spd = pet.CharacterStat.getStat(TEffectType.p_speed);
            double anim = spd * 1f / 130;
            double anim2 = (1.1) * atkspd / 300;
            double runSpd = spd / anim;
            double walkSpd = spd * .8 / anim;
            double cast = pet.CharacterStat.getStat(TEffectType.b_casting_spd);
            writeD(cast);
            writeD(atkspd);
            writeD(runSpd);
            writeD(walkSpd);
            writeD(50);
            writeD(50);
            writeD(0);
            writeD(0);
            writeD(0);
            writeD(0);

            writeF(anim);
            writeF(anim2);

            writeF(pet.Template.CollisionRadius);
            writeF(pet.Template.CollisionHeight);
            writeD(0); // right hand weapon
            writeD(0); // body armor
            writeD(0); // left hand weapon

            writeC(pet.Owner != null ? 1 : 0);	// when pet is dead and player exit game, pet doesn't show master name
            writeC(pet.IsRunning);
            writeC(pet.isInCombat() ? 1 : 0);	// attacking 1=true
            writeC(pet.Dead ? 1 : 0);
            writeC(pet.AppearMethod()); 
            writeS(pet.Name);
            writeS(pet.Title);
            writeD(1); //show title?

            writeD(pet.getPvPStatus());
            writeD(pet.getKarma());
            writeD(pet.CurrentTime);
            writeD(pet.MaxTime); 
            writeD(pet.CurHP);
            writeD(pet.CharacterStat.getStat(TEffectType.b_max_hp));
            writeD(pet.CurMP);
            writeD(pet.CharacterStat.getStat(TEffectType.b_max_mp));

            writeD(pet.StatusSP);
            writeD(pet.Level);
            writeQ(pet.StatusExp);
            writeQ(pet.getExpCurrentLevel());
            writeQ(pet.getExpToLevelUp());

            writeD(pet.CurrentWeight());
            writeD(pet.MaxWeight());
		    writeD(pet.CharacterStat.getStat(TEffectType.p_physical_attack));
		    writeD(pet.CharacterStat.getStat(TEffectType.p_physical_defense));
            writeD(pet.CharacterStat.getStat(TEffectType.p_magical_attack)); ;
		    writeD(pet.CharacterStat.getStat(TEffectType.p_magical_defense));
            writeD(pet.CharacterStat.getStat(TEffectType.b_accuracy));
            writeD(pet.CharacterStat.getStat(TEffectType.b_evasion));
            writeD(pet.CharacterStat.getStat(TEffectType.b_critical_rate));
		    writeD(runSpd);
		    writeD(atkspd);
            writeD(cast);

            writeD(pet.AbnormalBitMask);
            writeH(pet.IsMountable());

            writeC(0); // c2
            writeH(0); // ??

            writeC(pet.TeamID);
            writeD(pet.Template.soulshot_count);
            writeD(pet.Template.spiritshot_count);
            writeD(pet.getForm());
            writeD(pet.AbnormalBitMaskEx);
        }
    }
}
