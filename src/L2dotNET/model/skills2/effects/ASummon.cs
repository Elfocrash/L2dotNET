using L2dotNET.model.playable;
using L2dotNET.model.player;
using L2dotNET.Network.serverpackets;
using L2dotNET.world;

namespace L2dotNET.model.skills2.effects
{
    class ASummon : Effect
    {
        private int _npcId;

        public override void Build(string str)
        {
            _npcId = int.Parse(str.Split(' ')[1]);
        }

        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            //L2Summon summon = new L2Summon();
            ////summon.setTemplate(NpcTable.Instance.GetNpcTemplate(npcId));
            //summon.SetOwner((L2Player)caster);
            //summon.SpawmMe();

            return Nothing;
        }

        public override bool CanUse(L2Character caster)
        {
            L2Player player = (L2Player)caster;
            if (player.Summon == null)
                return true;

            player.SendSystemMessage(SystemMessage.SystemMessageId.YouAlreadyHaveAPet);
            return false;
        }
    }
}