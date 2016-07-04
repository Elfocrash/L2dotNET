using System;
using System.Linq;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Npcs.Decor
{
    public class L2StaticObject : L2Character
    {
        /// <summary>
        /// EL2_DOOR (1), EL2_AIRSHIPKEY (3)
        /// </summary>
        public int StaticID;
        public int ClanID = -1;
        public int MeshID = 0;
        public int StructureId = 0;
        public int Type = 0;
        public byte Closed = 1;
        public ShowTownMap townMap;
        public string htm;
        public int pdef;
        public int mdef;
        public bool UnlockTrigger = false;
        public bool UnlockSkill = false;
        public bool UnlockNpc = false;

        public override void BroadcastUserInfo()
        {
            foreach (L2Player obj in KnownObjects.Values.OfType<L2Player>())
                obj.SendPacket(new StaticObject(this));
        }

        public override void OnAction(L2Player player)
        {
            player.SendMessage(AsString());

            player.ChangeTarget(this);
        }

        public byte CanBeSelected()
        {
            return 1;
        }

        public byte ShowHP()
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

        public void setLoc(string[] p)
        {
            X = Convert.ToInt32(p[0]);
            Y = Convert.ToInt32(p[1]);
            Z = Convert.ToInt32(p[2]);
        }

        public void setTex(string[] d)
        {
            townMap = new ShowTownMap("town_map." + d[0], Convert.ToInt32(d[1]), Convert.ToInt32(d[2]));
        }
    }
}