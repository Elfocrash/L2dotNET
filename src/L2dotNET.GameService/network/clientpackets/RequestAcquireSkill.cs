using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Player.General;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestAcquireSkill : GameServerNetworkRequest
    {
        public RequestAcquireSkill(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private int _id;
        private int _level;
        private int _skillType;

        public override void Read()
        {
            _id = ReadD();
            _level = ReadD();
            _skillType = ReadD();
        }

        public override void Run()
        {
            L2Player player = GetClient().CurrentPlayer;

            SortedList<int, AcquireSkill> seq = player.ActiveSkillTree;

            if ((seq == null) || !seq.ContainsKey(_id))
            {
                player.SendActionFailed();
                return;
            }

            AcquireSkill e = seq[_id];

            if (e.Lv != _level)
            {
                player.SendActionFailed();
                return;
            }

            if (e.LvUpSp > player.Sp)
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.NotEnoughSpToLearnSkill);
                player.SendActionFailed();
                return;
            }

            //if (e.itemid > 0)
            //    if (!player.hasItem(e.itemid, e.itemcount))
            //    {
            //        player.sendSystemMessage(SystemMessage.SystemMessageId.ITEM_MISSING_TO_LEARN_SKILL);
            //        player.sendActionFailed();
            //        return;
            //    }

            if (e.LvUpSp > 0)
            {
                player.Sp -= e.LvUpSp;
                StatusUpdate su = new StatusUpdate(player.ObjId);
                su.Add(StatusUpdate.Sp, player.Sp);
                player.SendPacket(su);
            }

            player.DestroyItemById(e.ItemId, e.ItemCount);

            Skill skill = SkillTable.Instance.Get(e.Id, e.Lv);
            if (skill != null)
            {
                player.AddSkill(skill, true, true);
            }
            else
            {
                player.SendMessage("failed to learn null skill");
                player.SendActionFailed();
                return;
            }

            if (_level > 1)
            {
                bool upd = false;
                lock (player.Shortcuts)
                {
                    foreach (L2Shortcut sc in player.Shortcuts.Where(sc => (sc.Type == L2Shortcut.TypeSkill) && (sc.Id == _id)))
                    {
                        sc.Level = _level;
                        upd = true;
                    }
                }

                if (upd)
                {
                    player.SendPacket(new ShortCutInit(player));
                }
            }

            player.ActiveSkillTree.Remove(_id);
            player.FolkNpc.ShowAvailRegularSkills(player, true);
        }
    }
}