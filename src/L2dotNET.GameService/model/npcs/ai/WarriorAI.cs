using System.Timers;
using L2dotNET.GameService.Model.Playable.PetAI;
using L2dotNET.GameService.Tools;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Npcs.Ai
{
    public class WarriorAI : StandartAiTemplate
    {
        public WarriorAI(L2Character cha)
        {
            character = cha;
        }

        public Timer attackMove;

        public override void NotifyOnHit(L2Character attacker, double damage)
        {
            MoveHome = 0;
            if (character.IsMoving())
                character.NotifyStopMove(true, true);

            character.ChangeTarget(attacker);
            attack(attacker);
        }

        public void attack(L2Character target)
        {
            if (attackMove == null)
            {
                attackMove = new Timer();
                attackMove.Elapsed += new ElapsedEventHandler(AttackMoveTask);
                attackMove.Interval = 100;
            }

            attackMove.Enabled = true;
        }

        private void AttackMoveTask(object sender, ElapsedEventArgs e)
        {
            if (MoveHome > 0)
            {
                ValidateSpawnLocation();
                return;
            }

            if (character.IsAttacking())
                return;

            double dis = Calcs.calculateDistance(character, character.CurrentTarget, true);
            if (dis < 80)
            {
                MoveTarget = 0;
                L2Character target = character.CurrentTarget;
                character.DoAttack(target);
            }
            else
            {
                if (character.CantMove())
                    return;

                if ((lastx != character.CurrentTarget.X) || (lasty != character.CurrentTarget.Y) || (lastz != character.CurrentTarget.Z))
                    MoveTarget = 0;

                if (MoveTarget == 0)
                {
                    MoveTarget = 1;
                    character.MoveTo(character.CurrentTarget.X, character.CurrentTarget.Y, character.CurrentTarget.Z);
                    lastx = character.CurrentTarget.X;
                    lasty = character.CurrentTarget.Y;
                    lastz = character.CurrentTarget.Z;
                }
            }
        }

        public int lastx,
                   lasty,
                   lastz;

        public override void NotifyOnDie(L2Character killer)
        {
            if (attackMove != null)
                attackMove.Enabled = false;

            base.NotifyOnDie(killer);
        }

        public override void NotifyTargetDead()
        {
            MoveHome = 1;
            base.NotifyTargetDead();
        }

        public override void NotifyTargetNull()
        {
            character.ChangeTarget();
            MoveHome = 1;
            base.NotifyTargetNull();
        }

        private byte MoveHome;
        private byte MoveTarget;

        private void ValidateSpawnLocation()
        {
            if (character.CantMove())
                return;

            switch (MoveHome)
            {
                case 1:
                    double dis = Calcs.calculateDistance(character.X, character.Y, character.Z, character.SpawnX, character.SpawnZ, character.SpawnZ, true);
                    if (dis > 100)
                    {
                        MoveHome = 2;
                        character.MoveTo(character.SpawnX, character.SpawnY, character.SpawnZ);
                    }
                    else
                    {
                        MoveHome = 3;
                    }
                    break;
                case 3:
                    if (attackMove != null)
                        attackMove.Enabled = false;

                    break;
            }
        }
    }
}