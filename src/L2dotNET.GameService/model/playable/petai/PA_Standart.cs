using System;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tools;

namespace L2dotNET.GameService.Model.Playable.PetAI
{
    public class PA_Standart : StandartAiTemplate
    {
        private readonly L2Pet pet;

        public PA_Standart(L2Summon s)
        {
            character = s;
            pet = (L2Pet)character;
        }

        private DateTime under55percent;

        private int lastOwnerX,
                    lastOwnerY,
                    lastOwnerZ;

        public override void DoFollow(object sender = null, System.Timers.ElapsedEventArgs e = null)
        {
            if (pet.CantMove())
                return;

            double dis = Calcs.calculateDistance(pet, pet.Owner, true);

            if (dis > 120)
                if ((lastOwnerX != pet.Owner.X) && (lastOwnerY != pet.Owner.Y) && (lastOwnerZ != pet.Owner.Z))
                {
                    pet.MoveTo(pet.Owner.X, pet.Owner.Y, pet.Owner.Z);

                    lastOwnerX = pet.Owner.X;
                    lastOwnerY = pet.Owner.Y;
                    lastOwnerZ = pet.Owner.Z;
                }
        }

        public override void DoThink(object sender = null, System.Timers.ElapsedEventArgs e = null)
        {
            if (pet.CurrentTime / pet.MaxTime < 0.55)
            {
                under55percent = DateTime.Now;
                pet.Owner.SendSystemMessage(SystemMessage.SystemMessageId.YOUR_PET_IS_VERY_HUNGRY);
            }
        }
    }
}