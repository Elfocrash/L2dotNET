using L2dotNET.GameService.model.playable.petai;
using L2dotNET.GameService.tools;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.model.npcs.ai
{
    public class WarriorAI : StandartAiTemplate
    {
        public WarriorAI(L2Character cha)
        {
            character = cha;
        }

        public System.Timers.Timer attackMove;

        public override void NotifyOnHit(L2Character attacker, double damage)
        {
            MoveHome = 0;
            if (character.isMoving())
                character.NotifyStopMove(true, true);

            character.ChangeTarget(attacker);
            attack(attacker);
        }

        public void attack(L2Character target)
        {
            if (attackMove == null)
            {
                attackMove = new System.Timers.Timer();
                attackMove.Elapsed += new System.Timers.ElapsedEventHandler(AttackMoveTask);
                attackMove.Interval = 100;
            }

            attackMove.Enabled = true;
        }

        private void AttackMoveTask(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (MoveHome > 0)
            {
                ValidateSpawnLocation();
                return;
            }

            if (character.isAttacking())
                return;

            double dis = Calcs.calculateDistance(character, character.CurrentTarget, true);
            if (dis < 80)
            {
                MoveTarget = 0;
                L2Character target = (L2Character)character.CurrentTarget;
                character.doAttack(target);
            }
            else
            {
                if (character.cantMove())
                {
                    return;
                }

                if (lastx != character.CurrentTarget.X || lasty != character.CurrentTarget.Y || lastz != character.CurrentTarget.Z)
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

        public int lastx, lasty, lastz;

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

        private byte MoveHome = 0;
        private byte MoveTarget = 0;

        private void ValidateSpawnLocation()
        {
            if (character.cantMove())
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