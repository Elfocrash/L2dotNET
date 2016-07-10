using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class NpcInfo : GameServerNetworkPacket
    {
        private readonly L2Npc _npc;

        public NpcInfo(L2Npc npc)
        {
            _npc = npc;
        }

        protected internal override void Write()
        {
            WriteC(0x0c);
            WriteD(_npc.ObjId);
            WriteD(_npc.NpcHashId);
            WriteD(_npc.Attackable);
            WriteD(_npc.X);
            WriteD(_npc.Y);
            WriteD(_npc.Z);
            WriteD(_npc.Heading);
            WriteD(0x00);

            double spd = _npc.CharacterStat.GetStat(EffectType.PSpeed);
            double atkspd = _npc.CharacterStat.GetStat(EffectType.BAttackSpd);
            double cast = _npc.CharacterStat.GetStat(EffectType.BCastingSpd);
            double anim = (spd * 1f) / 120;
            double anim2 = (1.1 * atkspd) / 277;

            WriteD(cast);
            WriteD(atkspd);
            WriteD(spd);
            WriteD(spd * .8);
            WriteD(0); // swimspeed
            WriteD(0); // swimspeed
            WriteD(0);
            WriteD(0);
            WriteD(0);
            WriteD(0);

            WriteF(anim);
            WriteF(anim2);
            WriteF(_npc.Radius);
            WriteF(_npc.Height);
            WriteD(_npc.Template.RHand); // right hand weapon
            WriteD(0);
            WriteD(_npc.Template.LHand); // left hand weapon
            WriteC(1); // name above char 1=true ... ??
            WriteC(_npc.isRunning());
            WriteC(_npc.isInCombat() ? 1 : 0);
            WriteC(_npc.IsAlikeDead());
            WriteC(_npc.Summoned ? 2 : 0); // invisible ?? 0=false  1=true   2=summoned (only works if model has a summon animation)
            WriteS(_npc.Name);
            WriteS(_npc.Template.Title);
            WriteD(0x00); // Title color 0=client default
            WriteD(0x00); //pvp flag
            WriteD(0x00); // karma

            WriteD(_npc.AbnormalBitMask);
            WriteD(_npc.ClanId);
            WriteD(_npc.ClanCrestId);
            WriteD(_npc.AllianceId);
            WriteD(_npc.AllianceCrestId);
            WriteC(_npc.IsFlying() ? 2 : 0); // C2

            WriteC(_npc.TeamId);
            WriteF(_npc.Template.CollisionRadius);
            WriteF(_npc.Template.CollisionHeight);
            WriteD(0); // enchant
            WriteD(_npc.IsFlying() ? 1 : 0); // C6
            WriteD(0x00);
            WriteD(0x00); //red?
            WriteC(0x01);
            WriteC(0x01);
            WriteD(_npc.AbnormalBitMaskEx);
            WriteD(0x00); //freya
        }
    }
}