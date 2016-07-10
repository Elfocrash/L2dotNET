using System;
using L2dotNET.GameService.Network.Serverpackets;
using L2dotNET.GameService.Tools;

namespace L2dotNET.GameService.Model.Playable.PetAI
{
    public class PaStandart : StandartAiTemplate
    {
        private readonly L2Pet _pet;

        public PaStandart(L2Summon s)
        {
            Character = s;
            _pet = (L2Pet)Character;
        }

        private DateTime _under55Percent;

        private int _lastOwnerX,
                    _lastOwnerY,
                    _lastOwnerZ;

        public override void DoFollow(object sender = null, System.Timers.ElapsedEventArgs e = null)
        {
            if (_pet.CantMove())
            {
                return;
            }

            double dis = Calcs.CalculateDistance(_pet, _pet.Owner, true);

            if (dis > 120)
            {
                if ((_lastOwnerX != _pet.Owner.X) && (_lastOwnerY != _pet.Owner.Y) && (_lastOwnerZ != _pet.Owner.Z))
                {
                    _pet.MoveTo(_pet.Owner.X, _pet.Owner.Y, _pet.Owner.Z);

                    _lastOwnerX = _pet.Owner.X;
                    _lastOwnerY = _pet.Owner.Y;
                    _lastOwnerZ = _pet.Owner.Z;
                }
            }
        }

        public override void DoThink(object sender = null, System.Timers.ElapsedEventArgs e = null)
        {
            if ((_pet.CurrentTime / (float)_pet.MaxTime) < 0.55)
            {
                _under55Percent = DateTime.Now;
                _pet.Owner.SendSystemMessage(SystemMessage.SystemMessageId.YourPetIsVeryHungry);
            }
        }
    }
}