using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Npcs.Ai
{
    class GrandmasterTotal
    {
        public static void CreateClan(L2Player player, string name, L2Npc npc) { }

        public static void OnReply(L2Player player, int reply, L2Npc npc)
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
                        if (player.Clan.LeaderId == player.ObjId)
                        {
                            player.ShowHtm("pl003.htm", npc);
                        }
                        else
                        {
                            player.ShowHtm("pl004.htm", npc);
                        }

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