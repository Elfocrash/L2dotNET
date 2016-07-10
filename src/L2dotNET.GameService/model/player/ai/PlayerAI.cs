using L2dotNET.GameService.Model.Playable.PetAI;
using L2dotNET.GameService.Model.Skills2;
using L2dotNET.GameService.Tools;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Player.AI
{
    public class PlayerAi : StandartAiTemplate
    {
        private readonly L2Player _player;

        public PlayerAi(L2Character cha)
        {
            Character = cha;
            _player = (L2Player)cha;
        }

        public override void NotifyStartMoving()
        {
            foreach (SpecEffect ef in _player.SpecEffects)
            {
                ef.OnStartMoving(_player);
            }
        }

        public override void NotifyStopMoving()
        {
            foreach (SpecEffect ef in _player.SpecEffects)
            {
                ef.OnStopMoving(_player);
            }
        }

        public override void NotifyOnStartDay()
        {
            foreach (SpecEffect ef in _player.SpecEffects)
            {
                ef.OnStartDay(_player);
            }
        }

        public override void NotifyOnStartNight()
        {
            foreach (SpecEffect ef in _player.SpecEffects)
            {
                ef.OnStartNight(_player);
            }
        }

        public override void StopAutoAttack()
        {
            if (AttackMove != null)
            {
                AttackMove.Enabled = false;
            }

            _moveTarget = 0;
        }

        public System.Timers.Timer AttackMove;

        public override void Attack(L2Character target)
        {
            if (AttackMove == null)
            {
                AttackMove = new System.Timers.Timer();
                AttackMove.Elapsed += AttackMoveTask;
                AttackMove.Interval = 100;
            }

            _moveTarget = 0;
            AttackMove.Enabled = true;
        }

        private int _lastx;
        private int _lasty;
        private int _lastz;
        private byte _moveTarget;

        private void AttackMoveTask(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_player.IsAttacking())
            {
                return;
            }

            if (_player.CurrentTarget == null)
            {
                AttackMove.Enabled = false;
                return;
            }

            double dis = Calcs.CalculateDistance(_player, Character.CurrentTarget, true);
            if (dis < 80)
            {
                L2Character target = _player.CurrentTarget;
                if (!target.Dead)
                {
                    _player.DoAttack(target);
                }
            }
            else
            {
                if (_player.CantMove())
                {
                    return;
                }

                if ((_lastx != _player.CurrentTarget.X) || (_lasty != _player.CurrentTarget.Y) || (_lastz != _player.CurrentTarget.Z))
                {
                    _moveTarget = 0;
                }

                if (_moveTarget != 0)
                {
                    return;
                }

                _player.MoveTo(_player.CurrentTarget.X, _player.CurrentTarget.Y, _player.CurrentTarget.Z);
                _moveTarget = 1;
                _lastx = Character.CurrentTarget.X;
                _lasty = Character.CurrentTarget.Y;
                _lastz = Character.CurrentTarget.Z;
            }
        }
    }
}