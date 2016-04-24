using L2dotNET.Game.logger;
using L2dotNET.Game.model.zones.forms;
using L2dotNET.Game.network;
using L2dotNET.Game.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Game.model.zones
{
    public abstract class L2ZoneType
    {
        private int _id;
        public int Id { get { return _id; } }
        protected L2ZoneForm _zone;
        public L2ZoneForm Zone { get { return _zone; } set { _zone = value; } }

        protected List<L2Character> _characterList;

        public L2ZoneType(int id)
        {
            _id = id;
            _characterList = new List<L2Character>();
        }

        public bool IsInsideZone(int x, int y)
        {
            return _zone.IsInsideZone(x, y, _zone.GetHighZ());
        }

        public bool IsInsideZone(int x, int y, int z)
        {
            return _zone.IsInsideZone(x, y, z);
        }

        public bool IsInsideZone(L2Object obj)
        {
            return IsInsideZone(obj.X, obj.Y, obj.Z);
        }

        public double GetDistanceToZone(int x, int y)
        {
            return Zone.GetDistanceToZone(x, y);
        }

        public double GetDistanceToZone(L2Object obj)
        {
            return Zone.GetDistanceToZone(obj.X, obj.Y);
        }

        protected bool IsAffected(L2Character character)
        {
            return false;
        }

        public void RevalidateInZone(L2Character character)
        {
            if (!IsAffected(character))
                return;

            if(IsInsideZone(character.X,character.Y,character.Z))
            {
                if(!_characterList.Contains(character))
                {
                    //quest check here
                    _characterList.Add(character);
                    OnEnter(character);
                }
            }
            else
            {
                if (_characterList.Contains(character))
                {
                    //quest check here
                    _characterList.Remove(character);
                    OnExit(character);
                }
            }
        }

        public void RemoveCharacter(L2Character character)
        {
            if (_characterList.Contains(character))
            {
                //quest check here
                _characterList.Remove(character);
                OnEnter(character);
            }
        }

        public bool IsCharacterInZone(L2Character character)
        {
            return _characterList.Contains(character);
        }

        protected abstract void OnEnter(L2Character character);

        protected abstract void OnExit(L2Character character);

        public abstract void OnDieInside(L2Character character);

        public abstract void OnReviveInside(L2Character character);

        public abstract void SetParameter(string name, string value);

        public List<L2Character> GetCharactersInside()
        {
            return _characterList;
        }

        public void BroadcastPacket(GameServerNetworkPacket packet)
        {
            if (_characterList.Count == 0)
                return;

            foreach(L2Character character in _characterList)
            {
                if (character != null && character is L2Player)
                    character.sendPacket(packet);
            }
        }


    }
}
