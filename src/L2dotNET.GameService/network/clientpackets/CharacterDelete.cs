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
        public IPlayerService PlayerService => GameServer.Kernel.Get<IPlayerService>();

        public CharacterDelete(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private int _charSlot;

        public override void Read()
        {
            _charSlot = ReadD();
        }

        public override void Run()
        {
            //if (!FloodProtectors.performAction(getClient(), Action.CHARACTER_SELECT))
            //{
            //	getClient().sendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DELETION_FAILED));
            //	return;
            //}

            L2Player player = GetClient().AccountChars.FirstOrDefault(filter => filter.CharSlot == _charSlot);

            if (player == null)
            {
                GetClient().SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DeletionFailed));
                return;
            }

            if ((player.ClanId != 0) && (player.Clan != null))
            {
                if (player.Clan.LeaderId == player.ObjId)
                {
                    GetClient().SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.ClanLeadersMayNotBeDeleted));
                    return;
                }

                GetClient().SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.YouMayNotDeleteClanMember));
                return;
            }

            if (Config.Config.Instance.GameplayConfig.Server.Client.DeleteCharAfterDays == 0)
            {
                if (!PlayerService.DeleteCharByObjId(player.ObjId))
                {
                    GetClient().SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DeletionFailed));
                    return;
                }

                GetClient().RemoveAccountCharAndResetSlotIndex(_charSlot);
            }
            else
            {
                if (!PlayerService.MarkToDeleteChar(player.ObjId))
                {
                    GetClient().SendPacket(new CharDeleteFail(CharDeleteFail.CharDeleteFailReason.DeletionFailed));
                    return;
                }
            }

            GetClient().SendPacket(new CharDeleteOk());
            CharacterSelectionInfo csl = new CharacterSelectionInfo(GetClient().AccountName, GetClient().AccountChars, GetClient().SessionId);
            GetClient().SendPacket(csl);
        }
    }
}