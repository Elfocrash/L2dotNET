using L2dotNET.GameService.model.player.telebooks;

namespace L2dotNET.GameService.network.l2send
{
    class ExGetBookMarkInfo : GameServerNetworkPacket
    {
        private TeleportBook book;
        private byte current;

        public ExGetBookMarkInfo(byte current, TeleportBook teleportBook)
        {
            this.book = teleportBook;
            this.current = current;

        }

        protected internal override void write()
        {
		    writeC(0xFE);
		    writeH(0x84);
		    writeD(0x00);
            writeD(current);
            writeD(book == null ? 0 : book.bookmarks.Count);

            if (book != null)
            {
                foreach (TelBook_Mark mark in book.bookmarks.Values)
                {
                    writeD(mark.id);
                    writeD(mark.x);
                    writeD(mark.y);
                    writeD(mark.z);
                    writeS(mark.name);
                    writeD(mark.icon);
                    writeS(mark.tag);
                }
            }
        }
    }
}
