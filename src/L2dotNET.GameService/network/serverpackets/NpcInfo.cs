using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Skills2;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class NpcInfo : GameserverPacket
    {
        private readonly L2Npc _npc;

        public NpcInfo(L2Npc npc)
        {
            _npc = npc;
        }

        protected internal override void Write()
        {
            WriteByte(0x0c);
            WriteInt(_npc.ObjId);
            WriteInt(_npc.NpcHashId);
            WriteInt(_npc.Attackable);
            WriteInt(_npc.X);
            WriteInt(_npc.Y);
            WriteInt(_npc.Z);
            WriteInt(_npc.Heading);
            WriteInt(0x00);

            double spd = _npc.CharacterStat.GetStat(EffectType.PSpeed);
            double atkspd = _npc.CharacterStat.GetStat(EffectType.BAttackSpd);
            double cast = _npc.CharacterStat.GetStat(EffectType.BCastingSpd);
            double anim = (spd * 1f) / 120;
            double anim2 = (1.1 * atkspd) / 277;

            WriteInt(cast);
            WriteInt(atkspd);
            WriteInt(spd);
            WriteInt(spd * .8);
            WriteInt(0); // swimspeed
            WriteInt(0); // swimspeed
            WriteInt(0);
            WriteInt(0);
            WriteInt(0);
            WriteInt(0);

            WriteDouble(anim);
            WriteDouble(anim2);
            WriteDouble(_npc.Radius);
            WriteDouble(_npc.Height);
            WriteInt(_npc.Template.RHand); // right hand weapon
            WriteInt(0);
            WriteInt(_npc.Template.LHand); // left hand weapon
            WriteByte(1); // name above char 1=true ... ??
            WriteByte(_npc.isRunning());
            WriteByte(_npc.isInCombat() ? 1 : 0);
            WriteByte(_npc.IsAlikeDead());
            WriteByte(_npc.Summoned ? 2 : 0); // invisible ?? 0=false  1=true   2=summoned (only works if model has a summon animation)
            WriteString(_npc.Name);
            WriteString(_npc.Template.Title);
            WriteInt(0x00); // Title color 0=client default
            WriteInt(0x00); //pvp flag
            WriteInt(0x00); // karma

            WriteInt(_npc.AbnormalBitMask);
            WriteInt(_npc.ClanId);
            WriteInt(_npc.ClanCrestId);
            WriteInt(_npc.AllianceId);
            WriteInt(_npc.AllianceCrestId);
            WriteByte(_npc.IsFlying() ? 2 : 0); // C2

            WriteByte(_npc.TeamId);
            WriteDouble(_npc.Template.CollisionRadius);
            WriteDouble(_npc.Template.CollisionHeight);
            WriteInt(0); // enchant
            WriteInt(_npc.IsFlying() ? 1 : 0); // C6
            WriteInt(0x00);
            WriteInt(0x00); //red?
            WriteByte(0x01);
            WriteByte(0x01);
            WriteInt(_npc.AbnormalBitMaskEx);
            WriteInt(0x00); //freya
        }
    }
}