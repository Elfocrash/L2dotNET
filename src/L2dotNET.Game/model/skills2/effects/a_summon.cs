using L2dotNET.Game.model.playable;
using L2dotNET.Game.tables;

namespace L2dotNET.Game.model.skills2.effects
{
    class a_summon : TEffect
    {
        private int npcId;
        public override void build(string str)
        {
            this.npcId = int.Parse(str.Split(' ')[1]);
        }

        public override TEffectResult onStart(world.L2Character caster, world.L2Character target)
        {
            L2Summon summon = new L2Summon();
            summon.setTemplate(NpcTable.Instance.getNpcTemplate(npcId));
            summon.setOwner((L2Player)caster);
            summon.SpawmMe();

            return nothing;
        }

        public override bool canUse(world.L2Character caster)
        {
            L2Player player = (L2Player)caster;
            if (player.Summon != null)
            {
                player.sendSystemMessage(543);//You already have a pet.
                return false;
            }

            return true;
        }
    }
}
