using System;
using System.Collections.Generic;
using L2dotNET.Game.model.npcs;
using L2dotNET.Game.model.npcs.decor;
using L2dotNET.Game.tables;
using L2dotNET.Game.world;

namespace L2dotNET.Game.model.structures
{
    public class HideoutTemplate
    {
        public int ID;
        public string Name, Descr;

        public SortedList<int, L2Citizen> npcs;
        internal void SetNpc(int id)
        {
            if (npcs == null)
                npcs = new SortedList<int, L2Citizen>();

            NpcTemplate t = NpcTable.getInstance().getNpcTemplate(id);
            L2Citizen npc = null;
            switch (t.NpcId)
            {
                case 35461:
                    npc = new L2HideoutManager(this);
                    break;
                case 35462:
                    npc = new L2Doormen(this);
                    break;
            }

            npc.setTemplate(t);

            StructureSpawn ss = StructureTable.getInstance().getSpawn(id);
            npc.X = ss.x;
            npc.Y = ss.y;
            npc.Z = ss.z;
            npc.Heading = ss.heading;

            npcs.Add(t.NpcId, npc);
        }

        public void SpawnNpcs()
        {
            if (npcs == null)
                return;

            foreach (L2Citizen npc in npcs.Values)
            {
                L2World.Instance.RealiseEntry(npc, null, true);
                npc.onSpawn();
            }
        }

        public List<L2Door> doors;
        internal void SetDoor(int id)
        {
            if (doors == null)
                doors = new List<L2Door>();

            L2Door door = StaticObjTable.getInstance().getDoor(id);
            door.structure = this;
            doors.Add(door);
        }

        public int[] ownerLoc;
        internal void SetOwnerRespawn(string[] p)
        {
            ownerLoc = new int[] { Convert.ToInt32(p[0]), Convert.ToInt32(p[1]), Convert.ToInt32(p[2]) };
        }

        public int[] outsideLoc;
        internal void SetOutsideRespawn(string[] p)
        {
            outsideLoc = new int[] { Convert.ToInt32(p[0]), Convert.ToInt32(p[1]), Convert.ToInt32(p[2]) };
        }

        public int[] banishLoc;
        internal void SetBanishRespawn(string[] p)
        {
            banishLoc = new int[] { Convert.ToInt32(p[0]), Convert.ToInt32(p[1]), Convert.ToInt32(p[2]) };
        }

        public List<int[]> zoneLoc;
        internal void SetZoneLoc(string[] p)
        {
            if (zoneLoc == null)
                zoneLoc = new List<int[]>();

            zoneLoc.Add(new int[] { Convert.ToInt32(p[0]), Convert.ToInt32(p[1]), Convert.ToInt32(p[2]), Convert.ToInt32(p[3]) });
        }

        public virtual void init() { }


    }
}
