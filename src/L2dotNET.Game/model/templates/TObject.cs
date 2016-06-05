namespace L2dotNET.GameService.Model.templates
{
    public class TObject
    {
        public int NpcId;
        public TObjectCategory Category;
        public double CollisionRadius;
        public double CollisionHeight;
        public byte Level;
        public long exp;
        public int ex_crt_effect = 1;
        public int unique = 0;
        public double s_npc_prop_hp_rate = 1;
        public TObjectRace Race;
        public TObjectSex Sex;
        public string slot_chest;
        public string slot_rhand;
        public string slot_lhand;
        public double hit_time_factor;
        public double hit_time_factor_skill = -1;
        public int Str;
        public int Int;
        public int Dex;
        public int Wit;
        public int Con;
        public int Men;
        public double org_hp;
        public double org_hp_regen;
        public double org_mp;
        public double org_mp_regen;
        public TObjectBaseAttackType base_attack_type;
        public int base_attack_range;
        public int base_rand_dam;
        public double base_physical_attack;
        public int base_critical;
        public double physical_hit_modify;
        public int base_attack_speed;
        public int base_reuse_delay;
        public double base_magic_attack;
        public double base_defend;
        public double base_magic_defend;
        public double physical_avoid_modify;
        public int shield_defense_rate;
        public double shield_defense;
        public int safe_height = 100;
        public int soulshot_count;
        public int spiritshot_count;
        public string clan;
        public int clan_help_range;
        public int undying = 0;
        public int can_be_attacked = 1;
        public int corpse_time = 7;
        public int no_sleep_mode;
        public int agro_range;
        public int passable_door;
        public int can_move = 1;
        public int flying;
        public int has_summoner;
        public int targetable = 1;
        public int show_name_tag = 1;
        public int event_flag;
        public int unsowing = 1;
        public int private_respawn_log;
        public double acquire_exp_rate;
        public int acquire_sp;
        public int acquire_rp;
        public int fake_class_id = -1;

        internal void setNpcSkills(string value) { }
    }

    public enum TObjectCategory
    {
        warrior,
        citizen,
        holything,
        guard,
        merchant,
        teleporter,
        guild_master,
        warehouse_keeper,
        ownthing,
        package_keeper,
        boss,
        minion,
        xmastree,
        treasure,
        pc_trap,
        doppelganger,
        collection,
        world_trap,
        guild_coach,
        blacksmith,
        mrkeeper,
        monrace,
        siege_attacker,
        player,
        pet,
        summon
    }

    public enum TObjectRace
    {
        undead,
        animal,
        dragon,
        fairy,
        etc,
        angel,
        construct,
        elemental,
        demonic,
        siege_weapon,
        humanoid,
        bug,
        human,
        beast,
        plant,
        giant,
        elf,
        orc,
        dwarf,
        darkelf,
        castle_guard,
        mercenary,
        creature,
        divine,
        kamael
    }

    public enum TObjectSex
    {
        male,
        female,
        etc
    }

    public enum TObjectBaseAttackType
    {
        fist,
        pole,
        sword,
        dagger,
        bow,
        blunt,
        dual,
        dualfist
    }
}