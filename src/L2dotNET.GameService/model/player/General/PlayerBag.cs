using System;
using System.Collections.Generic;
using System.Dynamic;

namespace L2dotNET.GameService.Model.Player.General
{
    public class PlayerBag : DynamicObject
    {
        private readonly Dictionary<string, dynamic> _properties = new Dictionary<string, dynamic>(StringComparer.InvariantCultureIgnoreCase);

        public override bool TryGetMember(GetMemberBinder binder, out dynamic result)
        {
            result = this._properties.ContainsKey(binder.Name) ? this._properties[binder.Name] : null;

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, dynamic value)
        {
            if (value == null)
            {
                if (_properties.ContainsKey(binder.Name))
                {
                    _properties.Remove(binder.Name);
                }
            }
            else
            {
                _properties[binder.Name] = value;
            }

            return true;
        }
    }
}