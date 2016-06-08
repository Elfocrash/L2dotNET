using System.Threading;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Npcs
{
    class BuffForQuestReward
    {
        public L2Character cha;
        private readonly L2Npc npc;
        private int skillId;
        private readonly TSkill skill;

        public BuffForQuestReward(L2Npc npc, L2Character target, int skillId)
        {
            this.npc = npc;
            cha = target;
            this.skillId = skillId;
            skill = TSkillTable.Instance.Get(skillId);
            cha.broadcastPacket(new MagicSkillUse(npc, cha, skill, skill.skill_hit_time));

            new Thread(Run).Start();
        }

        private void Run()
        {
            if ((cha == null) || (npc == null) || (skill == null))
                return;

            Thread.Sleep(skill.skill_hit_time);
            cha.addEffect(npc, skill, true, false);
        }
    }
}