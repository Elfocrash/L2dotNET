using L2dotNET.model.player;

namespace L2dotNET.model.npcs.ai.ex
{
    class AiLooserOfGracia : AiTemplate
    {
        public override void OnDialog(L2Player player, int ask, int reply, L2Npc npc)
        {
            switch (ask)
            {
                case -1425:
                {
                    switch (reply)
                    {
                        case 1:
                            if (player.Level < 75)
                                player.ShowHtm(FnLowLevel, npc);
                            else
                            {
                                if (player.GetAdena() >= 150000)
                                    player.InstantTeleportWithItem(-149406, 255247, -85, 57, 150000);
                                else
                                    player.ShowHtm(FnNotHaveAdena, npc);
                            }
                            break;
                    }
                }

                    break;
            }
        }
    }
}