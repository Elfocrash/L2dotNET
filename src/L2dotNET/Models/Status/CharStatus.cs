using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Timers;
using L2dotNET.model.player;
using L2dotNET.world;

namespace L2dotNET.Models.Status
{
    public class CharStatus
    {
        public L2Character Character { get; set; }

        public List<L2Character> StatusListener { get; set; } = new List<L2Character>();

        protected static readonly byte RegenFlagCp = 4;
        private static readonly byte RegenFlagHp = 1;
        private static readonly byte RegenFlagMp = 2;
        public double CurrentHp { get; set; } = 0;
        public double CurrentMp { get; set; } = 0;

        private Timer _regTask;
        protected byte _flagsRegenActive = 0;

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

            if(value > 0)
                SetCurrentHp(Math.Max(CurrentHp - value, 0));

            if(Character.CurHp < 0.5)
            {
                Character.AbortAttack();
                Character.DoDie(attacker);
            }
        }

        public void ReduceMp(double value)
        {
            Character.CurMp = Math.Max(Character.CurMp - value, 0);
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
                Character.CurMp = maxMp;
                _flagsRegenActive &= RegenFlagMp;

                if (_flagsRegenActive == 0)
                    StopHpMpRegeneration();
            }
            else
            {
                Character.CurMp = newMp;
                _flagsRegenActive |= RegenFlagMp;

                StartHpMpRegeneration();
            }

            if(broadcastUpdate)
                Character.BroadcastStatusUpdate();
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
                Character.CurHp = maxHp;
                _flagsRegenActive &= RegenFlagHp;

                if (_flagsRegenActive == 0)
                    StopHpMpRegeneration();
            }
            else
            {
                Character.CurHp = newHp;
                _flagsRegenActive |= RegenFlagHp;

                StartHpMpRegeneration();
            }

            if(broadcastUpdate)
                Character.BroadcastStatusUpdate();
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
            if (Character.CurHp < Character.MaxHp)
                SetCurrentHp(Character.CurHp + (Character.MaxHp * 1.0 / 100), false); // we will calculate the actual modified when we do formulas

            if (Character.CurMp < Character.MaxMp)
                SetCurrentMp(Character.CurMp + (Character.MaxMp * 1.0 / 100), false); // we will calculate the actual modified when we do formulas

            Character.BroadcastStatusUpdate();
        }
    }
}