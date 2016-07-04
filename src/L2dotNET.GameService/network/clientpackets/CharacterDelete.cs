using System.Linq;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.Services.Contracts;
using Ninject;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class CharacterDelete : GameServerNetworkRequest
    {
        [Inject]
        public IPlayerService playerService
        {
            get { return GameServer.Kernel.Get<IPlayerService>(); }
        }

        public CharacterDelete(GameClient client, byte[] data)
        {
            makeme(client, data);
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

            L2Player player = getClient().AccountChars.FirstOrDefault(filter => filter.CharSlot == _charSlot);

            if (player == null)
            {
                getClient().SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DELETION_FAILED));
                return;
            }

            if ((player.ClanId != 0) && (player.Clan != null))
            {
                if (player.Clan.LeaderID == player.ObjId)
                {
                    getClient().SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.CLAN_LEADERS_MAY_NOT_BE_DELETED));
                    return;
                }

                getClient().SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.YOU_MAY_NOT_DELETE_CLAN_MEMBER));
                return;
            }

            if (Config.Config.Instance.GameplayConfig.Server.Client.DeleteCharAfterDays == 0)
            {
                if (!playerService.DeleteCharByObjId(player.ObjId))
                {
                    getClient().SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DELETION_FAILED));
                    return;
                }

                getClient().RemoveAccountCharAndResetSlotIndex(_charSlot);
            }
            else
            {
                if (!playerService.MarkToDeleteChar(player.ObjId))
                {
                    getClient().SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DELETION_FAILED));
                    return;
                }
            }

            getClient().SendPacket(new CharDeleteOk());
            CharacterSelectionInfo csl = new CharacterSelectionInfo(getClient().AccountName, getClient().AccountChars, getClient().SessionId);
            getClient().SendPacket(csl);
        }
    }
}