using System;
using System.Linq;
using L2dotNET.Models.Player;
using L2dotNET.Network.serverpackets;
using L2dotNET.Templates;

namespace L2dotNET.Models.Npcs.Decor
{
    public class L2StaticObject : L2Character
    {
        /// <summary>
        /// EL2_DOOR (1), EL2_AIRSHIPKEY (3)
        /// </summary>
        public int StaticId;
        public int MeshId = 0;
        public int StructureId = 0;
        public int Type = 0;
        public byte Closed = 1;
        public ShowTownMap TownMap;
        public string Htm;
        public int Pdef;
        public int Mdef;
        public bool UnlockTrigger = false;
        public bool UnlockSkill = false;
        public bool UnlockNpc = false;

        public L2StaticObject(int objectId, CharTemplate template) : base(objectId, template)
        {
        }

        public override void BroadcastUserInfo()
        {
            foreach (L2Player obj in KnownObjects.Values.OfType<L2Player>())
                obj.SendPacket(new StaticObject(this));
        }

        public override void OnAction(L2Player player)
        {
            player.SendMessage(AsString());

            player.SetTarget(this);
        }

        public byte CanBeSelected()
        {
            return 1;
        }

        public byte ShowHp()
        {
            //TODO castle war
            return 0;
        }

        public virtual int GetDamage()
        {
            return 0;
        }

        public int Enemy()
        {
            return 0;
        }

        public void SetLoc(string[] p)
        {
            X = Convert.ToInt32(p[0]);
            Y = Convert.ToInt32(p[1]);
            Z = Convert.ToInt32(p[2]);
        }

        public void SetTex(string[] d)
        {
            TownMap = new ShowTownMap($"town_map.{d[0]}", Convert.ToInt32(d[1]), Convert.ToInt32(d[2]));
        }
    }
}