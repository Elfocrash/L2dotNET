using log4net;
using L2dotNET.managers;
using L2dotNET.model.npcs;
using L2dotNET.model.npcs.ai;
using L2dotNET.model.player;
using L2dotNET.model.quests;
using L2dotNET.Utility;

namespace L2dotNET.Network.clientpackets
{
    class RequestBypassToServer : PacketBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestBypassToServer));

        private readonly GameClient _client;
        private string _alias;

        public RequestBypassToServer(Packet packet, GameClient client)
        {
            _client = client;
            _alias = packet.ReadString();
        }

        private L2Npc GetNpc()
        {
            Log.Info($"bypass '{_alias}'");
            L2Npc npc = (L2Npc)_client.CurrentPlayer.Target;

            if (npc != null)
                return npc;

            _client.CurrentPlayer.SendMessage("no npc found");
            _client.CurrentPlayer.SendActionFailed();
            return null;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            if (player.PBlockAct == 1)
            {
                player.SendActionFailed();
                return;
            }

            L2Npc npc;

            if (_alias.EqualsIgnoreCase("teleport_request"))
            {
                npc = GetNpc();

                if (npc == null)
                {
                    player.SendActionFailed();
                    return;
                }

                npc.OnTeleportRequest(player);
            }
            else
            {
                if (_alias.StartsWithIgnoreCase("menu_select?"))
                {
                    npc = GetNpc();

                    _alias = _alias.Replace(" ", string.Empty);
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

                    npc.OnDialog(player, ask, reply);
                }
                else
                {
                    if (_alias.EqualsIgnoreCase("talk_select"))
                    {
                        npc = GetNpc();
                        QuestManager.Instance.TalkSelection(player, npc);
                    }
                    else
                    {
                        if (_alias.StartsWithIgnoreCase("quest_accept?"))
                        {
                            npc = GetNpc();
                            _alias = _alias.Replace(" ", string.Empty);
                            string x1 = _alias.Split('?')[1];
                            int qid = int.Parse(x1.Split('=')[1]);

                            QuestManager.Instance.QuestAccept(player, npc, qid);
                        }
                        else
                        {
                            if (_alias.StartsWithIgnoreCase("quest_continue?"))
                            {
                                npc = GetNpc();
                                _alias = _alias.Replace(" ", string.Empty);
                                string x1 = _alias.Split('?')[1];
                                int qid = int.Parse(x1.Split('=')[1]);

                                QuestManager.Instance.Quest_continue(player, npc, qid);
                            }
                            else
                            {
                                if (_alias.StartsWithIgnoreCase("quest_tryaccept?"))
                                {
                                    npc = GetNpc();
                                    _alias = _alias.Replace(" ", string.Empty);
                                    string x1 = _alias.Split('?')[1];
                                    int qid = int.Parse(x1.Split('=')[1]);

                                    QuestManager.Instance.Quest_tryaccept(player, npc, qid);
                                }
                                else
                                {
                                    if (_alias.EqualsIgnoreCase("deposit"))
                                    {
                                        npc = GetNpc();
                                        npc.ShowPrivateWarehouse(player);
                                    }
                                    else
                                    {
                                        if (_alias.EqualsIgnoreCase("withdraw"))
                                        {
                                            npc = GetNpc();
                                            npc.ShowPrivateWarehouseBack(player);
                                        }
                                        else
                                        {
                                            if (_alias.EqualsIgnoreCase("deposit_pledge"))
                                            {
                                                npc = GetNpc();
                                                npc.ShowClanWarehouse(player);
                                            }
                                            else
                                            {
                                                if (_alias.EqualsIgnoreCase("withdraw_pledge"))
                                                {
                                                    npc = GetNpc();
                                                    npc.ShowClanWarehouseBack(player);
                                                }
                                                else
                                                {
                                                    if (_alias.EqualsIgnoreCase("learn_skill"))
                                                    {
                                                        npc = GetNpc();
                                                        npc.ShowSkillLearn(player, false);
                                                    }
                                                    else
                                                    {
                                                        if (_alias.StartsWithIgnoreCase("create_pledge?"))
                                                        {
                                                            npc = GetNpc();
                                                            //bypass -h create_pledge?pledge_name= $pledge_name
                                                            string x1 = _alias.Split('?')[1];
                                                            string name = x1.Split('=')[1];
                                                            name = name.Substring(1);

                                                            GrandmasterTotal.CreateClan(player, name, npc);
                                                        }
                                                        else
                                                        {
                                                            if (_alias.StartsWithIgnoreCase("teleport_next?"))
                                                            {
                                                                npc = GetNpc();
                                                                string x1 = _alias.Split('?')[1];
                                                                string[] x2 = x1.Split('&');
                                                                int ask = int.Parse(x2[0].Substring(4));
                                                                int reply = int.Parse(x2[1].Substring(6));

                                                                npc.UseTeleporter(player, ask, reply);
                                                            }
                                                            else
                                                            {
                                                                if (_alias.StartsWithIgnoreCase("petitionlink?"))
                                                                    PetitionManager.GetInstance().Petitionlink(player, _alias.Split('?')[1]);
                                                                else
                                                                    Log.Warn($"Unknown bypass '{_alias}'");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}