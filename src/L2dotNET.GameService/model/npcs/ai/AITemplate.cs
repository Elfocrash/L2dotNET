using System.Collections.Generic;
using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Model.Npcs.Ai
{
    public class AITemplate
    {
        public SortedList<string, string> parameters = new SortedList<string, string>();
        public bool chatOvr = false,
                    dialogOvr = false,
                    teleportOvr = false;
        public int id;
        public string ainame = "default01";

        public virtual void onShowChat(L2Player player, L2Npc npc) { }

        public virtual void onDialog(L2Player player, int ask, int reply, L2Npc npc) { }

        public virtual void onTeleportRequest(L2Player player, int ask, int reply, L2Npc npc) { }

        public virtual void onActionClicked(L2Player player, L2Summon pet, int actionId) { }

        public int getValueInt(string p)
        {
            if (parameters.ContainsKey(p))
                return int.Parse(parameters[p]);

            return -1;
        }

        public int[] getValueSkill(string p)
        {
            if (parameters.ContainsKey(p))
            {
                string[] str = parameters[p].Split('-');
                return new[] { int.Parse(str[0]), int.Parse(str[1]) }; //;
            }

            return null;
        }

        public string getValueString(string p)
        {
            return parameters.ContainsKey(p) ? parameters[p] : null;
        }

        public string fnLowLevel;
        public string fnNotHaveAdena;
        public string fnLowLevel2;
        public string fnLowLeve3l;
    }
}