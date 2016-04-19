using L2dotNET.Game.tools;

namespace L2dotNET.Game.model.playable.petai
{
    public class SA_Standart : StandartAiTemplate
    {
        public SA_Standart(L2Summon s)
        {
            character = s;
            summon = s;
        }

        private int lastOwnerX, lastOwnerY, lastOwnerZ;
        private L2Summon summon;

        public override void DoFollow(object sender = null, System.Timers.ElapsedEventArgs e = null)
        {
            if (summon.cantMove())
                return;

            double dis = Calcs.calculateDistance(summon, summon.Owner, true);

            if (dis > 120)
            {
                if (lastOwnerX != summon.Owner.X && lastOwnerY != summon.Owner.Y && lastOwnerZ != summon.Owner.Z)
                {
                    character.MoveTo(summon.Owner.X, summon.Owner.Y, summon.Owner.Z);

                    lastOwnerX = summon.Owner.X;
                    lastOwnerY = summon.Owner.Y;
                    lastOwnerZ = summon.Owner.Z;
                }
            }
        }

        public override void DoThink(object sender = null, System.Timers.ElapsedEventArgs e = null)
        {

        }
    }
}
