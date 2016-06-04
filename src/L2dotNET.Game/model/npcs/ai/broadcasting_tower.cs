using System.Collections.Generic;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.model.npcs.ai
{
    class broadcasting_tower : AITemplate
    {
        private int[][] data;
        public broadcasting_tower()
        {
            id = 31031;
            data = new int[21][];
            data[0] = new int[] { 612, 620, 500, -18347, 114000, -2360 };
            data[1] = new int[] { 612, 621, 500, -18347, 113255, -2447 };
            data[2] = new int[] { 613, 622, 500, 22321, 155785, -2604 };
            data[3] = new int[] { 613, 623, 500, 22321, 156492, -2627 };
            data[4] = new int[] { 614, 624, 500, 112000, 144864, -2445 };
            data[5] = new int[] { 614, 625, 500, 112657, 144864, -2525 };
            data[6] = new int[] { 615, 626, 500, 116260, 244600, -775 };
            data[7] = new int[] { 615, 627, 500, 116260, 245264, -721 };
            data[8] = new int[] { 616, 628, 500, 78100, 36950, -2242 };
            data[9] = new int[] { 616, 629, 500, 78744, 36950, -2244 };
            data[10] = new int[] { 617, 630, 500, 147457, 9601, -233 };
            data[11] = new int[] { 617, 631, 500, 147457, 8720, -252 };
            data[12] = new int[] { 1243, 1244, 500, 147542, -43543, -1328 };
            data[13] = new int[] { 1243, 1245, 500, 147465, -45259, -1328 };
            data[14] = new int[] { 1420, 1421, 500, 20598, - 49113, - 300 };
            data[15] = new int[] { 1420, 1422, 500, 18702, - 49150, - 600 };
            data[16] = new int[] { 1423, 1424, 500, 77541, -147447, 353 };
            data[17] = new int[] { 1423, 1425, 500, 77541, -149245, 353 };
            data[18] = new int[] { 619, 634, 80, 148416, 46724, -3000 };
            data[19] = new int[] { 619, 635, 80, 149500, 46724, -3000 };
            data[20] = new int[] { 619, 636, 80, 150511, 46724, -3000 };

            chatOvr = true;
            dialogOvr = true;
        }

        public override void onShowChat(L2Player player, L2Citizen npc)
        {
            List<int> ar = new List<int>();
            string text = "";

            foreach (int[] d in data)
            {
                if(ar.Contains(d[0]))
                    continue;

                ar.Add(d[0]);
            }

            foreach (int id in ar)
            {
                text += "<a action=\"bypass -h menu_select?ask=-2&reply=" + id + "\">&$" + id + ";</a><br1>";
            }

            player.ShowHtmPlain(text, npc);
        }

        public override void onDialog(L2Player player, int ask, int reply, L2Citizen npc)
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

        private void observeIt(L2Player player, L2Citizen npc, int reply)
        {
            int[] dx = null;

            foreach (int[] d in data)
            {
                if (d[1] == reply)
                {
                    dx = d;
                    break;
                }
            }

            if (player.getAdena() < dx[2])
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.YOU_NOT_ENOUGH_ADENA);
                player.sendActionFailed();
                return;
            }

            player.reduceAdena(dx[2], true, true);

            player.clearKnowns(true);

            player._obsx = player.X;
            player._obsy = player.Y;
            player._obsz = player.Z;

            player.sendPacket(new ObservationMode(dx[3], dx[4], dx[5]));
        }


        private void showGroup(L2Player player, L2Citizen npc, int group)
        {
            List<int[]> ar = new List<int[]>();
            string text = "&$650;<br>";

            foreach (int[] d in data)
            {

                if(d[0] == group)
                    ar.Add(d);
            }

            foreach (int[] id in ar)
            {
                text += "<a action=\"bypass -h menu_select?ask=-3&reply=" + id[1] + "\">&$" + id[1] + ";</a><br1>";
            }

            player.ShowHtmPlain(text, npc);
        }
    }
}
