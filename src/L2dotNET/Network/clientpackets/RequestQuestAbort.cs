using System.Linq;
using L2dotNET.model.player;
using L2dotNET.model.quests;

namespace L2dotNET.Network.clientpackets
{
    class RequestQuestAbort : PacketBase
    {
        private readonly GameClient _client;
        private readonly int _questId;

        public RequestQuestAbort(Packet packet, GameClient client)
        {
            _client = client;
            _questId = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            foreach (QuestInfo qi in player.Quests.Where(qi => qi.Id == _questId))
            {
                if (qi.Completed)
                {
                    player.SendActionFailed();
                    return;
                }

                //foreach (int id in qi._template.actItems)
                //    player.Inventory.Dest(id, true, false);

                player.SendMessage($"Quest {qi.Template.QuestName} aborted.");
                player.StopQuest(qi, true);
                return;
            }

            player.SendActionFailed();
        }
    }
}