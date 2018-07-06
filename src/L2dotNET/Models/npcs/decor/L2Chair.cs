﻿using L2dotNET.Templates;

namespace L2dotNET.Models.Npcs.Decor
{
    public sealed class L2Chair : L2StaticObject
    {
        public bool IsUsedAlready = false;

        public L2Chair(int objectId, CharTemplate template) : base(objectId, template)
        {
            Closed = 0;
            //MaxHp = 0;
            CharStatus.SetCurrentHp(0);
        }

        public override string AsString()
        {
            return $"L2Chair:{ObjectId} {StaticId}";
        }
    }
}