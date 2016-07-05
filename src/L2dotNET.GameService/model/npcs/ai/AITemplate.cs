using System.Collections.Generic;
using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Npcs.Ai
{
    public class AiTemplate
    {
        public SortedList<string, string> Parameters = new SortedList<string, string>();
        public bool ChatOvr = false,
                    DialogOvr = false,
                    TeleportOvr = false;
        public int Id;
        public string Ainame = "default01";

        public virtual void OnShowChat(L2Player player, L2Npc npc) { }

        public virtual void OnDialog(L2Player player, int ask, int reply, L2Npc npc) { }

        public virtual void OnTeleportRequest(L2Player player, int ask, int reply, L2Npc npc) { }

        public virtual void OnActionClicked(L2Player player, L2Summon pet, int actionId) { }

        public int GetValueInt(string p)
        {
            if (Parameters.ContainsKey(p))
            {
                return int.Parse(Parameters[p]);
            }

            return -1;
        }

        public int[] GetValueSkill(string p)
        {
            if (Parameters.ContainsKey(p))
            {
                string[] str = Parameters[p].Split('-');
                return new[] { int.Parse(str[0]), int.Parse(str[1]) }; //;
            }

            return null;
        }

        public string GetValueString(string p)
        {
            return Parameters.ContainsKey(p) ? Parameters[p] : null;
        }

        public string FnLowLevel;
        public string FnNotHaveAdena;
        public string FnLowLevel2;
        public string FnLowLeve3L;
    }
}