using L2dotNET.GameService.Network.Serverpackets;

namespace L2dotNET.GameService.Model.Skills2.Effects
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