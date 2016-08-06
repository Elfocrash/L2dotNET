using System.Linq;
using L2dotNET.Enums;
using L2dotNET.GameService.Templates;
using L2dotNET.Utility;

namespace L2dotNET.GameService.Model.Items
{
    public class Weapon : ItemTemplate
    {
        public WeaponTypeId Type { get; set; }
        public int SoulshotCount { get; set; }
        public int SpiritshotCount { get; set; }
        public int PDam { get; set; }
        public int RndDam { get; set; }
        public int Critical { get; set; }
        public double HitModifier { get; set; }
        public int AvoidModifier { get; set; }
        public int ShieldDef { get; set; }
        public double ShieldDefRate { get; set; }
        public int AtkSpeed { get; set; }
        public int AtkReuse { get; set; }
        public int MpConsume { get; set; }
        public int MDam { get; set; }

        public Weapon(StatsSet set) : base(set)
        {
            Type = Utilz.GetEnumFromString(set.GetString("weaponType", "none"), WeaponTypeId.None);
            SoulshotCount = set.GetInt("soulshots");
            SpiritshotCount = set.GetInt("spiritshots");
            PDam = set.GetInt("p_dam");
            RndDam = set.GetInt("rnd_dam");
            Critical = set.GetInt("critical");
            HitModifier = set.GetDouble("hit_modify");
            AvoidModifier = set.GetInt("avoid_modify");
            ShieldDef = set.GetInt("shield_def");
            ShieldDefRate = set.GetDouble("shield_def_rate");
            AtkSpeed = set.GetInt("atk_speed");
            AtkReuse = set.GetInt("atk_reuse", Type == WeaponTypeId.Bow ? 1500 : 0);
            MpConsume = set.GetInt("mp_consume");
            MDam = set.GetInt("m_dam");
        }

        public override int GetItemMask()
        {
            var orDefault = WeaponType.Values.FirstOrDefault(x => x.Id == Type);
            if (orDefault != null)
            {
                int firstOrDefault = orDefault.GetMask();
                return firstOrDefault;
            }
            return 0;
        }

    }
}