using System.Collections.Generic;
using L2dotNET.Game.db;
using L2dotNET.Game.model.items;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.model.player.telebooks
{
    public class TeleportBook
    {
        public SortedList<byte, TelBook_Mark> bookmarks;

        private int MY_TELEPORT_FLAG            = 20033;
        private int MY_TELEPORT_SCROLL          = 13016;
        private int MY_TELEPORT_SCROLL_EVENT    = 13302;

        public TeleportBook()
        {
            bookmarks = new SortedList<byte, TelBook_Mark>();
        }

        public void SaveMark(L2Player player, string name, int icon, string tag)
        {
            if (bookmarks.Count >= player.TelbookLimit)
            {
                player.sendSystemMessage(2358); //You have no space to save the teleport location.
                return;
            }

            if (!player.hasItem(MY_TELEPORT_FLAG, 1))
            {
                player.sendSystemMessage(6501); //You cannot bookmark this location because you do not have a My Teleport Flag.
                return;
            }

            byte max = (byte)(bookmarks.Count +2), id = 1;

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

            SQL_Block sqb = new SQL_Block("user_telbooks");
            sqb.param("ownerId", player.ObjID);
            sqb.param("id", mark.id);
            sqb.param("locx", mark.x);
            sqb.param("locy", mark.y);
            sqb.param("locz", mark.z);
            sqb.param("icon", mark.icon);
            sqb.param("name", mark.name);
            sqb.param("tag", mark.tag);
            sqb.sql_insert(false);

            player.sendPacket(new ExGetBookMarkInfo(player.TelbookLimit, this));
        }

        public void ModifyMark(L2Player player, byte id, string name, int icon, string tag)
        {
            if (!bookmarks.ContainsKey(id))
            {
                player.sendMessage("You do not have bookmark with id #"+id);
                player.sendActionFailed();
                return;
            }

            TelBook_Mark mark = bookmarks[id];
            mark.name = name;
            mark.icon = icon;
            mark.tag = tag;

            SQL_Block sqb = new SQL_Block("user_telbooks");
            sqb.param("name", name);
            sqb.param("icon", icon);
            sqb.param("tag", tag);
            sqb.where("ownerId", player.ObjID);
            sqb.where("id", id);
            sqb.sql_update(false);

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

            SQL_Block sqb = new SQL_Block("user_telbooks");
            sqb.where("ownerId", player.ObjID);
            sqb.where("id", id);
            sqb.sql_delete(false);

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
                player.sendSystemMessage(2359); //You cannot teleport because you do not have a teleport item.
                return;
            }

            if (player._isDead)
            {
                player.sendSystemMessage(2354); //You cannot use My Teleports while you are dead.
                return;
            }
            else if (player.isInWater())
            {
                player.sendSystemMessage(2356); //You cannot use My Teleports underwater.
                return;
            }
            else if (player.IsFlying)
            {
                player.sendSystemMessage(2351); //You cannot use My Teleports while flying.
                return;
            }

            player.Inventory.destroyItem(scroll, 1, true, true);

            TelBook_Mark mark = bookmarks[id];
            player.teleport(mark.x, mark.y, mark.z);
        }
    }
}
