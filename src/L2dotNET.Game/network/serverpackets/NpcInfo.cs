using L2dotNET.GameService.model.npcs;
using L2dotNET.GameService.model.skills2;

namespace L2dotNET.GameService.network.l2send
{
    class NpcInfo : GameServerNetworkPacket
    {
        private L2Npc npc;
        public NpcInfo(L2Npc npc)
        {
            this.npc = npc;
        }

        protected internal override void write()
        {
            writeC(0x0c);
            writeD(npc.ObjID);
            writeD(npc.NpcHashId);
            writeD(npc.Attackable);
            writeD(npc.X);
            writeD(npc.Y);
            writeD(npc.Z);
            writeD(npc.Heading);
            writeD(0x00);

            double spd = npc.CharacterStat.getStat(TEffectType.p_speed);
            double atkspd = npc.CharacterStat.getStat(TEffectType.b_attack_spd);
            double cast = npc.CharacterStat.getStat(TEffectType.b_casting_spd);
            double anim = spd * 1f / 120;
            double anim2 = (1.1) * atkspd / 277;

            writeD(cast);
            writeD(atkspd);
            writeD(spd);
            writeD(spd * .8);
            writeD(0);  // swimspeed
            writeD(0);  // swimspeed
            writeD(0);
            writeD(0);
            writeD(0);
            writeD(0);

            writeF(anim);
            writeF(anim2);
            writeF(npc.Radius);
            writeF(npc.Height);
            writeD(npc.Template.RHand); // right hand weapon
            writeD(0);
            writeD(npc.Template.LHand); // left hand weapon
            writeC(1);	// name above char 1=true ... ??
            writeC(npc.isRunning());
            writeC(npc.isInCombat() ? 1 : 0);
            writeC(npc.isAlikeDead());
            writeC(npc._summoned ? 2 : 0); // invisible ?? 0=false  1=true   2=summoned (only works if model has a summon animation)
            writeS(npc.Name);
            writeS(npc.Template.Title);
            writeD(0x00); // Title color 0=client default
            writeD(0x00); //pvp flag
            writeD(0x00); // karma

            writeD(npc.AbnormalBitMask);
            writeD(npc.ClanId); 
            writeD(npc.ClanCrestId);
            writeD(npc.AllianceId);
            writeD(npc.AllianceCrestId);
            writeC(npc.isFlying() ? 2 : 0); // C2

            writeC(npc.TeamID);
            writeF(npc.Template.CollisionRadius);
            writeF(npc.Template.CollisionHeight);
            writeD(0); // enchant
            writeD(npc.isFlying() ? 1 : 0); // C6
            writeD(0x00);
            writeD(0x00);  //red?
            writeC(0x01);
            writeC(0x01);
            writeD(npc.AbnormalBitMaskEx);
            writeD(0x00);//freya
        }
    }
}
