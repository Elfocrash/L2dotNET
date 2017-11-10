using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace L2dotNET.Models.Stats
{
    public class Calculator
    {
        private static readonly Func[] EmptyFuncs = new Func[0];

        private Func[] _functions;

        public Calculator()
        {
            _functions = EmptyFuncs;
        }

        public Calculator(Calculator calculator)
        {
            _functions = calculator._functions;
        }

        public int Size => _functions.Length;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddFunc(Func func)
        {
            var funcs = _functions;
            var tmp = new Func[funcs.Length + 1];

            var order = func.Order;
            int i;
            for (i = 0; i < funcs.Length && order >= funcs[i].Order; i++)
                tmp[i] = funcs[i];

            tmp[i] = func;

            for (; i < funcs.Length; i++)
                tmp[i + 1] = funcs[i];

            _functions = tmp;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveFunc(Func func)
        {
            var funcs = _functions;
            var tmp = new Func[funcs.Length - 1];
            
            int i;
            for (i = 0; i < funcs.Length && func != funcs[i]; i++)
                tmp[i] = funcs[i];

            if (i == funcs.Length)
                return;

            for (; i < funcs.Length; i++)
                tmp[i - 1] = funcs[i];

            _functions = tmp.Length == 0 ? EmptyFuncs : tmp;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public List<Stat> RemoveOwner(object owner)
        {
            List<Stat> modifiedStats = new List<Stat>();
            foreach (var function in _functions)
            {
                if (function.FuncOwner == owner)
                {
                    modifiedStats.Add(function.Stat);
                    RemoveFunc(function);
                }
            }
            return modifiedStats;
        }

        public void Calculate(Env env)
        {
            foreach (var function in _functions)
            {
                function.Calculate(env);
            }
        }
    }
}