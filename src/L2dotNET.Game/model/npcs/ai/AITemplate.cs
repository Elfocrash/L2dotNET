using L2dotNET.GameService.model.playable;
using System.Collections.Generic;

namespace L2dotNET.GameService.model.npcs.ai
{
    public class AITemplate
    {
        public SortedList<string, string> parameters = new SortedList<string, string>();
        public bool chatOvr = false, dialogOvr = false, teleportOvr = false;
        public int id;
        public string ainame = "default01";
        public virtual void onShowChat(L2Player player, L2Citizen npc) { }
        public virtual void onDialog(L2Player player, int ask, int reply, L2Citizen npc) { }
        public virtual void onTeleportRequest(L2Player player, int ask, int reply, L2Citizen npc) { }
        public virtual void onActionClicked(L2Player player, L2Summon pet, int id) { }







        public int getValueInt(string p)
        {
            if (parameters.ContainsKey(p))
                return int.Parse(parameters[p]);
            else
                return -1;
        }

        public int[] getValueSkill(string p)
        {
            if (parameters.ContainsKey(p))
            {
                string[] str = parameters[p].Split('-');
                return new int[] { int.Parse(str[0]), int.Parse(str[1]) };//;
            }
            else
                return null;
        }

        public string getValueString(string p)
        {
            if (parameters.ContainsKey(p))
                return parameters[p];
            else
                return null;
        }


        public string fnLowLevel;
        public string fnNotHaveAdena;
        public string fnLowLevel2;
        public string fnLowLeve3l;
    }
}
