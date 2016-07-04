using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Skills2.Effects
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