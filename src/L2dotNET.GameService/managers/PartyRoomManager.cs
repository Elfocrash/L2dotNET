using System.Collections.Generic;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Managers
{
    public class PartyRoomManager
    {
        private static readonly PartyRoomManager m = new PartyRoomManager();

        public static PartyRoomManager getInstance()
        {
            return m;
        }

        public SortedList<int, L2PartyRoom> _rooms = new SortedList<int, L2PartyRoom>();
        public static int _idFactory = 20;

        public L2PartyRoom newRoom(L2Player player, int roomId, int maxMembers, int minLevel, int maxLevel, int lootDist, string roomTitle)
        {
            L2PartyRoom room = new L2PartyRoom();
            room._roomId = roomId;
            room._maxMembers = maxMembers;
            room._minLevel = minLevel;
            room._maxLevel = maxLevel;
            room._lootDist = lootDist;
            room._title = roomTitle;
            room._leaderId = player.ObjId;
            _idFactory++;
            _rooms.Add(roomId, room);

            return room;
        }
    }
}