using System.Threading;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Npcs
{
    class BuffForQuestReward
    {
        public L2Character Cha;
        private readonly L2Npc _npc;
        private int _skillId;
        private readonly Skill _skill;

        public BuffForQuestReward(L2Npc npc, L2Character target, int skillId)
        {
            this._npc = npc;
            Cha = target;
            this._skillId = skillId;
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