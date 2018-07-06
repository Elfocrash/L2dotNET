using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Timers;

namespace L2dotNET.Models.Status
{
    public class CharStatus
    {
        public L2Character Character { get; set; }

        public List<L2Character> StatusListener { get; set; } = new List<L2Character>();

        protected static readonly byte RegenFlagCp = 4;
        private static readonly byte RegenFlagHp = 1;
        private static readonly byte RegenFlagMp = 2;
        public double CurrentHp { get => _currentHp; }
        public double CurrentMp { get => _currentMp; }
        private Timer _regTask;
        protected byte _flagsRegenActive = 0;
        private double _currentHp = 0;
        private double _currentMp = 0;

        public CharStatus(L2Character character)
        {
            Character = character;
        }

        public void AddStatusListener(L2Character character)
        {
            if (character == Character)
                return;

            StatusListener.Add(character);
        }

        public void RemoveStatusListener(L2Character character)
        {
            StatusListener.Remove(character);
        }

        public void ReduceHp(double value, L2Character attacker /*, bool awake, bool isDot, bool isHpConsumption*/)
        {
            if (Character.Dead)
                return;

            Console.WriteLine(attacker.ObjectId);
            if (value > 0)
            {
                SetCurrentHp(Math.Max(_currentHp - value, 0), true);
            }

            if (_currentHp < 0.5)
            {
                Character.AbortAttack();
                Character.DoDieAsync(attacker);
            }
        }

        public void ReduceMp(double value)
        {
            _currentMp = Math.Max(_currentMp - value, 0);
        }

        public void SetCurrentMp(double newMp)
        {
            SetCurrentMp(newMp, true);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetCurrentMp(double newMp, bool broadcastUpdate)
        {
            int maxMp = Character.MaxMp;

            if (Character.Dead)
                return;

            if (newMp >= maxMp)
            {
                _currentMp = maxMp;
                _flagsRegenActive &= RegenFlagMp;

                if (_flagsRegenActive == 0)
                    StopHpMpRegeneration();
            }
            else
            {
                _currentMp = newMp;
                _flagsRegenActive |= RegenFlagMp;

                StartHpMpRegeneration();
            }

            if (broadcastUpdate)
                Character.BroadcastStatusUpdateAsync();
        }

        public void SetCurrentHp(double newHp)
        {
            SetCurrentHp(newHp, true);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetCurrentHp(double newHp, bool broadcastUpdate)
        {
            double maxHp = Character.MaxHp;

            if (Character.Dead)
                return;

            if (newHp >= maxHp)
            {
                _currentHp = maxHp;
                _flagsRegenActive &= RegenFlagHp;

                if (_flagsRegenActive == 0)
                    StopHpMpRegeneration();
            }
            else
            {
                _currentHp = newHp;
                _flagsRegenActive |= RegenFlagHp;

                StartHpMpRegeneration();
            }

            if (broadcastUpdate)
                Character.BroadcastStatusUpdateAsync();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void StartHpMpRegeneration()
        {
            if (_regTask == null && !Character.Dead)
            {
                int period = 3000; //3 seconds for HP regen

                _regTask = new Timer(period);
                _regTask.Elapsed += RegenTask;
                _regTask.Enabled = true;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void StopHpMpRegeneration()
        {
            if (_regTask != null)
            {
                _regTask.Enabled = false;
                _flagsRegenActive = 0;
            }
        }

        private void RegenTask(object sender, ElapsedEventArgs e)
        {
            if (_currentHp < Character.MaxHp)
                SetCurrentHp(_currentHp + (Character.MaxHp * 1.0 / 100), false); // we will calculate the actual modified when we do formulas

            if (_currentMp < Character.MaxMp)
                SetCurrentMp(_currentMp + (Character.MaxMp * 1.0 / 100), false); // we will calculate the actual modified when we do formulas

            Character.BroadcastStatusUpdateAsync();
        }
    }
}