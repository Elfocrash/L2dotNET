using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2dotNET.Game.world;
using L2dotNET.Game.Enums;

namespace L2dotNET.Game.model.zones.Type
{
    public class L2TownZone : L2SpawnZone
    {
        private int _townId;
        private int _taxById;
        private bool _isPeaceZone;
        public int TaxById { get { return _taxById; } }
        public bool IsPeaceZone { get { return _isPeaceZone; } }
        public int TownId { get { return _townId; } }

        public L2TownZone(int id) : base(id)
        {
            _taxById = 0;
            _isPeaceZone = true;
        }

        public override void SetParameter(string name, string value)
        {
            if (name.Equals("townId"))
                _townId = Convert.ToInt32(value);
            else if (name.Equals("taxById"))
                _taxById = Convert.ToInt32(value);
            else if (name.Equals("isPeaceZone"))
                _isPeaceZone = bool.Parse(value);
        }

        protected override void OnEnter(L2Character character)
        {
            if (_isPeaceZone)
                character.SetInsisdeZone(ZoneId.PEACE, true);

            character.SetInsisdeZone(ZoneId.TOWN, true);
        }

        protected override void OnExit(L2Character character)
        {
            if (_isPeaceZone)
                character.SetInsisdeZone(ZoneId.PEACE, false);

            character.SetInsisdeZone(ZoneId.TOWN, false);
        }

        public override void OnDieInside(L2Character character)
        {

        }

        public override void OnReviveInside(L2Character character)
        {

        }
    }
}
