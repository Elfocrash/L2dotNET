using L2dotNET.tools;

namespace L2dotNET.model.playable.petai
{
    public class SaStandart : StandartAiTemplate
    {
        public SaStandart(L2Summon s)
        {
            Character = s;
            _summon = s;
        }

        private int _lastOwnerX,
                    _lastOwnerY,
                    _lastOwnerZ;
        private readonly L2Summon _summon;

        public override void DoFollow(object sender = null, System.Timers.ElapsedEventArgs e = null)
        {
            if (_summon.CantMove())
                return;

            double dis = Calcs.CalculateDistance(_summon, _summon.Owner, true);

            if (!(dis > 120))
                return;

            if ((_lastOwnerX == _summon.Owner.X) || (_lastOwnerY == _summon.Owner.Y) || (_lastOwnerZ == _summon.Owner.Z))
                return;

            Character.MoveTo(_summon.Owner.X, _summon.Owner.Y, _summon.Owner.Z);

            _lastOwnerX = _summon.Owner.X;
            _lastOwnerY = _summon.Owner.Y;
            _lastOwnerZ = _summon.Owner.Z;
        }

        public override void DoThink(object sender = null, System.Timers.ElapsedEventArgs e = null) { }
    }
}