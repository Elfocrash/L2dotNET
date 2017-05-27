using L2dotNET.Network.serverpackets;

namespace L2dotNET.model.skills2.effects
{
    class PMagicalDefence : Effect
    {
        public PMagicalDefence()
        {
            SuId = StatusUpdate.MDef;
            Type = EffectType.PMagicalDefense;
        }

        public override void Build(string str)
        {
            string[] v = str.Split(' ');
            SetCondition(v[1]);
            SetSup(v[2]);
        }
    }
}