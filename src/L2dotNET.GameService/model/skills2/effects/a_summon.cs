using L2dotNET.GameService.Model.Playable;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class a_summon : TEffect
    {
        private int npcId;

        public override void build(string str)
        {
            npcId = int.Parse(str.Split(' ')[1]);
        }

        public override TEffectResult onStart(L2Character caster, L2Character target)
        {
            L2Summon summon = new L2Summon();
            //summon.setTemplate(NpcTable.Instance.GetNpcTemplate(npcId));
            summon.setOwner((L2Player)caster);
            summon.SpawmMe();

            return nothing;
        }

        public override bool canUse(L2Character caster)
        {
            L2Player player = (L2Player)caster;
            if (player.Summon != null)
            {
                player.sendSystemMessage(SystemMessage.SystemMessageId.YOU_ALREADY_HAVE_A_PET);
                return false;
            }

            return true;
        }
    }
}