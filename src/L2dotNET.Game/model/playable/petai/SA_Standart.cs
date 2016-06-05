using L2dotNET.GameService.Tools;

namespace L2dotNET.GameService.Model.Playable.PetAI
{
    public class SA_Standart : StandartAiTemplate
    {
        public SA_Standart(L2Summon s)
        {
            character = s;
            summon = s;
        }

        private int lastOwnerX,
                    lastOwnerY,
                    lastOwnerZ;
        private readonly L2Summon summon;

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

        public override void DoThink(object sender = null, System.Timers.ElapsedEventArgs e = null) { }
    }
}