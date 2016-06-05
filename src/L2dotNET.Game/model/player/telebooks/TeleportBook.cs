using System.Collections.Generic;
using L2dotNET.GameService.model.items;
using L2dotNET.GameService.network.l2send;

namespace L2dotNET.GameService.model.player.telebooks
{
    public class TeleportBook
    {
        public SortedList<byte, TelBook_Mark> bookmarks;

        private readonly int MY_TELEPORT_FLAG = 20033;
        private readonly int MY_TELEPORT_SCROLL = 13016;
        private readonly int MY_TELEPORT_SCROLL_EVENT = 13302;

        public TeleportBook()
        {
            bookmarks = new SortedList<byte, TelBook_Mark>();
        }

        public void SaveMark(L2Player player, string name, int icon, string tag)
        {
            if (bookmarks.Count >= player.TelbookLimit)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.NO_SPACE_TO_SAVE_TELEPORT_LOCATION);
                return;
            }

            if (!player.hasItem(MY_TELEPORT_FLAG, 1))
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.CANNOT_BOOKMARK_THIS_LOCATION_BECAUSE_NO_MY_TELEPORT_FLAG);
                return;
            }

            byte max = (byte)(bookmarks.Count + 2), id = 1;

            for (byte x = 1; x < max; x++)
                if (!bookmarks.ContainsKey(x))
                {
                    id = x;
                    break;
                }

            player.Inventory.destroyItem(MY_TELEPORT_FLAG, 1, true, true);

            TelBook_Mark mark = new TelBook_Mark();
            mark.id = id;
            mark.x = player.X;
            mark.y = player.Y;
            mark.z = player.Z;
            mark.icon = icon;
            mark.name = name;
            mark.tag = tag;

            bookmarks.Add(mark.id, mark);

            //SQL_Block sqb = new SQL_Block("user_telbooks");
            //sqb.param("ownerId", player.ObjID);
            //sqb.param("id", mark.id);
            //sqb.param("locx", mark.x);
            //sqb.param("locy", mark.y);
            //sqb.param("locz", mark.z);
            //sqb.param("icon", mark.icon);
            //sqb.param("name", mark.name);
            //sqb.param("tag", mark.tag);
            //sqb.sql_insert(false);

            player.sendPacket(new ExGetBookMarkInfo(player.TelbookLimit, this));
        }

        public void ModifyMark(L2Player player, byte id, string name, int icon, string tag)
        {
            if (!bookmarks.ContainsKey(id))
            {
                player.sendMessage("You do not have bookmark with id #" + id);
                player.sendActionFailed();
                return;
            }

            TelBook_Mark mark = bookmarks[id];
            mark.name = name;
            mark.icon = icon;
            mark.tag = tag;

            //SQL_Block sqb = new SQL_Block("user_telbooks");
            //sqb.param("name", name);
            //sqb.param("icon", icon);
            //sqb.param("tag", tag);
            //sqb.where("ownerId", player.ObjID);
            //sqb.where("id", id);
            //sqb.sql_update(false);

            player.sendPacket(new ExGetBookMarkInfo(player.TelbookLimit, this));
        }

        public void DeleteMark(L2Player player, byte id)
        {
            if (!bookmarks.ContainsKey(id))
            {
                player.sendMessage("You do not have bookmark with id #" + id);
                player.sendActionFailed();
                return;
            }

            lock (bookmarks)
                bookmarks.Remove(id);

            //SQL_Block sqb = new SQL_Block("user_telbooks");
            //sqb.where("ownerId", player.ObjID);
            //sqb.where("id", id);
            //sqb.sql_delete(false);

            player.sendPacket(new ExGetBookMarkInfo(player.TelbookLimit, this));
        }

        public void UseMark(L2Player player, byte id)
        {
            if (!bookmarks.ContainsKey(id))
            {
                player.sendMessage("You do not have bookmark with id #" + id);
                player.sendActionFailed();
                return;
            }

            L2Item scroll = player.Inventory.getItemById(MY_TELEPORT_SCROLL);

            if (scroll == null)
                scroll = player.Inventory.getItemById(MY_TELEPORT_SCROLL_EVENT);

            if (scroll == null)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.CANNOT_TELEPORT_BECAUSE_DO_NOT_HAVE_TELEPORT_ITEM);
                return;
            }

            if (player.Dead)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.CANNOT_USE_MY_TELEPORTS_WHILE_DEAD);
                return;
            }
            else if (player.isInWater())
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.CANNOT_USE_MY_TELEPORTS_UNDERWATER);
                return;
            }
            else if (player.IsFlying)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.CANNOT_USE_MY_TELEPORTS_WHILE_FLYING);
                return;
            }

            player.Inventory.destroyItem(scroll, 1, true, true);

            TelBook_Mark mark = bookmarks[id];
            player.teleport(mark.x, mark.y, mark.z);
        }
    }
}