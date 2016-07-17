using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Skills2.SpecEffects;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2.Effects
{
    class BEvasionMove : Effect
    {
        private SpecEffect _ef;

        public override void Build(string str)
        {
            _ef = new BEvasionByMove(double.Parse(str.Split(' ')[1]));
        }

        public override EffectResult OnStart(L2Character caster, L2Character target)
        {
            if (!(target is L2Player))
                return Nothing;

            ((L2Player)target).SpecEffects.Add(_ef);

            EffectResult ter = new EffectResult();
            ter.AddSu(StatusUpdate.Evasion, target.CharacterStat.GetStat(EffectType.BEvasion));
            return Nothing;
        }

        public override EffectResult OnEnd(L2Character caster, L2Character target)
        {
            if (!(target is L2Player))
                return Nothing;

            lock (((L2Player)target).SpecEffects)
                ((L2Player)target).SpecEffects.Remove(_ef);

            EffectResult ter = new EffectResult();
            ter.AddSu(StatusUpdate.Evasion, target.CharacterStat.GetStat(EffectType.BEvasion));
            return Nothing;
        }
    }
}