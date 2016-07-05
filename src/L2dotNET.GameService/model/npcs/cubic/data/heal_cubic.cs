using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Npcs.Cubic.Data
{
    public class HealCubic : CubicTemplate
    {
        public HealCubic(int id, byte lv, int skillId, int skilllv, int duration = 900)
        {
            this.Id = id;
            Level = lv;
            Skill1 = SkillTable.Instance.Get(skillId, skilllv);
            Delay = 13;
            this.Duration = duration;
            MaxCount = 20;
        }

        public override int AiActionTask(L2Player owner)
        {
            if (owner.Dead || ((owner.CurHp / owner.MaxHp) > 0.9))
            {
                return 0;
            }

            owner.AddAbnormal(Skill1, owner, true, false);
            owner.BroadcastPacket(new MagicSkillUse(owner, owner, Skill1, Skill1.SkillHitTime));
            return 1;
        }
    }
}