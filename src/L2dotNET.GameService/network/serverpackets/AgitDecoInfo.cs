using L2dotNET.GameService.Model.Npcs.Ai;
using L2dotNET.GameService.Model.Structures;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class AgitDecoInfo : GameServerNetworkPacket
    {
        private readonly Hideout _hideout;

        public AgitDecoInfo(Hideout hideout)
        {
            this._hideout = hideout;
        }

        protected internal override void Write()
        {
            WriteC(0xf7);
            WriteD(_hideout.ID); // clanhall id
            WriteC(_hideout.GetFuncLevel(AgitManagerAi.DecotypeHpregen)); // FUNC_RESTORE_HP (Fireplace)
            WriteC(_hideout.GetFuncLevel(AgitManagerAi.DecotypeMpregen)); // FUNC_RESTORE_MP (Carpet)
            WriteC(_hideout.GetFuncLevel(AgitManagerAi.DecotypeCpregen)); // FUNC_RESTORE_MP (Statue)
            WriteC(_hideout.GetFuncLevel(AgitManagerAi.DecotypeXprestore)); // FUNC_RESTORE_EXP (Chandelier)
            WriteC(_hideout.GetFuncLevel(AgitManagerAi.DecotypeTeleport)); // FUNC_TELEPORT (Mirror)
            WriteC(_hideout.GetFuncLevel(AgitManagerAi.DecotypeBroadcast)); // Crytal
            WriteC(_hideout.GetFuncLevel(AgitManagerAi.DecotypeCurtain)); // Curtain
            WriteC(_hideout.GetFuncLevel(AgitManagerAi.DecotypeHanging)); // FUNC_ITEM_CREATE (Magic Curtain)
            WriteC(_hideout.GetFuncLevel(AgitManagerAi.DecotypeBuff)); // FUNC_SUPPORT
            WriteC(_hideout.GetFuncLevel(AgitManagerAi.DecotypeOuterflag)); // FUNC_SUPPORT (Flag)
            WriteC(_hideout.GetFuncLevel(AgitManagerAi.DecotypePlatform)); // Front Platform
            WriteC(_hideout.GetFuncLevel(AgitManagerAi.DecotypeItem)); // FUNC_ITEM_CREATE
            WriteD(0);
            WriteD(0);
        }
    }
}