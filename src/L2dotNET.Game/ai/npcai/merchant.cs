using L2dotNET.GameService.network.l2send;
using L2dotNET.GameService.tables;
using L2dotNET.GameService.tables.multisell;

namespace L2dotNET.GameService.ai.npcai
{
    class merchant : citizen
    {
        public string ShopName = "";
        public string fnSell = "msell.htm";
        public string fnBuy = "mbye.htm";
        public string fnUnableItemSell = "muib.htm";
        public string fnYouAreChaotic = "mcm.htm";
        public string fnNowSiege = "mns.htm";
        public int VillageLevel = 0;

        public override void Talked(L2Player talker)
        {
            talker.ShowHtm(GetDialog(talker.Karma > 0 ? "fnYouAreChaotic" : "fnHi"), myself);
        }

        public override void TalkedReply(L2Player talker, int ask, int reply)
        {
            if (ask == -1)
            {
                switch (reply)
                {
                    case 0: NpcData.Instance.Buylist(talker, myself, 0);
                        break;
                    case 1: NpcData.Instance.Buylist(talker, myself, 1);
                        break;
                    case 2: NpcData.Instance.Buylist(talker, myself, 2);
                        break;
                    case 3: NpcData.Instance.Buylist(talker, myself, 3);
                        break;
                    case 4: NpcData.Instance.Buylist(talker, myself, 4);
                        break;
                    case 5: NpcData.Instance.Buylist(talker, myself, 5);
                        break;
                    case 6: NpcData.Instance.Buylist(talker, myself, 6);
                        break;
                    case 7: NpcData.Instance.Buylist(talker, myself, 7);
                        break;
                    case 8:
                        talker.sendPacket(new ExBuySellList_Buy(talker.getAdena()));
                        talker.sendPacket(new ExBuySellList_Sell(talker));
                        break;
                }
            }
            else if (ask == -506)
            {
                MultiSell.Instance.ShowList(talker, myself, 212);
            }
            else if (ask == -507)
            {
                MultiSell.Instance.ShowList(talker, myself, 221);
            }
            else if (ask == -510)
            {
                if (reply == 1)
                {
                    if (talker.Level < 40)
                        talker.ShowHtm("reflect_weapon_none.htm", myself);
                    else if (talker.Level >= 40 && talker.Level < 46)
                        talker.ShowHtm("reflect_weapon_d.htm", myself);
                    else if (talker.Level >= 46 && talker.Level < 52)
                        talker.ShowHtm("reflect_weapon_c.htm", myself);
                    else if (talker.Level >= 52)
                        talker.ShowHtm("reflect_weapon_b.htm", myself);
                }
            }
            else
                base.TalkedReply(talker, ask, reply);
        }
    }
}
