using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.AI.NpcAI
{
    class black_judge_basic : citizen
    {
        public int s_penaltyoff = 458752001;
        public int cost_penaltyoffS = 0;
        public int cost_penaltyoffA = 0;
        public int cost_penaltyoffB = 0;
        public int cost_penaltyoffC = 0;
        public int cost_penaltyoffD = 0;
        public int cost_penaltyoff0 = 0;

        public override void TalkedReply(L2Player talker, int ask, int reply)
        {
            switch (ask)
            {
                case -505:
                    switch (reply)
                    {
                        case 1:
                            GetOffPenalty(talker, 76, 99, cost_penaltyoffS);
                            break;
                        case 2:
                            GetOffPenalty(talker, 61, 76, cost_penaltyoffA);
                            break;
                        case 3:
                            GetOffPenalty(talker, 52, 61, cost_penaltyoffB);
                            break;
                        case 4:
                            GetOffPenalty(talker, 40, 52, cost_penaltyoffC);
                            break;
                        case 5:
                            GetOffPenalty(talker, 20, 40, cost_penaltyoffD);
                            break;
                        case 6:
                            GetOffPenalty(talker, 1, 20, cost_penaltyoff0);
                            break;
                    }

                    break;
                case -506:
                    if (talker.Level >= 76)
                        talker.ShowHtm("black_judge007.htm", myself);
                    else if ((talker.Level >= 61) && (talker.Level < 76))
                        talker.ShowHtm("black_judge006.htm", myself);
                    else if ((talker.Level >= 52) && (talker.Level < 61))
                        talker.ShowHtm("black_judge005.htm", myself);
                    else if ((talker.Level >= 40) && (talker.Level < 52))
                        talker.ShowHtm("black_judge004.htm", myself);
                    else if ((talker.Level >= 20) && (talker.Level < 40))
                        talker.ShowHtm("black_judge003.htm", myself);
                    else if ((talker.Level >= 1) && (talker.Level < 20))
                        talker.ShowHtm("black_judge002.htm", myself);
                    break;
                default:
                    base.TalkedReply(talker, ask, reply);
                    break;
            }
        }

        private void GetOffPenalty(L2Player talker, byte minLv, byte maxLv, int cost)
        {
            if ((talker.Level >= minLv) && (talker.Level <= maxLv))
                if (talker.DeathPenaltyLevel > 0)
                    if (talker.hasItem(adena, cost))
                    {
                        talker.takeItem(adena, cost);
                        myself.CastBuffForQuestReward(talker, s_penaltyoff);
                    }
                    else
                        talker.ShowHtm("black_judge008.htm", myself);
                else
                    talker.ShowHtm("black_judge009.htm", myself);
        }
    }
}