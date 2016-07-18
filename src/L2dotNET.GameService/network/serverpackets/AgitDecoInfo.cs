using L2dotNET.GameService.Model.Npcs.Ai;
using L2dotNET.GameService.Model.Structures;
using L2dotNET.Network;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class AgitDecoInfo : GameserverPacket
    {
        private readonly Hideout _hideout;

        public AgitDecoInfo(Hideout hideout)
        {
            _hideout = hideout;
        }

        public override void Write()
        {
            WriteByte(0xf7);
            WriteInt(_hideout.ID); // clanhall id
            WriteByte(_hideout.GetFuncLevel(AgitManagerAi.DecotypeHpregen)); // FUNC_RESTORE_HP (Fireplace)
            WriteByte(_hideout.GetFuncLevel(AgitManagerAi.DecotypeMpregen)); // FUNC_RESTORE_MP (Carpet)
            WriteByte(_hideout.GetFuncLevel(AgitManagerAi.DecotypeCpregen)); // FUNC_RESTORE_MP (Statue)
            WriteByte(_hideout.GetFuncLevel(AgitManagerAi.DecotypeXprestore)); // FUNC_RESTORE_EXP (Chandelier)
            WriteByte(_hideout.GetFuncLevel(AgitManagerAi.DecotypeTeleport)); // FUNC_TELEPORT (Mirror)
            WriteByte(_hideout.GetFuncLevel(AgitManagerAi.DecotypeBroadcast)); // Crytal
            WriteByte(_hideout.GetFuncLevel(AgitManagerAi.DecotypeCurtain)); // Curtain
            WriteByte(_hideout.GetFuncLevel(AgitManagerAi.DecotypeHanging)); // FUNC_ITEM_CREATE (Magic Curtain)
            WriteByte(_hideout.GetFuncLevel(AgitManagerAi.DecotypeBuff)); // FUNC_SUPPORT
            WriteByte(_hideout.GetFuncLevel(AgitManagerAi.DecotypeOuterflag)); // FUNC_SUPPORT (Flag)
            WriteByte(_hideout.GetFuncLevel(AgitManagerAi.DecotypePlatform)); // Front Platform
            WriteByte(_hideout.GetFuncLevel(AgitManagerAi.DecotypeItem)); // FUNC_ITEM_CREATE
            WriteInt(0);
            WriteInt(0);
        }
    }
}