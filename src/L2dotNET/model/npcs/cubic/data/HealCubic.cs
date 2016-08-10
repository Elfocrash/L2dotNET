using L2dotNET.model.player;
using L2dotNET.model.skills2;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.model.npcs.cubic.data
{
    public class HealCubic : CubicTemplate
    {
        public HealCubic(int id, byte lv, int skillId, int skilllv, int duration = 900)
        {
            Id = id;
            Level = lv;
            Skill1 = SkillTable.Instance.Get(skillId, skilllv);
            Delay = 13;
            Duration = duration;
            MaxCount = 20;
        }

        public override int AiActionTask(L2Player owner)
        {
            if (owner.Dead || ((owner.CurHp / owner.MaxHp) > 0.9))
                return 0;

            owner.AddAbnormal(Skill1, owner, true, false);
            owner.BroadcastPacket(new MagicSkillUse(owner, owner, Skill1, Skill1.SkillHitTime));
            return 1;
        }
    }
}