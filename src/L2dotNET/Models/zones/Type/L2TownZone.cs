using System;
using L2dotNET.Enums;
using L2dotNET.Utility;
using L2dotNET.world;

namespace L2dotNET.model.zones.Type
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
            if (name.EqualsIgnoreCase("townId"))
                TownId = Convert.ToInt32(value);
            else
            {
                if (name.EqualsIgnoreCase("taxById"))
                    TaxById = Convert.ToInt32(value);
                else
                {
                    if (name.EqualsIgnoreCase("isPeaceZone"))
                        IsPeaceZone = bool.Parse(value);
                }
            }
        }

        protected override void OnEnter(L2Character character)
        {
            if (IsPeaceZone)
                character.SetInsisdeZone(ZoneId.Peace, true);

            character.SetInsisdeZone(ZoneId.Town, true);
        }

        protected override void OnExit(L2Character character)
        {
            if (IsPeaceZone)
                character.SetInsisdeZone(ZoneId.Peace, false);

            character.SetInsisdeZone(ZoneId.Town, false);
        }

        public override void OnDieInside(L2Character character) { }

        public override void OnReviveInside(L2Character character) { }
    }
}