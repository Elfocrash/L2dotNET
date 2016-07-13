using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Npcs.Ai
{
    class BroadcastingTower : AiTemplate
    {
        private readonly int[][] _data;

        public BroadcastingTower()
        {
            Id = 31031;
            _data = new int[21][];
            _data[0] = new[] { 612, 620, 500, -18347, 114000, -2360 };
            _data[1] = new[] { 612, 621, 500, -18347, 113255, -2447 };
            _data[2] = new[] { 613, 622, 500, 22321, 155785, -2604 };
            _data[3] = new[] { 613, 623, 500, 22321, 156492, -2627 };
            _data[4] = new[] { 614, 624, 500, 112000, 144864, -2445 };
            _data[5] = new[] { 614, 625, 500, 112657, 144864, -2525 };
            _data[6] = new[] { 615, 626, 500, 116260, 244600, -775 };
            _data[7] = new[] { 615, 627, 500, 116260, 245264, -721 };
            _data[8] = new[] { 616, 628, 500, 78100, 36950, -2242 };
            _data[9] = new[] { 616, 629, 500, 78744, 36950, -2244 };
            _data[10] = new[] { 617, 630, 500, 147457, 9601, -233 };
            _data[11] = new[] { 617, 631, 500, 147457, 8720, -252 };
            _data[12] = new[] { 1243, 1244, 500, 147542, -43543, -1328 };
            _data[13] = new[] { 1243, 1245, 500, 147465, -45259, -1328 };
            _data[14] = new[] { 1420, 1421, 500, 20598, -49113, -300 };
            _data[15] = new[] { 1420, 1422, 500, 18702, -49150, -600 };
            _data[16] = new[] { 1423, 1424, 500, 77541, -147447, 353 };
            _data[17] = new[] { 1423, 1425, 500, 77541, -149245, 353 };
            _data[18] = new[] { 619, 634, 80, 148416, 46724, -3000 };
            _data[19] = new[] { 619, 635, 80, 149500, 46724, -3000 };
            _data[20] = new[] { 619, 636, 80, 150511, 46724, -3000 };

            ChatOvr = true;
            DialogOvr = true;
        }

        public override void OnShowChat(L2Player player, L2Npc npc)
        {
            List<int> ar = new List<int>();
            string text = "";

            foreach (int[] d in _data.Where(d => !ar.Contains(d[0])))
            {
                ar.Add(d[0]);
            }

            foreach (int val in ar)
            {
                text += "<a action=\"bypass -h menu_select?ask=-2&reply=" + val + "\">&$" + val + ";</a><br1>";
            }

            player.ShowHtmPlain(text, npc);
        }

        public override void OnDialog(L2Player player, int ask, int reply, L2Npc npc)
        {
            switch (ask)
            {
                case -2:
                    ShowGroup(player, npc, reply);
                    break;
                case -3:
                    ObserveIt(player, reply);
                    break;
            }
        }

        private void ObserveIt(L2Player player, int reply)
        {
            int[] dx = _data.FirstOrDefault(d => d[1] == reply);

            if ((dx != null) && (player.GetAdena() < dx[2]))
            {
                player.SendSystemMessage(SystemMessage.SystemMessageId.YouNotEnoughAdena);
                player.SendActionFailed();
                return;
            }

            if (dx == null)
            {
                return;
            }

            player.ReduceAdena(dx[2]);

            player.ClearKnowns(true);

            player.Obsx = player.X;
            player.Obsy = player.Y;
            player.Obsz = player.Z;

            player.SendPacket(new ObservationMode(dx[3], dx[4], dx[5]));
        }

        private void ShowGroup(L2Player player, L2Npc npc, int group)
        {
            List<int[]> ar = _data.Where(d => d[0] == group).ToList();

            string text = "&$650;<br>";
            foreach (int[] val in ar)
            {
                text += "<a action=\"bypass -h menu_select?ask=-3&reply=" + val[1] + "\">&$" + val[1] + ";</a><br1>";
            }

            player.ShowHtmPlain(text, npc);
        }
    }
}