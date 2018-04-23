using System;
using log4net;
using L2dotNET.Managers;
using L2dotNET.Models.Npcs;
using L2dotNET.Models.Player;
using L2dotNET.Utility;

namespace L2dotNET.Network.clientpackets
{
    class RequestBypassToServer : PacketBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestBypassToServer));

        private readonly GameClient _client;
        private string _alias;

        public RequestBypassToServer(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
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

        private L2Warehouse GetWarehouseNPC()
        {
            Log.Info($"bypass '{_alias}'");
            L2Warehouse npc = (L2Warehouse)_client.CurrentPlayer.Target;

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
            L2Warehouse warehouseNPC;

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
                        //QuestManager.Instance.TalkSelection(player, npc);
                    }
                    else
                    {
                        if (_alias.StartsWithIgnoreCase("quest_accept?"))
                        {
                            npc = GetNpc();
                            _alias = _alias.Replace(" ", string.Empty);
                            string x1 = _alias.Split('?')[1];
                            int qid = int.Parse(x1.Split('=')[1]);

                            //QuestManager.Instance.QuestAccept(player, npc, qid);
                        }
                        else
                        {
                            if (_alias.StartsWithIgnoreCase("quest_continue?"))
                            {
                                npc = GetNpc();
                                _alias = _alias.Replace(" ", string.Empty);
                                string x1 = _alias.Split('?')[1];
                                int qid = int.Parse(x1.Split('=')[1]);

                                //QuestManager.Instance.Quest_continue(player, npc, qid);
                            }
                            else
                            {
                                if (_alias.StartsWithIgnoreCase("quest_tryaccept?"))
                                {
                                    npc = GetNpc();
                                    _alias = _alias.Replace(" ", string.Empty);
                                    string x1 = _alias.Split('?')[1];
                                    int qid = int.Parse(x1.Split('=')[1]);

                                   // QuestManager.Instance.Quest_tryaccept(player, npc, qid);
                                }
                                else
                                {
                                    if (_alias.Contains("DepositP"))
                                    {
                                        warehouseNPC = GetWarehouseNPC();
                                        warehouseNPC.ShowPrivateWarehouse(player);
                                        return;
                                    }
                                    else
                                    {
                                        if (_alias.Contains("WithdrawP"))
                                        {
                                            warehouseNPC = GetWarehouseNPC();
                                            warehouseNPC.ShowPrivateWarehouseBack(player);
                                            return;
                                        }
                                        else
                                        {
                                            if (_alias.Contains("DepositC"))
                                            {
                                                warehouseNPC = GetWarehouseNPC();
                                                warehouseNPC.ShowClanWarehouse(player);
                                                return;
                                            }
                                            else
                                            {
                                                if (_alias.Contains("WithdrawC"))
                                                {
                                                    warehouseNPC = GetWarehouseNPC();
                                                    warehouseNPC.ShowClanWarehouseBack(player);
                                                    return;
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

                                                           // GrandmasterTotal.CreateClan(player, name, npc);
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
                                                                if (_alias.StartsWithIgnoreCase("npc"))
                                                                {
                                                                    npc = GetNpc();
                                                                    Log.Warn($"Bypass Accepted '{_alias}'");
                                                                }
                                                                else {
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
}