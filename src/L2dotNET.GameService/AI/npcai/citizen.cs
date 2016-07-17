using System;
using System.Timers;
using L2dotNET.GameService.Managers;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Structures;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.Tables.Multisell;

namespace L2dotNET.GameService.AI.NpcAI
{
    class Citizen : Template.Ai
    {
        public string FnHi = "lector001.htm";
        public string FnFeudInfo = "gludio_feud_manager001.htm";
        public string FnNoFeudInfo = "farm_messenger002.htm";
        public string FnBracketL = "[";
        public string FnBracketR = "]";
        public string FnFlagMan = "flagman.htm";
        public int MoveAroundSocial = 0;
        public int MoveAroundSocial1 = 0;
        public string AiType = "pet_around_pet_manager";
        public int HavePet = 0;
        public int Silhouette = 20130;
        public int FriendShip1 = 0;
        public int FriendShip2 = 0;
        public int FriendShip3 = 0;
        public int FriendShip4 = 0;
        public int FriendShip5 = 0;
        public string FnNoFriend = "citizen_html";
        public int NoFnHi = 0;

        public Timer SocialMoveTimer;

        public override void Talked(L2Player talker)
        {
            if (NoFnHi == 1)
                return;

            //if (FriendShip1 == 0)
            //    talker.ShowHtm(GetDialog("fnHi"), myself);
            //else if (talker.hasSomeOfThisItems())
            //    talker.ShowHtm(GetDialog("fnFlagMan"), myself);
            //else if (talker.hasSomeOfThisItems(FriendShip1, FriendShip2, FriendShip3, FriendShip4, FriendShip5))
            //    talker.ShowHtm(GetDialog("fnHi"), myself);
            //else
            talker.ShowHtm(GetDialog("fnNoFriend"), Myself);
        }

        public override void Created()
        {
            if (HavePet == 1)
                Myself.CreateOnePrivateEx(Silhouette, AiType, Myself.X + 10, Myself.Y + 10, Myself.Z);

            if ((MoveAroundSocial <= 0) && (MoveAroundSocial1 <= 0))
                return;

            if (SocialMoveTimer == null)
            {
                SocialMoveTimer = new Timer
                                  {
                                      Interval = 10000
                                  };
                SocialMoveTimer.Elapsed += SocialTask;
            }

            SocialMoveTimer.Enabled = true;
        }

        private void SocialTask(object sender, ElapsedEventArgs e)
        {
            if (!(Myself.CurHp > (Myself.MaxHp * 0.400000)) || Myself.Dead)
                return;

            if ((MoveAroundSocial > 0) && (new Random().Next(100) < 40))
            {
                //myself::AddEffectActionDesire( myself.sm, 3, MoveAroundSocial * 1000 / 30, 50 );
            }
            else
            {
                if ((MoveAroundSocial1 > 0) && (new Random().Next(100) < 40))
                {
                    //myself::AddEffectActionDesire( myself.sm, 2, MoveAroundSocial1 * 1000 / 30, 50 );
                }
            }
        }

