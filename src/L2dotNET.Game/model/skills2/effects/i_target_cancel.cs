using System;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    public class i_target_cancel : TEffect
    {
        public i_target_cancel()
        {
            type = TEffectType.i_target_cancel;
        }

        private int rate;

        public override void build(string str)
        {
            string[] v = str.Split(' ');
            rate = int.Parse(v[1]);
        }

        public override TEffectResult onStart(L2Character caster, L2Character target)
        {
            if (new Random().Next(100) < rate)
                target.ChangeTarget();

            return nothing;
        }
    }
}