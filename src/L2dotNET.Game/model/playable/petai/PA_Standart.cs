using System;
using L2dotNET.GameService.tools;

namespace L2dotNET.GameService.model.playable.petai
{
    public class PA_Standart : StandartAiTemplate
    {
        private L2Pet pet;
        public PA_Standart(L2Summon s)
        {
            character = s;
            pet = (L2Pet)character;
        }

        private DateTime under55percent;

        private int lastOwnerX, lastOwnerY, lastOwnerZ;

        public override void DoFollow(object sender = null, System.Timers.ElapsedEventArgs e = null)
        {
            if (pet.cantMove())
                return;

            double dis = Calcs.calculateDistance(pet, pet.Owner, true);

            if (dis > 120)
            {
                if (lastOwnerX != pet.Owner.X && lastOwnerY != pet.Owner.Y && lastOwnerZ != pet.Owner.Z)
                {
                    pet.MoveTo(pet.Owner.X, pet.Owner.Y, pet.Owner.Z);

                    lastOwnerX = pet.Owner.X;
                    lastOwnerY = pet.Owner.Y;
                    lastOwnerZ = pet.Owner.Z;
                }
            }
        }

        public override void DoThink(object sender = null, System.Timers.ElapsedEventArgs e = null)
        {
            if (pet.CurrentTime / pet.MaxTime < 0.55)
            {
                if (under55percent == null)
                {
                    under55percent = DateTime.Now;
                    pet.Owner.sendSystemMessage(595);//Your pet is very hungry.
                }
            }
        }
    }
}
