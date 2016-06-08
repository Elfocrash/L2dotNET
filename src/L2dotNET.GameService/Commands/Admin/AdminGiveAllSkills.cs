using System.Collections.Generic;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Commands.Admin
{
    class AdminGiveAllSkills : AAdminCommand
    {
        public AdminGiveAllSkills()
        {
            Cmd = "set_skill_all";
        }

        protected internal override void Use(L2Player admin, string alias)
        {
            if (!(admin.CurrentTarget is L2Player))
            {
                admin.sendMessage("target is not a player.");
                return;
            }

            L2Player target = (L2Player)admin.CurrentTarget;
            TAcquireSkillsEntry skills = TSkillTable.Instance.GetAllRegularSkills(target.ActiveClass.ClassId.Id);

            SortedList<int, TAcquireSkill> avail = new SortedList<int, TAcquireSkill>();
            Dictionary<int, int> updDel = new Dictionary<int, int>();

            int nextLvl = 800;
            foreach (TAcquireSkill e in skills.skills)
            {
                if (e.get_lv > target.Level)
                {
                    if (nextLvl > e.get_lv)
                        nextLvl = e.get_lv;
                    continue;
                }

                if (avail.ContainsKey(e.id))
                    continue;

                if (target._skills.ContainsKey(e.id))
                {
                    TSkill skill = target._skills[e.id];

                    if (skill.level >= e.lv)
                        continue;

                    if (!avail.ContainsKey(e.id))
                    {
                        avail.Add(e.id, e);
                        updDel.Add(e.id, e.lv);
                        break;
                    }
                }
                else
                    avail.Add(e.id, e);
            }

            //foreach (int a in updDel.Keys)
            //    target.removeSkill(a, true, false);
            //updDel.Clear();

            foreach (TAcquireSkill sk in avail.Values)
            {
                TSkill skill = TSkillTable.Instance.Get(sk.id, sk.lv);
                if (skill != null)
                    target.addSkill(skill, false, false);
                else
                    target.sendMessage("no skill #" + sk.id + "-" + sk.lv);
            }

            target.ActiveSkillTree = avail;
            target.sendPacket(new AcquireSkillList(0, target));

            target.updateSkillList();
            target.sendMessage("gor all skills [" + skills.skills.Count + "][" + avail.Count + "] for lv" + target.Level + ", class @" + target.ActiveClass.ClassId.Id);
        }
    }
}