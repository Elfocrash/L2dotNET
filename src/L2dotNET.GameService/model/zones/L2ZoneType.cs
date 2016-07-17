using System.Collections.Generic;
using System.Linq;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.Model.Zones.Forms;
using L2dotNET.GameService.Network;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Zones
{
    public abstract class L2ZoneType
    {
        public int Id { get; }

        public L2ZoneForm Zone { get; set; }

        protected List<L2Character> CharacterList;

        protected L2ZoneType(int id)
        {
            Id = id;
            CharacterList = new List<L2Character>();
        }

        public bool IsInsideZone(int x, int y)
        {
            return Zone.IsInsideZone(x, y, Zone.GetHighZ());
        }

        public bool IsInsideZone(int x, int y, int z)
        {
            return Zone.IsInsideZone(x, y, z);
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

            if (IsInsideZone(character.X, character.Y, character.Z))
            {
                if (CharacterList.Contains(character))
                    return;
                //quest check here
                CharacterList.Add(character);
                OnEnter(character);
            }
            else
            {
                if (!CharacterList.Contains(character))
                    return;
                //quest check here
                CharacterList.Remove(character);
                OnExit(character);
            }
        }

        public void RemoveCharacter(L2Character character)
        {
            if (!CharacterList.Contains(character))
                return;
            //quest check here
            CharacterList.Remove(character);
            OnEnter(character);
        }

        public bool IsCharacterInZone(L2Character character)
        {
            return CharacterList.Contains(character);
        }

        protected abstract void OnEnter(L2Character character);

        protected abstract void OnExit(L2Character character);

        public abstract void OnDieInside(L2Character character);

        public abstract void OnReviveInside(L2Character character);

        public abstract void SetParameter(string name, string value);

        public List<L2Character> GetCharactersInside()
        {
            return CharacterList;
        }

        public void BroadcastPacket(GameServerNetworkPacket packet)
        {
            if (CharacterList.Count == 0)
                return;

            foreach (L2Player character in CharacterList.OfType<L2Player>())
                character.SendPacket(packet);
        }
    }
}