using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Npcs.Ai
{
    class broadcasting_tower : AITemplate
    {
        private readonly int[][] data;

        public broadcasting_tower()
        {
            id = 31031;
            data = new int[21][];
            data[0] = new[] { 612, 620, 500, -18347, 114000, -2360 };
            data[1] = new[] { 612, 621, 500, -18347, 113255, -2447 };
            data[2] = new[] { 613, 622, 500, 22321, 155785, -2604 };
            data[3] = new[] { 613, 623, 500, 22321, 156492, -2627 };
            data[4] = new[] { 614, 624, 500, 112000, 144864, -2445 };
            data[5] = new[] { 614, 625, 500, 112657, 144864, -2525 };
            data[6] = new[] { 615, 626, 500, 116260, 244600, -775 };
            data[7] = new[] { 615, 627, 500, 116260, 245264, -721 };
            data[8] = new[] { 616, 628, 500, 78100, 36950, -2242 };
            data[9] = new[] { 616, 629, 500, 78744, 36950, -2244 };
            data[10] = new[] { 617, 630, 500, 147457, 9601, -233 };
            data[11] = new[] { 617, 631, 500, 147457, 8720, -252 };
            data[12] = new[] { 1243, 1244, 500, 147542, -43543, -1328 };
            data[13] = new[] { 1243, 1245, 500, 147465, -45259, -1328 };
            data[14] = new[] { 1420, 1421, 500, 20598, -49113, -300 };
            data[15] = new[] { 1420, 1422, 500, 18702, -49150, -600 };
            data[16] = new[] { 1423, 1424, 500, 77541, -147447, 353 };
            data[17] = new[] { 1423, 1425, 500, 77541, -149245, 353 };
            data[18] = new[] { 619, 634, 80, 148416, 46724, -3000 };
            data[19] = new[] { 619, 635, 80, 149500, 46724, -3000 };
            data[20] = new[] { 619, 636, 80, 150511, 46724, -3000 };

            chatOvr = true;
            dialogOvr = true;
        }

        public override void onShowChat(L2Player player, L2Npc npc)
        {
            List<int> ar = new List<int>();
            string text = "";

            foreach (int[] d in data.Where(d => !ar.Contains(d[0])))
                ar.Add(d[0]);

            foreach (int val in ar)
                text += "<a action=\"bypass -h menu_select?ask=-2&reply=" + val + "\">&$" + val + ";</a><br1>";

            player.ShowHtmPlain(text, npc);
        }

        public override void onDialog(L2Player player, int ask, int reply, L2Npc npc)
        {
            switch (ask)
            {
                case -2:
                    showGroup(player, npc, reply);
                    break;
                case -3:
                    observeIt(player, npc, reply);
                    break;
            }
        }

        private void observeIt(L2Player player, L2Npc npc, int reply)
        {
            int[] dx = data.FirstOrDefault(d => d[1] == reply);

            if ((dx != null) && (player.GetAdena() < dx[2]))
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.YOU_NOT_ENOUGH_ADENA);
                player.sendActionFailed();
                return;
            }

            if (dx != null)
            {
                player.ReduceAdena(dx[2]);

                player.clearKnowns(true);

                player._obsx = player.X;
                player._obsy = player.Y;
                player._obsz = player.Z;

                player.sendPacket(new ObservationMode(dx[3], dx[4], dx[5]));
            }
        }

        private void showGroup(L2Player player, L2Npc npc, int group)
        {
            string text = "&$650;<br>";

            List<int[]> ar = data.Where(d => d[0] == group).ToList();

            foreach (int[] val in ar)
                text += "<a action=\"bypass -h menu_select?ask=-3&reply=" + val[1] + "\">&$" + val[1] + ";</a><br1>";

            player.ShowHtmPlain(text, npc);
        }
    }
}