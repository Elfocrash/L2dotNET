using L2dotNET.GameService.network.l2send;
using L2dotNET.Models;
using L2dotNET.Services.Contracts;
using Ninject;
using System.Linq;

namespace L2dotNET.GameService.network.l2recv
{
    class CharacterDelete : GameServerNetworkRequest
    {
        [Inject]
        public IPlayerService playerService { get { return GameServer.Kernel.Get<IPlayerService>(); } }

        public CharacterDelete(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        private int _charSlot;

        public override void read()
        {
            _charSlot = readD();
        }

        public override void run()
        {
            //if (!FloodProtectors.performAction(getClient(), Action.CHARACTER_SELECT))
            //{
            //	getClient().sendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DELETION_FAILED));
            //	return;
            //}		

            PlayerModel playerModel = playerService.GetPlayerModelBySlotId(getClient().AccountName, _charSlot);
            L2Player player = getClient().AccountChars.FirstOrDefault(filter => filter.CharSlot == _charSlot);

            int reason = 0;
            if (player.ClanId != 0)
            {
                if (player.Clan == null)
                    reason = 0;
                else if (player.Clan.LeaderID == player.ObjID)
                    reason = 3;
                else
                    reason = 2;
            }

            if (reason == 0)
            {
                bool success;

                if (Config.Instance.gameplayConfig.DELETE_DAYS == 0)
                {
                    success = playerService.DeleteCharByObjId(player.ObjID);
                    getClient().AccountChars.Remove(getClient().AccountChars.FirstOrDefault(filter => filter.CharSlot == _charSlot));
                    //Reset CharSlot index
                    getClient().AccountChars = getClient().AccountChars.OrderBy(orderby => orderby.CharSlot).ToList();
                    for (int i = 0; i < getClient().AccountChars.Count; i++)
                    {
                        getClient().AccountChars[i].CharSlot = i;
                    }
                }
                else
                    success = playerService.MarkToDeleteChar(player.ObjID);

                if (!success)
                    reason = 1;
            }

            switch (reason)
            {
                case 0:
                    getClient().sendPacket(new CharDeleteOk());
                    break;
                case 1:
                default:
                    getClient().sendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DELETION_FAILED));
                    break;
                case 2:
                    getClient().sendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.YOU_MAY_NOT_DELETE_CLAN_MEMBER));
                    break;
                case 3:
                    getClient().sendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.CLAN_LEADERS_MAY_NOT_BE_DELETED));
                    break;
            }

            CharacterSelectionInfo csl = new CharacterSelectionInfo(getClient().AccountName, getClient().AccountChars, getClient().SessionId);
            getClient().sendPacket(csl);
        }
    }
}
