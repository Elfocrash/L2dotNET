using L2dotNET.Game.db;
using L2dotNET.Game.model.communities;
using L2dotNET.Game.tables;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.model.npcs.ai
{
    class grandmaster_total
    {
        public static void createClan(L2Player player, string name, L2Citizen npc)
        {
            
        }

        public static void onReply(L2Player player, int reply, L2Citizen npc)
        {
            switch (reply)
            {
                case 0: //new clan
                    {
                        if (player.Level < 10)
                        {
                            player.ShowHtm("pl002.htm", npc);
                            return;
                        }

                        if (player.Clan != null)
                        {
                            if (player.Clan.LeaderID == player.ObjID)
                                player.ShowHtm("pl003.htm", npc);
                            else
                                player.ShowHtm("pl004.htm", npc);

                            return;
                        }

                        player.ShowHtm("pl005.htm", npc);
                    }
                    break;
                case 1: //Повысить
                    player.ShowHtm("pl013.htm", npc);
                    break;
                case 2: //Распустить
                    break;
                case 3: //Восстановить
                    break;
                case 4: //Умения
                    break;
                case 5: //Управлять Академией
                    break;
                case 6: //Управлять Подразделениями Стражей
                    break;
                case 9: //УправлятьПодразделениями Рыцарей
                    break;
                case 13: //.Передать Полномочия Лидера Клана
                    break;
                case 100: //main
                    player.ShowHtm("pl001.htm", npc);
                    break;
            }
        }
    }
}
