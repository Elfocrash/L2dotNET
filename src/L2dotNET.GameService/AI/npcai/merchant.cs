using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tables;
using L2dotNET.GameService.Tables.Multisell;

namespace L2dotNET.GameService.AI.NpcAI
{
    class Merchant : Citizen
    {
        public string ShopName = string.Empty;
        public string FnSell = "msell.htm";
        public string FnBuy = "mbye.htm";
        public string FnUnableItemSell = "muib.htm";
        public string FnYouAreChaotic = "mcm.htm";
        public string FnNowSiege = "mns.htm";
        public int VillageLevel = 0;

        public override void Talked(L2Player talker)
        {
            talker.ShowHtm(GetDialog(talker.Karma > 0 ? "fnYouAreChaotic" : "fnHi"), Myself);
        }

        public override void TalkedReply(L2Player talker, int ask, int reply)
        {
            switch (ask)
            {
                case -1:
                    switch (reply)
                    {
                        case 0:
                            NpcData.Instance.Buylist(talker, Myself, 0);
                            break;
                        case 1:
                            NpcData.Instance.Buylist(talker, Myself, 1);
                            break;
                        case 2:
                            NpcData.Instance.Buylist(talker, Myself, 2);
                            break;
                        case 3:
                            NpcData.Instance.Buylist(talker, Myself, 3);
                            break;
                        case 4:
                            NpcData.Instance.Buylist(talker, Myself, 4);
                            break;
                        case 5:
                            NpcData.Instance.Buylist(talker, Myself, 5);
                            break;
                        case 6:
                            NpcData.Instance.Buylist(talker, Myself, 6);
                            break;
                        case 7:
                            NpcData.Instance.Buylist(talker, Myself, 7);
                            break;
                        case 8:
                            talker.SendPacket(new ExBuySellListBuy(talker.GetAdena()));
                            talker.SendPacket(new ExBuySellListSell(talker));
                            break;
                    }

                    break;
                case -506:
                    MultiSell.Instance.ShowList(talker, Myself, 212);
                    break;
                case -507:
                    MultiSell.Instance.ShowList(talker, Myself, 221);
                    break;
                case -510:
                    if (reply == 1)
                    {
                        if (talker.Level < 40)
                            talker.ShowHtm("reflect_weapon_none.htm", Myself);
                        else
                        {
                            if ((talker.Level >= 40) && (talker.Level < 46))
                                talker.ShowHtm("reflect_weapon_d.htm", Myself);
                            else
                            {
                                if ((talker.Level >= 46) && (talker.Level < 52))
                                    talker.ShowHtm("reflect_weapon_c.htm", Myself);
                                else
                                {
                                    if (talker.Level >= 52)
                                        talker.ShowHtm("reflect_weapon_b.htm", Myself);
                                }
                            }
                        }
                    }
                    break;
                default:
                    base.TalkedReply(talker, ask, reply);
                    break;
            }
        }
    }
}