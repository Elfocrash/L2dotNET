using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using L2dotNET.Models.Items;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Templates;
using L2dotNET.Tools;
using L2dotNET.Tables;
using NLog;

namespace L2dotNET.Models.Npcs
{
    public class L2Warehouse : L2Npc
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public new NpcTemplate Template;

        public L2Warehouse(SpawnTable spawnTable, int objectId, NpcTemplate template, L2Spawn spawn) : base(spawnTable, objectId, template, spawn)
        {
            Template = template;
            Name = template.Name;
            InitializeCharacterStatus();
            this.spawn = spawn;
        }

        public override async Task NotifyActionAsync(L2Player player)
        {
            double dis = Calcs.CalculateDistance(player, this, true);
            await TryMoveToAsync(X, Y, Z);
        }

        public override async Task OnActionAsync(L2Player player)
        {
            if (player.Target != this)
                player.SetTargetAsync(this);
            else
            {
                player.MoveToAsync(X, Y, Z);
                await player.SendPacketAsync(new MoveToPawn(player, this, 150));

                player.ShowHtm($"warehouse/{NpcId}.htm", this);
            }
        }

        public override void OnDialog(L2Player player, int ask, int reply)
        {
            player.FolkNpc = this;

            //if (ask > 0 && ask < 1000)
            //{
            //    QuestManager.Instance.OnQuestTalk(player, this, ask, reply);
            //    return;
            //}

            //AITemplate t = AIManager.Instance.CheckDialogResult(Template.NpcId);
            //if (t != null)
            //{
            //    t.onDialog(player, ask, reply, this);
            //    return;
            //}

            //switch (ask)
            //{
            //    case -1:
            //        switch (reply)
            //        {
            //            case 8:
            //                player.sendPacket(new ExBuySellList_Buy(player.getAdena()));
            //                player.sendPacket(new ExBuySellList_Sell(player));
            //                break;

            //            default:
            //                NpcData.Instance.Buylist(player, this, (short)reply);
            //                break;
            //        }
            //        break;
            //    case -3:
            //        grandmaster_total.onReply(player, reply, this);
            //        break;
            //    case -19: //нобл запрос
            //        switch (reply)
            //        {
            //            case 0:
            //            case 1:
            //                player.ShowHtm(player.Noblesse == 1 ? "fornobless.htm" : "fornonobless.htm", this);
            //                break;
            //        }
            //        break;
            //    case -20: //нобл запрос
            //        switch (reply)
            //        {
            //            case 2:
            //                NpcData.Instance.RequestTeleportList(this, player, 2);
            //                break;
            //        }
            //        break;
            //    case -21: //нобл запрос
            //        switch (reply)
            //        {
            //            case 2:
            //                NpcData.Instance.RequestTeleportList(this, player, 3);
            //                break;
            //        }
            //        break;
            //    case -22: //нобл запрос
            //        switch (reply)
            //        {
            //            case 2:
            //                NpcData.Instance.RequestTeleportList(this, player, 1);
            //                break;
            //        }
            //        break;
            //    case -303:
            //        MultiSell.Instance.ShowList(player, this, reply);
            //        break;
            //    case -305:
            //        switch (reply)
            //        {
            //            case 1:
            //                //  NpcData.getInstance().multisell(player, this, reply); //TEST
            //                break;
            //        }
            //        break;
            //    case -1000:
            //    {
            //        switch (reply)
            //        {
            //            case 1:
            //                //See the lord and get the tax rate information
            //                break;
            //        }
            //    }
            //        break;
            //}
        }

        public override async Task OnForcedAttackAsync(L2Player player)
        {
            bool newtarget = false;
            if (player.Target == null)
            {
                player.Target = this;
                newtarget = true;
            }
            else
            {
                if (player.Target.CharacterId != CharacterId)
                {
                    player.Target = this;
                    newtarget = true;
                }
            }

            if (newtarget)
                await player.SendPacketAsync(new MyTargetSelected(CharacterId, 0));
            else
                await player.SendActionFailedAsync();
        }

        public async Task ShowPrivateWarehouse(L2Player player)
        {
            List<L2Item> items = player.GetAllItems().Where(item => item.IsEquipped != 1).ToList();

            await player.SendPacketAsync(new WareHouseDepositList(player, items, WareHouseDepositList.WhPrivate));
            player.FolkNpc = this;
        }

        public void ShowClanWarehouse(L2Player player)
        {
            //if (player.Clan == null)
            //{
            //    player.SendSystemMessage(SystemMessage.SystemMessageId.YouDoNotHaveTheRightToUseClanWarehouse);
            //    player.SendActionFailed();
            //    return;
            //}

            //if (player.Clan.Level == 0)
            //{
            //    player.SendSystemMessage(SystemMessage.SystemMessageId.OnlyLevel1ClanOrHigherCanUseWarehouse);
            //    player.SendActionFailed();
            //    return;
            //}

            List<L2Item> items = player.GetAllItems().Where(item => item.IsEquipped != 1).ToList();

            player.SendPacketAsync(new WareHouseDepositList(player, items, WareHouseDepositList.WhClan));
            player.FolkNpc = this;
        }

        public void ShowPrivateWarehouseBack(L2Player player)
        {
            //if (player._warehouse == null)
            //{
            //    player.sendSystemMessage(SystemMessage.SystemMessageId.NO_ITEM_DEPOSITED_IN_WH);
            //    player.sendActionFailed();
            //    return;
            //}

            //List<L2Item> items = player.getAllWarehouseItems().Cast<L2Item>().ToList();

            //if (items.Count == 0) // на случай если вх был создан и убраны вещи до времени выхода с сервера
            //{
            //    player.sendSystemMessage(SystemMessage.SystemMessageId.NO_ITEM_DEPOSITED_IN_WH);
            //    player.sendActionFailed();
            //    return;
            //}

            //player.sendPacket(new WareHouseWithdrawalList(player, items, WareHouseWithdrawalList.WH_PRIVATE));
            //player.FolkNpc = this;
        }

        public void ShowClanWarehouseBack(L2Player player)
        {
            //if (player.Clan == null)
            //{
            //    player.SendSystemMessage(SystemMessage.SystemMessageId.YouDoNotHaveTheRightToUseClanWarehouse);
            //    player.SendActionFailed();
            //}
            //else
            //{
            //    if (player.Clan.Level != 0)
            //        return;

            //    player.SendSystemMessage(SystemMessage.SystemMessageId.OnlyLevel1ClanOrHigherCanUseWarehouse);
            //    player.SendActionFailed();
            //}
        }
    }
}