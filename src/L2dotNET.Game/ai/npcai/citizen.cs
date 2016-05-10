using System;
using System.Timers;
using L2dotNET.Game.ai.template;
using L2dotNET.Game.managers;
using L2dotNET.Game.model.structures;
using L2dotNET.Game.network.l2send;
using L2dotNET.Game.tables;
using L2dotNET.Game.tables.multisell;

namespace L2dotNET.Game.ai.npcai
{
    class citizen : AI
    {
        public string fnHi = "lector001.htm";
        public string fnFeudInfo = "gludio_feud_manager001.htm";
        public string fnNoFeudInfo = "farm_messenger002.htm";
        public string fnBracketL = "[";
        public string fnBracketR = "]";
        public string fnFlagMan = "flagman.htm";
        public int MoveAroundSocial = 0;
        public int MoveAroundSocial1 = 0;
        public string ai_type = "pet_around_pet_manager";
        public int HavePet = 0;
        public int silhouette = 20130;
        public int FriendShip1 = 0;
        public int FriendShip2 = 0;
        public int FriendShip3 = 0;
        public int FriendShip4 = 0;
        public int FriendShip5 = 0;
        public string fnNoFriend = "citizen_html";
        public int NoFnHi = 0;

        public Timer socialMoveTimer;

        public override void Talked(L2Player talker)
        {
            if (NoFnHi == 1)
                return;

            if (FriendShip1 == 0)
                talker.ShowHtm(GetDialog("fnHi"), myself);
            else if (talker.hasSomeOfThisItems())
                talker.ShowHtm(GetDialog("fnFlagMan"), myself);
            else if (talker.hasSomeOfThisItems(FriendShip1, FriendShip2, FriendShip3, FriendShip4, FriendShip5))
                talker.ShowHtm(GetDialog("fnHi"), myself);
            else
                talker.ShowHtm(GetDialog("fnNoFriend"), myself);
        }

        public override void Created()
        {
            if (HavePet == 1)
                myself.CreateOnePrivateEx(silhouette, ai_type, myself.X + 10, myself.Y + 10, myself.Z);

            if (MoveAroundSocial > 0 || MoveAroundSocial1 > 0)
            {
                if (socialMoveTimer == null)
                {
                    socialMoveTimer = new Timer();
                    socialMoveTimer.Interval = 10000;
                    socialMoveTimer.Elapsed += new ElapsedEventHandler(SocialTask);
                }

                socialMoveTimer.Enabled = true;
            }
        }

        private void SocialTask(object sender, ElapsedEventArgs e)
        {
            if (myself.CurHP > myself.MaxHp * 0.400000 && !myself.Dead)
            {
                if (MoveAroundSocial > 0 && new Random().Next(100) < 40)
                {
                    //myself::AddEffectActionDesire( myself.sm, 3, MoveAroundSocial * 1000 / 30, 50 );
                }
                else if (MoveAroundSocial1 > 0 && new Random().Next(100) < 40)
                {
                    //myself::AddEffectActionDesire( myself.sm, 2, MoveAroundSocial1 * 1000 / 30, 50 );
                }
            }
        }