        public override void TalkedReply(L2Player talker, int ask, int reply)
        {
            switch (ask)
            {
                case -1000:
                    switch (reply)
                    {
                        case 0:
                            talker.ShowHtm(GetDialog("fnHi"), Myself);
                            break;
                        case 1:
                            NpcHtmlMessage htm = new NpcHtmlMessage(talker, GetDialog(Myself.ResidenceId > 0 ? "fnFeudInfo" : "fnNoFeudInfo"), Myself.ObjId);

                            if (Myself.ResidenceId > 0)
                            {
                                Castle castle = CastleManager.GetInstance().Get(Myself.ResidenceId);
                                htm.Replace("<?my_pledge_name?>", castle.OwningClanName);
                                htm.Replace("<?my_owner_name?>", castle.OwningPlayerName);
                                htm.Replace("<?current_tax_rate?>", (int)castle.Tax);
                            }

                            htm.Replace("<?kingdom_name?>", FString.GetInstance().Get(Myself.ResidenceId < 7 ? 1001000 : 1001100));
                            htm.Replace("<?feud_name?>", FString.GetInstance().Get(Myself.ResidenceId + 1001000));

                            talker.SendPacket(htm);
                            break;
                    }

                    break;
                case -303:
                    switch (reply)
                    {
                        case 579:
                            if ((talker.Level > 40) && (talker.Level < 46))
                                MultiSell.Instance.ShowList(talker, Myself, reply);
                            break;
                        case 580:
                            if ((talker.Level >= 46) && (talker.Level < 52))
                                MultiSell.Instance.ShowList(talker, Myself, reply);
                            break;
                        case 581:
                            if (talker.Level >= 52)
                                MultiSell.Instance.ShowList(talker, Myself, reply);
                            break;
                        default:
                            MultiSell.Instance.ShowList(talker, Myself, reply);
                            break;
                    }

                    break;
                case -503:
                    switch (reply)
                    {
                        case 100:
                            ShowVariationMakeWindow(talker);
                            break;
                        case 200:
                            ShowVariationCancelWindow(talker);
                            break;
                    }

                    break;
                case -601:
                    //if (reply == 0)
                    //    if (!talker.hasAllOfThisItems(8957, 8958, 8959))
                    //        talker.ShowHtm("welcomeback003.htm", myself);
                    //    else
                    //        talker.ShowHtm("welcomeback004.htm", myself);
                    //else
                    //    switch (reply)
                    //    {
                    //        case 0:
                    //            if (!talker.hasAllOfThisItems(8957, 8958, 8959))
                    //                talker.ShowHtm("welcome_lin2_cat002.htm", myself);
                    //            else
                    //                talker.ShowHtm("welcome_lin2_cat004.htm", myself);
                    //            break;
                    //        case 2:
                    //            if (talker.Level < 20)
                    //                MultiSell.Instance.ShowList(talker, myself, 583);
                    //            else if ((talker.Level >= 20) && (talker.Level < 40))
                    //                MultiSell.Instance.ShowList(talker, myself, 584);
                    //            else if ((talker.Level >= 40) && (talker.Level < 52))
                    //                MultiSell.Instance.ShowList(talker, myself, 585);
                    //            else if ((talker.Level >= 52) && (talker.Level < 61))
                    //                MultiSell.Instance.ShowList(talker, myself, 586);
                    //            else if ((talker.Level >= 61) && (talker.Level < 76))
                    //                MultiSell.Instance.ShowList(talker, myself, 587);
                    //            else if (talker.Level >= 76)
                    //                MultiSell.Instance.ShowList(talker, myself, 588);
                    //            break;
                    //        case 3:
                    //            if (talker.Level < 20)
                    //                MultiSell.Instance.ShowList(talker, myself, 589);
                    //            else if ((talker.Level >= 20) && (talker.Level < 40))
                    //                MultiSell.Instance.ShowList(talker, myself, 590);
                    //            else if ((talker.Level >= 40) && (talker.Level < 52))
                    //                MultiSell.Instance.ShowList(talker, myself, 591);
                    //            else if ((talker.Level >= 52) && (talker.Level < 61))
                    //                MultiSell.Instance.ShowList(talker, myself, 592);
                    //            else if ((talker.Level >= 61) && (talker.Level < 76))
                    //                MultiSell.Instance.ShowList(talker, myself, 593);
                    //            else if (talker.Level >= 76)
                    //                MultiSell.Instance.ShowList(talker, myself, 594);
                    //            break;
                    //        case 4:
                    //            if (talker.Level < 20)
                    //                MultiSell.Instance.ShowList(talker, myself, 595);
                    //            else if ((talker.Level >= 20) && (talker.Level < 40))
                    //                MultiSell.Instance.ShowList(talker, myself, 596);
                    //            else if ((talker.Level >= 40) && (talker.Level < 52))
                    //                MultiSell.Instance.ShowList(talker, myself, 597);
                    //            else if ((talker.Level >= 52) && (talker.Level < 61))
                    //                MultiSell.Instance.ShowList(talker, myself, 598);
                    //            else if ((talker.Level >= 61) && (talker.Level < 76))
                    //                MultiSell.Instance.ShowList(talker, myself, 601);
                    //            else if (talker.Level >= 76)
                    //                MultiSell.Instance.ShowList(talker, myself, 600);
                    //            break;
                    //    }

                    break;
            }
        }

        public void ShowVariationCancelWindow(L2Player talker)
        {
            talker.SendMessage("citizen.ShowVariationCancelWindow");
        }

        public void ShowVariationMakeWindow(L2Player talker)
        {
            talker.SendMessage("citizen.ShowVariationMakeWindow");
        }
    }
}