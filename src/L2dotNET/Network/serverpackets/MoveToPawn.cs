﻿using L2dotNET.Models;

namespace L2dotNET.Network.serverpackets
{
    class MoveToPawn : GameserverPacket
    {
        private readonly int _id;
        private readonly int _target;
        private readonly int _dist;
        private int _x;
        private int _y;
        private int _z;

        public MoveToPawn(int id, L2Object target, int dist, int x, int y, int z)
        {
            _id = id;
            _target = target.CharacterId;
            _dist = dist;
            _x = x;
            _y = y;
            _z = z;
        }

        public MoveToPawn(L2Character character, L2Object target, int dist)
        {
            _id = character.CharacterId;
            _target = target.CharacterId;
            _dist = dist;
            _x = character.X;
            _y = character.Y;
            _z = character.Z;
        }

        public override void Write()
        {
            WriteByte(0x60);

            WriteInt(_id);
            WriteInt(_target);
            WriteInt(_dist);
            WriteInt(_x);
            WriteInt(_y);
            WriteInt(_z);
        }
    }
}