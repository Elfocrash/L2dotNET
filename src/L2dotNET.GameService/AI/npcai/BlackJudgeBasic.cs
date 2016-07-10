using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.AI.NpcAI
{
    class BlackJudgeBasic : Citizen
    {
        public int SPenaltyoff = 458752001;
        public int CostPenaltyoffS = 0;
        public int CostPenaltyoffA = 0;
        public int CostPenaltyoffB = 0;
        public int CostPenaltyoffC = 0;
        public int CostPenaltyoffD = 0;
        public int CostPenaltyoff0 = 0;

        public override void TalkedReply(L2Player talker, int ask, int reply)
        {
            switch (ask)
            {
                case -505:
                    switch (reply)
                    {
                        case 1:
                            GetOffPenalty(talker, 76, 99, CostPenaltyoffS);
                            break;
                        case 2:
                            GetOffPenalty(talker, 61, 76, CostPenaltyoffA);
                            break;
                        case 3:
                            GetOffPenalty(talker, 52, 61, CostPenaltyoffB);
                            break;
                        case 4:
                            GetOffPenalty(talker, 40, 52, CostPenaltyoffC);
                            break;
                        case 5:
                            GetOffPenalty(talker, 20, 40, CostPenaltyoffD);
                            break;
                        case 6:
                            GetOffPenalty(talker, 1, 20, CostPenaltyoff0);
                            break;
                    }

                    break;
                case -506:
                    if (talker.Level >= 76)
                    {
                        talker.ShowHtm("black_judge007.htm", Myself);
                    }
                    else if ((talker.Level >= 61) && (talker.Level < 76))
                    {
                        talker.ShowHtm("black_judge006.htm", Myself);
                    }
                    else if ((talker.Level >= 52) && (talker.Level < 61))
                    {
                        talker.ShowHtm("black_judge005.htm", Myself);
                    }
                    else if ((talker.Level >= 40) && (talker.Level < 52))
                    {
                        talker.ShowHtm("black_judge004.htm", Myself);
                    }
                    else if ((talker.Level >= 20) && (talker.Level < 40))
                    {
                        talker.ShowHtm("black_judge003.htm", Myself);
                    }
                    else if ((talker.Level >= 1) && (talker.Level < 20))
                    {
                        talker.ShowHtm("black_judge002.htm", Myself);
                    }
                    break;
                default:
                    base.TalkedReply(talker, ask, reply);
                    break;
            }
        }

        private void GetOffPenalty(L2Player talker, byte minLv, byte maxLv, int cost)
        {
            if ((talker.Level < minLv) || (talker.Level > maxLv))
            {
                return;
            }

            if (talker.DeathPenaltyLevel > 0)
            {
                if (talker.ReduceAdena(cost))
                {
                    talker.DestroyItemById(Adena, cost);
                    Myself.CastBuffForQuestReward(talker, SPenaltyoff);
                }
                else
                {
                    talker.ShowHtm("black_judge008.htm", Myself);
                }
            }
            else
            {
                talker.ShowHtm("black_judge009.htm", Myself);
            }
        }
    }
}