using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using L2dotNET.DataContracts.Shared.Enums;

namespace L2dotNET.Models.Stats
{
    public class Calculator
    {
        public static Calculator[] GetCalculatorsForStats() => new Calculator[Enum.GetNames(typeof(CharacterStatId)).Length];

        public int Size { get; private set; }

        private readonly List<StatFunction> _functions;

        public Calculator()
        {
            _functions = new List<StatFunction>();
            Size = 0;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddFunc(StatFunction func)
        {
            Size++;
            _functions.Add(func);
        }

        public void Calculate(StatFunctionEnvironment statFuncEnv)
        {
            foreach (StatFunction function in _functions.OrderBy(x => x.Order))
            {
                function.Calculate(statFuncEnv);
            }
        }
    }
}