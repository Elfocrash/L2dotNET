using System.Threading;
using L2dotNET.model.skills2;
using L2dotNET.Network.serverpackets;
using L2dotNET.world;

namespace L2dotNET.model.npcs
{
    class BuffForQuestReward
    {
        public L2Character Cha;
        private readonly L2Npc _npc;
        private int _skillId;
        private readonly Skill _skill;

        public BuffForQuestReward(L2Npc npc, L2Character target, int skillId)
        {
            _npc = npc;
            Cha = target;
            _skillId = skillId;
            _skill = SkillTable.Instance.Get(skillId);
            Cha.BroadcastPacket(new MagicSkillUse(npc, Cha, _skill, _skill.SkillHitTime));

            new Thread(Run).Start();
        }

        private void Run()
        {
            if ((Cha == null) || (_npc == null) || (_skill == null))
                return;

            Thread.Sleep(_skill.SkillHitTime);
            Cha.AddEffect(_npc, _skill, true, false);
        }
    }
}