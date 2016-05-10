﻿using L2dotNET.Game.managers;
using L2dotNET.Game.model.npcs;
using L2dotNET.Game.model.npcs.ai;
using L2dotNET.Game.model.quests;
using L2dotNET.Game.model.events;
using System;
using log4net;

namespace L2dotNET.Game.network.l2recv
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

        private L2Citizen getNpc()
        {
            Console.WriteLine("bypass '" + _alias+"'");
            L2Citizen npc = (L2Citizen)getClient().CurrentPlayer.CurrentTarget;

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

            L2Citizen npc;

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
                QuestManager.Instance.talkSelection(player, npc);
            }
            else if (_alias.StartsWith("quest_accept?"))
            {
                npc = getNpc();
                _alias = _alias.Replace(" ", "");
                string x1 = _alias.Split('?')[1];
                int qid = int.Parse(x1.Split('=')[1]);

                QuestManager.Instance.questAccept(player, npc, qid);
            }
            else if (_alias.StartsWith("quest_continue?"))
            {
                npc = getNpc();
                _alias = _alias.Replace(" ", "");
                string x1 = _alias.Split('?')[1];
                int qid = int.Parse(x1.Split('=')[1]);

                QuestManager.Instance.quest_continue(player, npc, qid);
            }
            else if (_alias.StartsWith("quest_tryaccept?"))
            {
                npc = getNpc();
                _alias = _alias.Replace(" ", "");
                string x1 = _alias.Split('?')[1];
                int qid = int.Parse(x1.Split('=')[1]);

                QuestManager.Instance.quest_tryaccept(player, npc, qid);
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
            else if (_alias.StartsWith("_mr"))
            {
                npc = getNpc();
                MonsterRace.Instance.OnBypass(player, npc, _alias);
            }
            else
                log.Warn($"Unknown bypass '{ _alias }'");
        }
    }
}
