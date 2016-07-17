using System;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2
{
    public class Effect
    {
        public virtual EffectResult OnStart(L2Character caster, L2Character target)
        {
            double[] val = target.CharacterStat.Apply(this);

            if ((SuId == -1) || (val[0] == val[1]))
                return Nothing;

            EffectResult ter = new EffectResult();
            ter.AddSu(SuId, val[1]);
            return ter;
        }

        public virtual EffectResult OnEnd(L2Character caster, L2Character target)
        {
            double[] val = target.CharacterStat.Stop(this);

            if ((SuId == -1) || (val[0] == val[1]))
                return Nothing;

            EffectResult ter = new EffectResult();
            ter.AddSu(SuId, val[1]);
            return ter;
        }

        public virtual bool CanUse(L2Character caster)
        {
            return true;
        }

        public virtual void Build(string str)
        {
            // string[] v = str.Split(' ');
            //  SetCondition(v[1]);
            // SetSup(v[2]);
        }

        public virtual void SetCondition(string str)
        {
            _conditionStr = str;
        }

        public void SetSup(string str)
        {
            SupMethod = new SupMethod();
            switch (str.ToCharArray()[0])
            {
                case '*':
                    SupMethod.Method = SupMethod.Mul;
                    break;
                case '+':
                    SupMethod.Method = SupMethod.Add;
                    break;
                case '-':
                    SupMethod.Method = SupMethod.Sub;
                    break;
                case '/':
                    SupMethod.Method = SupMethod.Div;
                    break;
                case '=':
                    SupMethod.Method = SupMethod.Ovr;
                    break;
            }

            SupMethod.Value = Convert.ToDouble(str.Substring(1));
        }

        public EffectResult Nothing = new EffectResult();

        public SupMethod SupMethod;
        public EffectType Type;
        public long HashId;
        public byte Order;
        public int SuId;
        public int SkillId;
        public int SkillLv;
        private string _conditionStr;
    }
}