        public override void TalkedReply(L2Player talker, int ask, int reply)
        {
            if (ask == -1000)
            {
                if (reply == 0)
                {
                    talker.ShowHtm(GetDialog("fnHi"), myself);
                }
                else if (reply == 1)
                {
                    NpcHtmlMessage htm = new NpcHtmlMessage(talker, GetDialog(myself.residenceId > 0 ? "fnFeudInfo" : "fnNoFeudInfo"), myself.ObjID);

                    if (myself.residenceId > 0)
                    {
                        Castle castle = CastleManager.getInstance().get(myself.residenceId);
                        htm.replace("<?my_pledge_name?>", castle.OwningClanName);
                        htm.replace("<?my_owner_name?>", castle.OwningPlayerName);
                        htm.replace("<?current_tax_rate?>", (int)castle.Tax);
                    }

                    htm.replace("<?kingdom_name?>", FString.getInstance().get(myself.residenceId < 7 ? 1001000 : 1001100));
                    htm.replace("<?feud_name?>", FString.getInstance().get(myself.residenceId + 1001000));

                    talker.sendPacket(htm);
                }
            }
            else if (ask == -303)
            {
                if (reply == 579)
                {
                    if (talker.Level > 40 && talker.Level < 46)
                    {
                            MultiSell.Instance.ShowList(talker, myself, reply);
                    }
                }
                else if (reply == 580)
                {
                    if (talker.Level >= 46 && talker.Level < 52)
                    {
                            MultiSell.Instance.ShowList(talker, myself, reply);
                    }
                }
                else if (reply == 581)
                {
                    if (talker.Level >= 52)
                    {
                            MultiSell.Instance.ShowList(talker, myself, reply);
                    }
                }
                else
                    MultiSell.Instance.ShowList(talker, myself, reply);
            }
            else if (ask == -503)
            {
                if (reply == 100)
                    ShowVariationMakeWindow(talker);
                else if (reply == 200)
                    ShowVariationCancelWindow(talker);
            }
            else if (ask == -601)
            {
                if (reply == 0)
                {
                    if (!talker.hasAllOfThisItems(8957, 8958, 8959))
                        talker.ShowHtm("welcomeback003.htm", myself);
                    else
                        talker.ShowHtm("welcomeback004.htm", myself);
                }
                else if (reply == 0)
                {
                    if (!talker.hasAllOfThisItems(8957, 8958, 8959))
                        talker.ShowHtm("welcome_lin2_cat002.htm", myself);
                    else
                        talker.ShowHtm("welcome_lin2_cat004.htm", myself);
                }
                else if (reply == 2)
                {
                    if(talker.Level < 20)
                        MultiSell.Instance.ShowList(talker, myself, 583);
                    else if(talker.Level >= 20 && talker.Level < 40)
                        MultiSell.Instance.ShowList(talker, myself, 584);
                    else if (talker.Level >= 40 && talker.Level < 52)
                        MultiSell.Instance.ShowList(talker, myself, 585);
                    else if (talker.Level >= 52 && talker.Level < 61)
                        MultiSell.Instance.ShowList(talker, myself, 586);
                    else if (talker.Level >= 61 && talker.Level < 76)
                        MultiSell.Instance.ShowList(talker, myself, 587);
                    else if (talker.Level >= 76)
                        MultiSell.Instance.ShowList(talker, myself, 588);
                }
                else if (reply == 3)
                {
                    if (talker.Level < 20)
                        MultiSell.Instance.ShowList(talker, myself, 589);
                    else if (talker.Level >= 20 && talker.Level < 40)
                        MultiSell.Instance.ShowList(talker, myself, 590);
                    else if (talker.Level >= 40 && talker.Level < 52)
                        MultiSell.Instance.ShowList(talker, myself, 591);
                    else if (talker.Level >= 52 && talker.Level < 61)
                        MultiSell.Instance.ShowList(talker, myself, 592);
                    else if (talker.Level >= 61 && talker.Level < 76)
                        MultiSell.Instance.ShowList(talker, myself, 593);
                    else if (talker.Level >= 76)
                        MultiSell.Instance.ShowList(talker, myself, 594);
                }
                else if (reply == 4)
                {
                    if (talker.Level < 20)
                        MultiSell.Instance.ShowList(talker, myself, 595);
                    else if (talker.Level >= 20 && talker.Level < 40)
                        MultiSell.Instance.ShowList(talker, myself, 596);
                    else if (talker.Level >= 40 && talker.Level < 52)
                        MultiSell.Instance.ShowList(talker, myself, 597);
                    else if (talker.Level >= 52 && talker.Level < 61)
                        MultiSell.Instance.ShowList(talker, myself, 598);
                    else if (talker.Level >= 61 && talker.Level < 76)
                        MultiSell.Instance.ShowList(talker, myself, 601);
                    else if (talker.Level >= 76)
                        MultiSell.Instance.ShowList(talker, myself, 600);
                }
            }
        }

        public void ShowVariationCancelWindow(L2Player talker)
        {
            talker.sendMessage("citizen.ShowVariationCancelWindow");
        }

        public void ShowVariationMakeWindow(L2Player talker)
        {
            talker.sendMessage("citizen.ShowVariationMakeWindow");
        }
    }
}
