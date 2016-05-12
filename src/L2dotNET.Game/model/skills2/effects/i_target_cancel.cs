using System;

namespace L2dotNET.GameService.model.skills2.effects
{
    public class i_target_cancel : TEffect
    {
        public i_target_cancel()
        {
            type = TEffectType.i_target_cancel;
        }

        int rate;
        public override void build(string str)
        {
            string[] v = str.Split(' ');
            rate = int.Parse(v[1]);
        }

        public override TEffectResult onStart(world.L2Character caster, world.L2Character target)
        {
            if (new Random().Next(100) < rate)
                target.ChangeTarget();

            return nothing;
        }
    }
}
