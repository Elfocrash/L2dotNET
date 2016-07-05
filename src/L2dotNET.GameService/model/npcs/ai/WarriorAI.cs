using System.Timers;
using L2dotNET.GameService.Model.Playable.PetAI;
using L2dotNET.GameService.Tools;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Npcs.Ai
{
    public class WarriorAi : StandartAiTemplate
    {
        public WarriorAi(L2Character cha)
        {
            Character = cha;
        }

        public Timer AttackMove;

        public override void NotifyOnHit(L2Character attacker, double damage)
        {
            _moveHome = 0;
            if (Character.IsMoving())
            {
                Character.NotifyStopMove(true, true);
            }

            Character.ChangeTarget(attacker);
            attack(attacker);
        }

        public void attack(L2Character target)
        {
            if (AttackMove == null)
            {
                AttackMove = new Timer();
                AttackMove.Elapsed += new ElapsedEventHandler(AttackMoveTask);
                AttackMove.Interval = 100;
            }

            AttackMove.Enabled = true;
        }

        private void AttackMoveTask(object sender, ElapsedEventArgs e)
        {
            if (_moveHome > 0)
            {
                ValidateSpawnLocation();
                return;
            }

            if (Character.IsAttacking())
            {
                return;
            }

            double dis = Calcs.CalculateDistance(Character, Character.CurrentTarget, true);
            if (dis < 80)
            {
                _moveTarget = 0;
                L2Character target = Character.CurrentTarget;
                Character.DoAttack(target);
            }
            else
            {
                if (Character.CantMove())
                {
                    return;
                }

                if ((Lastx != Character.CurrentTarget.X) || (Lasty != Character.CurrentTarget.Y) || (Lastz != Character.CurrentTarget.Z))
                {
                    _moveTarget = 0;
                }

                if (_moveTarget == 0)
                {
                    _moveTarget = 1;
                    Character.MoveTo(Character.CurrentTarget.X, Character.CurrentTarget.Y, Character.CurrentTarget.Z);
                    Lastx = Character.CurrentTarget.X;
                    Lasty = Character.CurrentTarget.Y;
                    Lastz = Character.CurrentTarget.Z;
                }
            }
        }

        public int Lastx,
                   Lasty,
                   Lastz;

        public override void NotifyOnDie(L2Character killer)
        {
            if (AttackMove != null)
            {
                AttackMove.Enabled = false;
            }

            base.NotifyOnDie(killer);
        }

        public override void NotifyTargetDead()
        {
            _moveHome = 1;
            base.NotifyTargetDead();
        }

        public override void NotifyTargetNull()
        {
            Character.ChangeTarget();
            _moveHome = 1;
            base.NotifyTargetNull();
        }

        private byte _moveHome;
        private byte _moveTarget;

        private void ValidateSpawnLocation()
        {
            if (Character.CantMove())
            {
                return;
            }

            switch (_moveHome)
            {
                case 1:
                    double dis = Calcs.CalculateDistance(Character.X, Character.Y, Character.Z, Character.SpawnX, Character.SpawnZ, Character.SpawnZ, true);
                    if (dis > 100)
                    {
                        _moveHome = 2;
                        Character.MoveTo(Character.SpawnX, Character.SpawnY, Character.SpawnZ);
                    }
                    else
                    {
                        _moveHome = 3;
                    }
                    break;
                case 3:
                    if (AttackMove != null)
                    {
                        AttackMove.Enabled = false;
                    }

                    break;
            }
        }
    }
}