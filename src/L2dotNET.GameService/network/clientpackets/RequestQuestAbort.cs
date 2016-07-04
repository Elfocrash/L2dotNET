using System.Linq;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Quests;

namespace L2dotNET.GameService.Network.Clientpackets
{
    class RequestQuestAbort : GameServerNetworkRequest
    {
        public RequestQuestAbort(GameClient client, byte[] data)
        {
            Makeme(client, data);
        }

        private int _questId;

        public override void Read()
        {
            _questId = ReadD();
        }

        public override void Run()
        {
            L2Player player = Client.CurrentPlayer;

            foreach (QuestInfo qi in player.Quests.Where(qi => qi.Id == _questId))
            {
                if (qi.Completed)
                {
                    player.SendActionFailed();
                    return;
                }

                //foreach (int id in qi._template.actItems)
                //    player.Inventory.Dest(id, true, false);

                player.SendMessage("Quest " + qi.Template.QuestName + " aborted.");
                player.StopQuest(qi, true);
                return;
            }

            player.SendActionFailed();
        }
    }
}