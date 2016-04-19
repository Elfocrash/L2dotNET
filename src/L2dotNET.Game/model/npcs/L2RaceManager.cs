using L2dotNET.Game.model.events;

namespace L2dotNET.Game.model.npcs
{
    public class L2RaceManager : L2Citizen
    {
        private MonsterRace monsterRace;
        public L2RaceManager()
        {
            monsterRace = MonsterRace.getInstance();
        }

        public override void NotifyAction(L2Player player)
        {
            player.ShowHtm("mr_keeper.htm", this);
        }

        public override void onDialog(L2Player player, int ask, int reply)
        {
            switch (ask)
            {
                case 255:
                    {
                        switch (reply)
                        {
                            case 1://Exit the monster race track.
                                break;
                        }
                    }
                    break;
            }
        }
    }
}
