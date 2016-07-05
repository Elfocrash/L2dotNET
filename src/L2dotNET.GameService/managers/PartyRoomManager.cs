using System.Collections.Generic;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Managers
{
    public class PartyRoomManager
    {
        private static readonly PartyRoomManager M = new PartyRoomManager();

        public static PartyRoomManager GetInstance()
        {
            return M;
        }

        public SortedList<int, L2PartyRoom> Rooms = new SortedList<int, L2PartyRoom>();
        public static int IdFactory = 20;

        public L2PartyRoom NewRoom(L2Player player, int roomId, int maxMembers, int minLevel, int maxLevel, int lootDist, string roomTitle)
        {
            L2PartyRoom room = new L2PartyRoom { RoomId = roomId, MaxMembers = maxMembers, MinLevel = minLevel, MaxLevel = maxLevel, LootDist = lootDist, Title = roomTitle, LeaderId = player.ObjId };
            IdFactory++;
            Rooms.Add(roomId, room);

            return room;
        }
    }
}