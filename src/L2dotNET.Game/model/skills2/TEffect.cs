using System;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.model.skills2
{
    public class TEffect
    {
        public virtual TEffectResult onStart(L2Character caster, L2Character target) 
        {
            double[] val = target.CharacterStat.Apply(this);

            if (SU_ID != -1 && val[0] != val[1])
            {
                TEffectResult ter = new TEffectResult();
                ter.addSU(SU_ID, val[1]);
                return ter;
            }

            return nothing;
        }

        public virtual TEffectResult onEnd(L2Character caster, L2Character target) 
        {
            double[] val = target.CharacterStat.Stop(this);

            if (SU_ID != -1 && val[0] != val[1])
            {
                TEffectResult ter = new TEffectResult();
                ter.addSU(SU_ID, val[1]);
                return ter;
            }

            return nothing;
        }

        public virtual bool canUse(L2Character caster)
        {
            return true;
        }

        public virtual void build(string str)
        {
           // string[] v = str.Split(' ');
          //  SetCondition(v[1]);
           // SetSup(v[2]);
        }

        public virtual void SetCondition(string str)
        {
            ConditionStr = str;
        }

        public void SetSup(string str)
        {
            supMethod = new SupMethod();
            switch (str.ToCharArray()[0])
            {
                case '*': supMethod.Method = SupMethod.MUL; break;
                case '+': supMethod.Method = SupMethod.ADD; break;
                case '-': supMethod.Method = SupMethod.SUB; break;
                case '/': supMethod.Method = SupMethod.DIV; break;
                case '=': supMethod.Method = SupMethod.OVR; break;
            }

            supMethod.Value = Convert.ToDouble(str.Substring(1));
        }

        public TEffectResult nothing = new TEffectResult();

        public SupMethod supMethod;
        public TEffectType type;
        public long HashID;
        public byte Order;
        public int SU_ID;
        public int SkillId;
        public int SkillLv;
        private string ConditionStr;
    }
}
