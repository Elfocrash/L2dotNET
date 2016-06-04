using L2dotNET.GameService.model.playable.petai;
using L2dotNET.GameService.model.skills2;
using L2dotNET.GameService.tools;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.model.player.ai
{
    public class PlayerAI : StandartAiTemplate
    {
        private L2Player player;
        public PlayerAI(L2Character cha)
        {
            character = cha;
            player = (L2Player)cha;
        }

        public override void NotifyStartMoving()
        {
            foreach (TSpecEffect ef in player.specEffects)
                ef.OnStartMoving(player);
        }

        public override void NotifyStopMoving()
        {
            foreach (TSpecEffect ef in player.specEffects)
                ef.OnStopMoving(player);
        }

        public override void NotifyOnStartDay()
        {
            foreach (TSpecEffect ef in player.specEffects)
                ef.OnStartDay(player);
        }

        public override void NotifyOnStartNight()
        {
            foreach (TSpecEffect ef in player.specEffects)
                ef.OnStartNight(player);
        }

        public override void StopAutoAttack()
        {
            if (attackMove != null)
                attackMove.Enabled = false;

            MoveTarget = 0;
        }

        public System.Timers.Timer attackMove;
        public override void Attack(L2Character target)
        {
            if (attackMove == null)
            {
                attackMove = new System.Timers.Timer();
                attackMove.Elapsed += new System.Timers.ElapsedEventHandler(AttackMoveTask);
                attackMove.Interval = 100;
            }

            MoveTarget = 0;
            attackMove.Enabled = true;
        }

        int lastx, lasty, lastz;
        byte MoveTarget = 0;
        private void AttackMoveTask(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (player.isAttacking())
                return;

            if (player.CurrentTarget == null)
            {
                attackMove.Enabled = false;
                return;
            }

            double dis = Calcs.calculateDistance(player, character.CurrentTarget, true);
            if (dis < 80)
            {
                L2Character target = (L2Character)player.CurrentTarget;
                if(!target.Dead)
                    player.doAttack(target);
            }
            else
            {
                if (player.cantMove())
                    return;

                if (lastx != player.CurrentTarget.X || lasty != player.CurrentTarget.Y || lastz != player.CurrentTarget.Z)
                    MoveTarget = 0;

                if (MoveTarget == 0)
                {
                    player.MoveTo(player.CurrentTarget.X, player.CurrentTarget.Y, player.CurrentTarget.Z);
                    MoveTarget = 1;
                    lastx = character.CurrentTarget.X;
                    lasty = character.CurrentTarget.Y;
                    lastz = character.CurrentTarget.Z;
                }
            }
        }
    }
}
