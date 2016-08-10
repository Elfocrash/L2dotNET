using L2dotNET.Network.serverpackets;

namespace L2dotNET.model.skills2.effects
{
    class PPhysicalAttack : Effect
    {
        public PPhysicalAttack()
        {
            SuId = StatusUpdate.PAtk;
            Type = EffectType.PPhysicalAttack;
        }

        public override void Build(string str)
        {
            string[] v = str.Split(' ');
            SetCondition(v[1]);
            SetSup(v[2]);
        }
    }
}