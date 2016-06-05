using System;
using L2dotNET.GameService.Enums;
using L2dotNET.GameService.world;

namespace L2dotNET.GameService.Model.zones.Type
{
    public class L2TownZone : L2SpawnZone
    {
        public int TaxById { get; private set; }
        public bool IsPeaceZone { get; private set; }
        public int TownId { get; private set; }

        public L2TownZone(int id) : base(id)
        {
            TaxById = 0;
            IsPeaceZone = true;
        }

        public override void SetParameter(string name, string value)
        {
            if (name.Equals("townId"))
                TownId = Convert.ToInt32(value);
            else if (name.Equals("taxById"))
                TaxById = Convert.ToInt32(value);
            else if (name.Equals("isPeaceZone"))
                IsPeaceZone = bool.Parse(value);
        }

        protected override void OnEnter(L2Character character)
        {
            if (IsPeaceZone)
                character.SetInsisdeZone(ZoneId.PEACE, true);

            character.SetInsisdeZone(ZoneId.TOWN, true);
        }

        protected override void OnExit(L2Character character)
        {
            if (IsPeaceZone)
                character.SetInsisdeZone(ZoneId.PEACE, false);

            character.SetInsisdeZone(ZoneId.TOWN, false);
        }

        public override void OnDieInside(L2Character character) { }

        public override void OnReviveInside(L2Character character) { }
    }
}