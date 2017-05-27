using L2dotNET.Network.serverpackets;

namespace L2dotNET.model.skills2.effects
{
    class PPhysicalDefence : Effect
    {
        public PPhysicalDefence()
        {
            SuId = StatusUpdate.PDef;
            Type = EffectType.PPhysicalDefense;
        }

        public override void Build(string str)
        {
            string[] v = str.Split(' ');
            SetCondition(v[1]);
            SetSup(v[2]);
        }
    }
}