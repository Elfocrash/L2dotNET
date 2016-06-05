using L2dotNET.GameService.model.templates;
using L2dotNET.GameService.tables;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.model.npcs
{
    public class NpcTemplate : TObject
    {
        internal void setSkills(string[] p) { }

        public string Name;
        public string Title;
        public string JClass;
        public int RunSpeed = 130;
        public int WalkSpeed = 88;
        public string[] FactionID;
        public int FactionRange;
        public int SShotRate;
        public DropContainer DropData;
        public double base_magic_speed = 333;
        public int slot_rhand_id;
        public int slot_lhand_id;

        public void roll_drops(L2Citizen npc, L2Character killer)
        {
            if (DropData != null)
            {
                if (DropData.multidrop.Count > 0)
                    DropData.roll_multidrop(npc, killer);
                if (DropData.multidrop_ex.Count > 0)
                    DropData.roll_multidrop_ex(npc, killer);
                if (DropData.qdrop.Count > 0)
                    DropData.roll_qdrop(npc, killer);
            }
        }

        public void roll_spoil(L2Player caster, L2Citizen npc)
        {
            if (DropData != null)
            {
                if (DropData.spoil.Count > 0)
                    DropData.roll_spoil(caster, npc);
            }
        }
    }
}