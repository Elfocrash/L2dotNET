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
        public int Size => _size;

        private int _size;
        private readonly ConcurrentBag<StatFunction> _functions;

        public Calculator()
        {
            _functions = new ConcurrentBag<StatFunction>();
            _size = 0;
        }

        public static Calculator[] GetCalculatorsForStats()
        {
            Calculator[] calculators = new Calculator[Enum.GetNames(typeof(CharacterStatId)).Length];

            for (int i = 0; i < calculators.Length; i++)
            {
                calculators[i] = new Calculator();
            }

            return calculators;
        }

        public void AddFunc(StatFunction func)
        {
            Interlocked.Increment(ref _size);
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