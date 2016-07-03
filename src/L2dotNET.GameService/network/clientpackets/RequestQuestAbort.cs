using System.Linq;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Quests;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestQuestAbort : GameServerNetworkRequest
    {
        public RequestQuestAbort(GameClient client, byte[] data)
        {
            makeme(client, data);
        }

        private int _questId;

        public override void read()
        {
            _questId = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            foreach (QuestInfo qi in player._quests.Where(qi => qi.id == _questId))
            {
                if (qi.completed)
                {
                    player.sendActionFailed();
                    return;
                }

                //foreach (int id in qi._template.actItems)
                //    player.Inventory.Dest(id, true, false);

                player.sendMessage("Quest " + qi._template.questName + " aborted.");
                player.stopQuest(qi, true);
                return;
            }

            player.sendActionFailed();
        }
    }
}