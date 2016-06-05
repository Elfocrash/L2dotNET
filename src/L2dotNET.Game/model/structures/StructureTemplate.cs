using System;
using System.Collections.Generic;
using L2dotNET.GameService.model.npcs;
using L2dotNET.GameService.model.npcs.decor;
using L2dotNET.GameService.tables;
using L2dotNET.GameService.world;
using L2dotNET.GameService.Templates;

namespace L2dotNET.GameService.model.structures
{
    public class HideoutTemplate
    {
        public int ID;
        public string Name, Descr;

        public SortedList<int, L2Npc> npcs;
        internal void SetNpc(int id)
        {
            if (npcs == null)
                npcs = new SortedList<int, L2Npc>();

            NpcTemplate t = new NpcTemplate(new GameService.templates.StatsSet());//NpcTable.Instance.GetNpcTemplate(id);
            L2Npc npc = null;
            switch (t.NpcId)
            {
                case 35461:
                    npc = new L2HideoutManager(this);
                    break;
                case 35462:
                    npc = new L2Doormen(this);
                    break;
            }

            //npc.setTemplate(t);
            if(npc != null)
            {
                StructureSpawn ss = StructureTable.Instance.GetSpawn(id);
                npc.X = ss.x;
                npc.Y = ss.y;
                npc.Z = ss.z;
                npc.Heading = ss.heading;

                npcs.Add(t.NpcId, npc);
            }
            
        }

        public void SpawnNpcs()
        {
            if (npcs == null)
                return;

            foreach (L2Npc npc in npcs.Values)
            {
                L2World.Instance.AddObject(npc);
                npc.onSpawn();
            }
        }

        public List<L2Door> doors;
        internal void SetDoor(int id)
        {
            if (doors == null)
                doors = new List<L2Door>();

            L2Door door = StaticObjTable.Instance.GetDoor(id);
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
