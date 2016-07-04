using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Npcs.Cubic.Data
{
    public class heal_cubic : CubicTemplate
    {
        public heal_cubic(int id, byte lv, int skillId, int skilllv, int duration = 900)
        {
            this.id = id;
            level = lv;
            skill1 = TSkillTable.Instance.Get(skillId, skilllv);
            delay = 13;
            this.duration = duration;
            max_count = 20;
        }

        public override int AiActionTask(L2Player owner)
        {
            if (owner.Dead || (owner.CurHp / owner.MaxHp > 0.9))
                return 0;

            owner.AddAbnormal(skill1, owner, true, false);
            owner.BroadcastPacket(new MagicSkillUse(owner, owner, skill1, skill1.skill_hit_time));
            return 1;
        }
    }
}