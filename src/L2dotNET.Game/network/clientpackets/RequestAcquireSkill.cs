using System.Collections.Generic;
using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.Model.skills2;
using L2dotNET.GameService.network.serverpackets;

namespace L2dotNET.GameService.network.clientpackets
{
    class RequestAcquireSkill : GameServerNetworkRequest
    {
        public RequestAcquireSkill(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int _id;
        private int _level;
        private int _skillType;

        public override void read()
        {
            _id = readD();
            _level = readD();
            _skillType = readD();
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            SortedList<int, TAcquireSkill> seq = player.ActiveSkillTree;

            if (seq == null || !seq.ContainsKey(_id))
            {
                player.sendActionFailed();
                return;
            }

            TAcquireSkill e = seq[_id];

            if (e.lv != _level)
            {
                player.sendActionFailed();
                return;
            }

            if (e.lv_up_sp > player.SP)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.NOT_ENOUGH_SP_TO_LEARN_SKILL);
                player.sendActionFailed();
                return;
            }

            if (e.itemid > 0)
            {
                if (!player.hasItem(e.itemid, e.itemcount))
                {
                    player.sendSystemMessage(SystemMessage.SystemMessageId.ITEM_MISSING_TO_LEARN_SKILL);
                    player.sendActionFailed();
                    return;
                }
            }

            if (e.lv_up_sp > 0)
            {
                player.SP -= e.lv_up_sp;
                StatusUpdate su = new StatusUpdate(player.ObjID);
                su.add(StatusUpdate.SP, player.SP);
                player.sendPacket(su);
            }

            player.Inventory.destroyItem(e.itemid, e.itemcount, true, true);

            TSkill skill = TSkillTable.Instance.Get(e.id, e.lv);
            if (skill != null)
                player.addSkill(skill, true, true);
            else
            {
                player.sendMessage("failed to learn null skill");
                player.sendActionFailed();
                return;
            }

            if (_level > 1)
            {
                bool upd = false;
                lock (player._shortcuts)
                {
                    foreach (L2Shortcut sc in player._shortcuts)
                    {
                        if (sc.Type == L2Shortcut.TYPE_SKILL && sc.Id == _id)
                        {
                            sc.Level = _level;
                            upd = true;
                        }
                    }
                }

                if (upd)
                    player.sendPacket(new ShortCutInit(player));
            }

            player.ActiveSkillTree.Remove(_id);
            player.FolkNpc.showAvailRegularSkills(player, true);
        }
    }
}