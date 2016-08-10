using L2dotNET.model.playable.petai;
using L2dotNET.tools;
using L2dotNET.world;

namespace L2dotNET.model.player.ai
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
            _player.SpecEffects.ForEach(ef => ef.OnStartMoving(_player));
        }

        public override void NotifyStopMoving()
        {
            _player.SpecEffects.ForEach(ef => ef.OnStopMoving(_player));
        }

        public override void NotifyOnStartDay()
        {
            _player.SpecEffects.ForEach(ef => ef.OnStartDay(_player));
        }

        public override void NotifyOnStartNight()
        {
            _player.SpecEffects.ForEach(ef => ef.OnStartNight(_player));
        }

        public override void StopAutoAttack()
        {
            if (AttackMove != null)
                AttackMove.Enabled = false;

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
                return;

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
                    _player.DoAttack(target);
            }
            else
            {
                if (_player.CantMove())
                    return;

                if ((_lastx != _player.CurrentTarget.X) || (_lasty != _player.CurrentTarget.Y) || (_lastz != _player.CurrentTarget.Z))
                    _moveTarget = 0;

                if (_moveTarget != 0)
                    return;

                _player.MoveTo(_player.CurrentTarget.X, _player.CurrentTarget.Y, _player.CurrentTarget.Z);
                _moveTarget = 1;
                _lastx = Character.CurrentTarget.X;
                _lasty = Character.CurrentTarget.Y;
                _lastz = Character.CurrentTarget.Z;
            }
        }
    }
}