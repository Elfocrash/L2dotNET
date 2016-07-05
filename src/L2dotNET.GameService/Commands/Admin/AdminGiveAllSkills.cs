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
                admin.SendMessage("target is not a player.");
                return;
            }

            L2Player target = (L2Player)admin.CurrentTarget;
            AcquireSkillsEntry skills = SkillTable.Instance.GetAllRegularSkills(target.ActiveClass.ClassId.Id);

            SortedList<int, AcquireSkill> avail = new SortedList<int, AcquireSkill>();
            Dictionary<int, int> updDel = new Dictionary<int, int>();

            int nextLvl = 800;
            foreach (AcquireSkill e in skills.Skills)
            {
                if (e.GetLv > target.Level)
                {
                    if (nextLvl > e.GetLv)
                    {
                        nextLvl = e.GetLv;
                    }
                    continue;
                }

                if (avail.ContainsKey(e.Id))
                {
                    continue;
                }

                if (target.Skills.ContainsKey(e.Id))
                {
                    Skill skill = target.Skills[e.Id];

                    if (skill.Level >= e.Lv)
                    {
                        continue;
                    }

                    if (!avail.ContainsKey(e.Id))
                    {
                        avail.Add(e.Id, e);
                        updDel.Add(e.Id, e.Lv);
                        break;
                    }
                }
                else
                {
                    avail.Add(e.Id, e);
                }
            }

            //foreach (int a in updDel.Keys)
            //    target.removeSkill(a, true, false);
            //updDel.Clear();

            foreach (AcquireSkill sk in avail.Values)
            {
                Skill skill = SkillTable.Instance.Get(sk.Id, sk.Lv);
                if (skill != null)
                {
                    target.AddSkill(skill, false, false);
                }
                else
                {
                    target.SendMessage("no skill #" + sk.Id + "-" + sk.Lv);
                }
            }

            target.ActiveSkillTree = avail;
            target.SendPacket(new AcquireSkillList(AcquireSkillList.SkillType.Usual));

            target.UpdateSkillList();
            target.SendMessage("gor all skills [" + skills.Skills.Count + "][" + avail.Count + "] for lv" + target.Level + ", class @" + target.ActiveClass.ClassId.Id);
        }
    }
}