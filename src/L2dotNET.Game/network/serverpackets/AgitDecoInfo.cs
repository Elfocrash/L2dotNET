using L2dotNET.GameService.Model.Npcs.Ai;
using L2dotNET.GameService.Model.Structures;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class AgitDecoInfo : GameServerNetworkPacket
    {
        private readonly Hideout hideout;

        public AgitDecoInfo(Hideout hideout)
        {
            this.hideout = hideout;
        }

        protected internal override void write()
        {
            writeC(0xf7);
            writeD(hideout.ID); // clanhall id
            writeC(hideout.GetFuncLevel(AgitManagerAI.decotype_hpregen)); // FUNC_RESTORE_HP (Fireplace)
            writeC(hideout.GetFuncLevel(AgitManagerAI.decotype_mpregen)); // FUNC_RESTORE_MP (Carpet)
            writeC(hideout.GetFuncLevel(AgitManagerAI.decotype_cpregen)); // FUNC_RESTORE_MP (Statue)
            writeC(hideout.GetFuncLevel(AgitManagerAI.decotype_xprestore)); // FUNC_RESTORE_EXP (Chandelier)
            writeC(hideout.GetFuncLevel(AgitManagerAI.decotype_teleport)); // FUNC_TELEPORT (Mirror)
            writeC(hideout.GetFuncLevel(AgitManagerAI.decotype_broadcast)); // Crytal
            writeC(hideout.GetFuncLevel(AgitManagerAI.decotype_curtain)); // Curtain
            writeC(hideout.GetFuncLevel(AgitManagerAI.decotype_hanging)); // FUNC_ITEM_CREATE (Magic Curtain)
            writeC(hideout.GetFuncLevel(AgitManagerAI.decotype_buff)); // FUNC_SUPPORT
            writeC(hideout.GetFuncLevel(AgitManagerAI.decotype_outerflag)); // FUNC_SUPPORT (Flag)
            writeC(hideout.GetFuncLevel(AgitManagerAI.decotype_platform)); // Front Platform
            writeC(hideout.GetFuncLevel(AgitManagerAI.decotype_item)); // FUNC_ITEM_CREATE
            writeD(0);
            writeD(0);
        }
    }
}