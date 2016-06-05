using L2dotNET.GameService.Model.Items;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Model.Skills2
{
    public class TSpecEffect
    {
        public double value;
        public bool mul = false;

        public virtual void OnStartMoving(L2Player player) { }

        public virtual void OnStopMoving(L2Player player) { }

        public virtual void OnStartNight(L2Player player) { }

        public virtual void OnStartDay(L2Player player) { }

        public virtual void OnReceiveDamage(L2Player player, L2Character attacker) { }

        public virtual void OnGaveDamage(L2Player player, L2Character target, bool crit) { }

        public virtual void OnCastSkill(L2Player player, L2Character target, TSkill skill) { }

        public virtual void OnStand(L2Player player) { }

        public virtual void OnSit(L2Player player) { }

        public virtual void OnEquipChest(L2Player player, L2Item item) { }

        public virtual void OnUnEquipChest(L2Player player) { }
    }
}