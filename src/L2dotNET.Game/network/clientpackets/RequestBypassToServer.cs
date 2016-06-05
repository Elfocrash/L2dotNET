using log4net;
using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.npcs;
using L2dotNET.GameService.Model.npcs.ai;
using L2dotNET.GameService.Model.player;
using L2dotNET.GameService.Model.quests;

namespace L2dotNET.GameService.network.clientpackets
{
    class RequestBypassToServer : GameServerNetworkRequest
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RequestBypassToServer));

        public RequestBypassToServer(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private string _alias;

        public override void read()
        {
            _alias = readS();
        }

        private L2Npc getNpc()
        {
            log.Info($"bypass '{_alias}'");
            L2Npc npc = (L2Npc)getClient().CurrentPlayer.CurrentTarget;

            if (npc == null)
            {
                getClient().CurrentPlayer.sendMessage("no npc found");
                getClient().CurrentPlayer.sendActionFailed();
                return null;
            }

            return npc;
        }

        public override void run()
        {
            L2Player player = getClient().CurrentPlayer;

            if (player._p_block_act == 1)
            {
                player.sendActionFailed();
                return;
            }

            L2Npc npc;

            if (_alias.Equals("teleport_request"))
            {
                npc = getNpc();

                if (npc == null)
                {
                    player.sendActionFailed();
                    return;
                }

                npc.onTeleportRequest(player);
            }
            else if (_alias.StartsWith("menu_select?"))
            {
                npc = getNpc();

                _alias = _alias.Replace(" ", "");
                string x1 = _alias.Split('?')[1];
                string[] x2 = x1.Split('&');
                int ask = int.Parse(x2[0].Substring(4));
                int reply;

                try
                {
                    reply = int.Parse(x2[1].Substring(6));
                }
                catch
                {
                    reply = 0;
                }

                npc.onDialog(player, ask, reply);
            }
            else if (_alias.Equals("talk_select"))
            {
                npc = getNpc();
                QuestManager.Instance.TalkSelection(player, npc);
            }
            else if (_alias.StartsWith("quest_accept?"))
            {
                npc = getNpc();
                _alias = _alias.Replace(" ", "");
                string x1 = _alias.Split('?')[1];
                int qid = int.Parse(x1.Split('=')[1]);

                QuestManager.Instance.QuestAccept(player, npc, qid);
            }
            else if (_alias.StartsWith("quest_continue?"))
            {
                npc = getNpc();
                _alias = _alias.Replace(" ", "");
                string x1 = _alias.Split('?')[1];
                int qid = int.Parse(x1.Split('=')[1]);

                QuestManager.Instance.Quest_continue(player, npc, qid);
            }
            else if (_alias.StartsWith("quest_tryaccept?"))
            {
                npc = getNpc();
                _alias = _alias.Replace(" ", "");
                string x1 = _alias.Split('?')[1];
                int qid = int.Parse(x1.Split('=')[1]);

                QuestManager.Instance.Quest_tryaccept(player, npc, qid);
            }
            else if (_alias.Equals("deposit"))
            {
                npc = getNpc();
                npc.showPrivateWarehouse(player);
            }
            else if (_alias.Equals("withdraw"))
            {
                npc = getNpc();
                npc.showPrivateWarehouseBack(player);
            }
            else if (_alias.Equals("deposit_pledge"))
            {
                npc = getNpc();
                npc.showClanWarehouse(player);
            }
            else if (_alias.Equals("withdraw_pledge"))
            {
                npc = getNpc();
                npc.showClanWarehouseBack(player);
            }
            else if (_alias.Equals("learn_skill"))
            {
                npc = getNpc();
                npc.showSkillLearn(player, false);
            }
            else if (_alias.StartsWith("create_pledge?"))
            {
                npc = getNpc();
                //bypass -h create_pledge?pledge_name= $pledge_name
                string x1 = _alias.Split('?')[1];
                string name = x1.Split('=')[1];
                name = name.Substring(1);

                grandmaster_total.createClan(player, name, npc);
            }
            else if (_alias.StartsWith("teleport_next?"))
            {
                npc = getNpc();
                string x1 = _alias.Split('?')[1];
                string[] x2 = x1.Split('&');
                int ask = int.Parse(x2[0].Substring(4));
                int reply = int.Parse(x2[1].Substring(6));

                npc.UseTeleporter(player, ask, reply);
            }
            else if (_alias.StartsWith("petitionlink?"))
            {
                PetitionManager.getInstance().petitionlink(player, _alias.Split('?')[1]);
            }
            else
                log.Warn($"Unknown bypass '{_alias}'");
        }
    }
}