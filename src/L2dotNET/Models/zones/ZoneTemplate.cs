using System.Collections.Generic;
using System.Linq;
using L2dotNET.Models.Zones.forms;

namespace L2dotNET.Models.Zones
{
    public class ZoneTemplate
    {
        public string MapNo;
        public ZoneType Type;
        public ZoneForm Territory;
        public ZoneTarget Target = ZoneTarget.All;
        public string AffectRace = "all";
        public int EnteringMessageNo;
        public int LeavingMessageNo;
        public int MoveBonus = 0;
        public bool DefaultStatus = true;
        public int EventId;
        public int DamageOnHp = 0;
        public int DamageOnMp = 0;
        public int MessageNo;

        public int SkillProb;
        public int UnitTick = 9;
        public int InitialDelay = 1;
        public string Name;
        public int HpRegenBonus;
        public int MpRegenBonus;
        public int[] X;
        public int[] Y;
        public int Z1,
                   Z2;
        public int ExpPenaltyPer;
        public bool ItemDrop;

        public enum ZoneTarget
        {
            Npc,
            Pc,
            All,
            OnlyPc
        }

        public enum ZoneType
        {
            MotherTree,
            PeaceZone,
            BattleZone,
            Poison,
            Water,
            NoRestart,
            SsqZone,
            Swamp,
            Damage,
            InstantSkill,
            InstantBuff,

            Hideout,
            MonsterRace
        }
        
        public void SetRange(string val)
        {
            string d1 = val.Substring(2).Replace("};{", "\f").Replace("}}", string.Empty);
            int s = d1.Split('\f').Length;
            X = new int[s];
            Y = new int[s];
            int y = 0;
            foreach (string[] xyz in d1.Split('\f').Select(loc => loc.Split(';')))
            {
                X[y] = int.Parse(xyz[0]);
                Y[y] = int.Parse(xyz[1]);
                Z1 = int.Parse(xyz[2]);
                Z2 = int.Parse(xyz[3]);
                y++;
            }
        }

        public void SetRange(List<int[]> zoneLoc)
        {
            X = new int[zoneLoc.Count];
            Y = new int[zoneLoc.Count];
            int y = 0;

            foreach (int[] l in zoneLoc)
            {
                X[y] = l[0];
                Y[y] = l[1];
                Z1 = l[2];
                Z2 = l[3];
                y++;
            }
        }
    }
}