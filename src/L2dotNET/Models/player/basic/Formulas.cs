using System;

namespace L2dotNET.Models.Player.Basic
{
    public class Formulas
    {
        public static readonly int MaxStatValue = 100;

        public static readonly double[] StrBonus = new double[MaxStatValue];
        public static readonly double[] IntBonus = new double[MaxStatValue];
        public static readonly double[] DexBonus = new double[MaxStatValue];
        public static readonly double[] ConBonus = new double[MaxStatValue];
        public static readonly double[] WitBonus = new double[MaxStatValue];
        public static readonly double[] MenBonus = new double[MaxStatValue];
        public static readonly double[] BaseEvasionAccuracy = new double[MaxStatValue];

        public static readonly double[] SqrtMenBonus = new double[MaxStatValue];
        public static readonly double[] SqrtConBonus = new double[MaxStatValue];

        private static readonly double[] StrCompute = {
            1.036,
            34.845
        };
        private static readonly double[] IntCompute = {
            1.020,
            31.375
        };
        private static readonly double[] DexCompute = {
            1.009,
            19.360
        };
        private static readonly double[] WitCompute = {
            1.050,
            20.000
        };
        private static readonly double[] ConCompute = {
            1.030,
            27.632
        };
        private static readonly double[] MenCompute = {
            1.010,
            -0.060
        };

        static Formulas()
        {
            for (int i = 0; i < StrBonus.Length; i++)
                StrBonus[i] = Math.Floor(Math.Pow(StrCompute[0], i - StrCompute[1]) * 100 + .5d) / 100;
            for (int i = 0; i < IntBonus.Length; i++)
                IntBonus[i] = Math.Floor(Math.Pow(IntCompute[0], i - IntCompute[1]) * 100 + .5d) / 100;
            for (int i = 0; i < DexBonus.Length; i++)
                DexBonus[i] = Math.Floor(Math.Pow(DexCompute[0], i - DexCompute[1]) * 100 + .5d) / 100;
            for (int i = 0; i < WitBonus.Length; i++)
                WitBonus[i] = Math.Floor(Math.Pow(WitCompute[0], i - WitCompute[1]) * 100 + .5d) / 100;
            for (int i = 0; i < ConBonus.Length; i++)
                ConBonus[i] = Math.Floor(Math.Pow(ConCompute[0], i - ConCompute[1]) * 100 + .5d) / 100;
            for (int i = 0; i < MenBonus.Length; i++)
                MenBonus[i] = Math.Floor(Math.Pow(MenCompute[0], i - MenCompute[1]) * 100 + .5d) / 100;

            for (int i = 0; i < BaseEvasionAccuracy.Length; i++)
                BaseEvasionAccuracy[i] = Math.Sqrt(i) * 6;

            // Precompute square root values
            for (int i = 0; i < SqrtConBonus.Length; i++)
                SqrtConBonus[i] = Math.Sqrt(ConBonus[i]);
            for (int i = 0; i < SqrtMenBonus.Length; i++)
                SqrtMenBonus[i] = Math.Sqrt(MenBonus[i]);
        }
    }
}