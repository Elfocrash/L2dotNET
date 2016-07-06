using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Skills2.Effects
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