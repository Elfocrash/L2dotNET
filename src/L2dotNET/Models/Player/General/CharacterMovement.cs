using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using L2dotNET.Network.serverpackets;
using L2dotNET.Utility;

namespace L2dotNET.Models.Player.General
{
    public class CharacterMovement
    {
        public int X
        {
            get
            {
                PerformMove();
                return _x;
            }
            set
            {
                if (IsMoving)
                {
                    NotifyStopMove();
                }

                _x = value;
            }
        }

        public int Y
        {
            get
            {
                PerformMove();
                return _y;
            }
            set
            {
                if (IsMoving)
                {
                    NotifyStopMove();
                }

                _y = value;
            }
        }

        public int Z { get; set; }

        public int Heading { get; private set; }
        public bool IsMoving { get; private set; }
        public int DestinationX { get; private set; }
        public int DestinationY { get; private set; }
        public int DestinationZ { get; private set; }

        private readonly L2Character _character;
        private long _movementLastTime;
        private long _movementUpdateTime;
        private L2Character _attackTarget;
        private int _x;
        private int _y;

        public CharacterMovement(L2Character character)
        {
            _character = character;
            _attackTarget = null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void PerformMove(bool forceUpdate = false)
        {
            //await ValidateWaterZones();

            if (!IsMoving)
            {
                return;
            }

            if (!CanMove())
            {
                NotifyStopMove();
                return;
            }

            long currentTime = DateTime.UtcNow.Ticks;
            float elapsedSeconds = (currentTime - _movementLastTime) / (float) TimeSpan.TicksPerSecond;

            // TODO: move to config
            if (!forceUpdate && elapsedSeconds < 0.05f) // 50 ms, skip run if last run was less then 50ms ago
            {
                return;
            }

            if (_attackTarget != null) // if we have target then update destination coordinates
            {
                DestinationX = _attackTarget.X;
                DestinationY = _attackTarget.Y;
                DestinationZ = _attackTarget.Z;
            }

            float distance = (float) Utilz.Length(DestinationX - _x, DestinationY - _y);

            // vector to destination with length = 1
            float vectorX = (DestinationX - _x) / distance;
            float vectorY = (DestinationY - _y) / distance;

            int dx = (int)(vectorX * _character.CharacterStat.MoveSpeed * elapsedSeconds);
            int dy = (int)(vectorY * _character.CharacterStat.MoveSpeed * elapsedSeconds);
            double ddistance = Utilz.Length(dx, dy);

            Heading = (int) (Math.Atan2(-vectorX, -vectorY) * 10430.378 + short.MaxValue);

            if (ddistance >= distance || distance < 1)
            {
                _x = DestinationX;
                _y = DestinationY;

                NotifyArrived();
                return;
            }

            _movementLastTime = currentTime;
            _x += (int) (vectorX * _character.CharacterStat.MoveSpeed * elapsedSeconds);
            _y += (int) (vectorY * _character.CharacterStat.MoveSpeed * elapsedSeconds);
        }

        public double DistanceTo(int x, int y)
        {
            return Math.Sqrt(Math.Pow(x - X, 2) + Math.Pow(y - Y, 2));
        }

        public double DistanceToSquared(int x, int y)
        {
            return Math.Pow(x - X, 2) + Math.Pow(y - Y, 2);
        }

        public double DistanceTo(L2Object obj)
        {
            return DistanceTo(obj.X, obj.Y);
        }

        public double DistanceToSquared(L2Object obj)
        {
            return DistanceToSquared(obj.X, obj.Y);
        }

        public bool CanMove()
        {
            if (_character.PBlockAct == 1)
            {
                return false;
            }

            if ((_character.AbnormalBitMaskEx & L2Character.AbnormalMaskExFreezing) == L2Character.AbnormalMaskExFreezing)
            {
                return false;
            }

            return true;
        }

        public void UpdatePosition()
        {
            PerformMove(true);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdatePosition(int x, int y, int z)
        {
            if (!IsMoving || !CanMove())
            {
                return;
            }

            bool slowDown = Utilz.DistanceSq(x, y, DestinationX, DestinationY) > Utilz.DistanceSq(_x, _y, DestinationX, DestinationY);

            int dx = x - _x;
            int dy = y - _y;

            double distance = Utilz.Length(dx, dy);

            long currentTime = DateTime.UtcNow.Ticks;

            // TODO: move to config
            const int maxSpeedUpPerSecondUnsync = 20;

            int distanceAllowedUnsync = (int)((slowDown ? _character.CharacterStat.MoveSpeed : maxSpeedUpPerSecondUnsync) 
                * (currentTime - _movementUpdateTime) / TimeSpan.TicksPerSecond);

            if (distance <= distanceAllowedUnsync)
            {
                _x = x;
                _y = y;
            }
            else
            {
                _x += (int) (dx / distance * distanceAllowedUnsync);
                _y += (int) (dy / distance * distanceAllowedUnsync);
            }

            Z = z;

            _movementUpdateTime = currentTime;
        }

        public async Task MoveTo(int x, int y, int z)
        {
            if (!CanMove())
            {
                _character.SendActionFailedAsync();
                return;
            }

            if (_character.IsAttacking())
            {
                _character.AbortAttack();
            }

            if (IsMoving)
            {
                UpdatePosition();
                await NotifyStopMove(false);
            }

            float dx = x - X;
            float dy = y - Y;
            float dz = z - Z;


            if (dx * dx + dy * dy > 9900 * 9900)
            {
                _character.SendActionFailedAsync();
                return;
            }

            DestinationX = x;
            DestinationY = y;
            DestinationZ = z;

            Vector2 targetVector = new Vector2(dx, dy);
            targetVector /= targetVector.Length();

            Heading = (int) (Math.Atan2(-targetVector.X, -targetVector.Y) * 10430.378 + short.MaxValue);

            _movementUpdateTime = _movementLastTime = DateTime.UtcNow.Ticks;
            _attackTarget = null;
            IsMoving = true;

            await _character.BroadcastPacketAsync(new CharMoveToLocation(_character));
        }

        public async Task MoveToAndHit(L2Character target)
        {
            _attackTarget = target;
            await MoveTo(target.X, target.Y, target.Z);
            Task.Factory.StartNew(BroadcastDestinationChanged);
        }

        private async void BroadcastDestinationChanged()
        {
            int oldDestinationX = DestinationX;
            int oldDestinationY = DestinationY;
            int oldDestinationZ = DestinationZ;

            await Task.Delay(1000); // TODO: move to config

            while (IsMoving && _attackTarget != null)
            {
                if (oldDestinationX != DestinationX || oldDestinationY != DestinationY || oldDestinationZ != DestinationZ)
                {
                    // Send broadcast every 1s if destination is changed
                    _character.BroadcastPacketAsync(new CharMoveToLocation(_character));
                }

                oldDestinationX = DestinationX;
                oldDestinationY = DestinationY;
                oldDestinationZ = DestinationZ;

                // TODO: move to config
                await Task.Delay(1000);
            }
        }

        public async Task NotifyStopMove(bool broadcast = true)
        {
            IsMoving = false;

            if (broadcast)
            {
                await _character.BroadcastPacketAsync(new StopMove(_character));
            }
        }

        public async Task NotifyArrived()
        {
            IsMoving = false;

            if (_attackTarget != null)
            {
               await _character.DoAttackAsync(_attackTarget);
                _attackTarget = null;
            }
        }
    }
}
