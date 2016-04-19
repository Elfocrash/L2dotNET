using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L2dotNET.Game.geo
{
    class MByteBuffer
    {
        private byte[] db;
        public MByteBuffer(byte[] db)
        {
            this.db = db;
        }

        public byte get(int index)
        {
            return db[index];
        }

        public short getShort(int index)
        {
            return BitConverter.ToInt16(db, index);
        }
    }
}
