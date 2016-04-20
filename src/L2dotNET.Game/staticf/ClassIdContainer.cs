using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace L2dotNET.Game.staticf
{
    public static class ClassIdContainer
    {
        public static SortedList<string, pc_parameter> parameters = new SortedList<string, pc_parameter>();

        public static List<pc_parameter> basics = new List<pc_parameter>();

        public static void init()
        {
            StreamReader reader = new StreamReader(new FileInfo(@"scripts\pc_parameter.txt").FullName);
            string current = "base_physical_attack_begin";
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (line.Length == 0)
                    continue;

                if (line.StartsWith("//"))
                    continue;

                line = line.Replace("\t", "");

                bool skip = false;
                switch (line)
                {
                    case "base_race_id_begin": current = line.Replace("_begin", ""); skip = true; break;
                    case "base_class_id_begin": current = line.Replace("_begin", ""); skip = true; break;
                    case "base_physical_attack_begin": current = line.Replace("_begin", ""); skip = true; break;
                    case "base_critical_begin": current = line.Replace("_begin", ""); skip = true; break;
                    case "base_attack_type_begin": current = line.Replace("_begin", ""); skip = true; break;
                    case "base_attack_speed_begin": current = line.Replace("_begin", ""); skip = true; break;
                    case "base_defend_begin": current = line.Replace("_begin", ""); skip = true; break;
                    case "base_magic_attack_begin": current = line.Replace("_begin", ""); skip = true; break;
                    case "base_magic_defend_begin": current = line.Replace("_begin", ""); skip = true; break;
                    case "base_can_penetrate_begin": current = line.Replace("_begin", ""); skip = true; break;
                    case "base_attack_range_begin": current = line.Replace("_begin", ""); skip = true; break;
                    case "base_damage_range_begin": current = line.Replace("_begin", ""); skip = true; break;
                    case "base_rand_dam_begin": current = line.Replace("_begin", ""); skip = true; break;
                    case "org_hp_regen_begin": current = line.Replace("_begin", ""); skip = true; break;
                    case "org_mp_regen_begin": current = line.Replace("_begin", ""); skip = true; break;
                    case "moving_speed_begin": current = line.Replace("_begin", ""); skip = true; break;
                }

                if (skip)
                {
                    continue;
                }

                if (current != null && !line.StartsWith(current + "_end"))
                {
                    string cl = line.Split('=')[0];

                    if(!parameters.ContainsKey(cl))
                    {
                        pc_parameter pcp = new pc_parameter();
                        pcp.classn = cl;
                        pcp.type(current, line.Split('=')[1]);

                        parameters.Add(cl, pcp);
                        continue;
                    }

                    parameters[cl].type(current, line.Split('=')[1]);
                }
                else
                {
                    current = null;
                }
            }

            basics.Add(parameters["MFighter"]);
            basics.Add(parameters["MMagic"]);
            basics.Add(parameters["MElfFighter"]);
            basics.Add(parameters["MElfMagic"]);
            basics.Add(parameters["MDarkelfFighter"]);
            basics.Add(parameters["MDarkelfMagic"]);
            basics.Add(parameters["MOrcFighter"]);
            basics.Add(parameters["MShaman"]);
            basics.Add(parameters["MDwarfFighter"]);

            Console.WriteLine("classid: loaded " + parameters.Count + " player templates.");
            Console.WriteLine("classid: stored " + basics.Count + " basic player templates.");
        }
    }

    public enum ClassId
    {
        fighter,
        warrior,
        knight,
        rogue,
        warlord,
        gladiator,
        paladin,
        dark_avenger,
        treasure_hunter,
        hawkeye,

        mage,
        wizard,
        cleric,
        sorcerer,
        necromancer,
        warlock,
        bishop,
        prophet,

        elven_fighter,
        elven_knight,
        elven_scout,
        temple_knight,
        swordsinger,
        plain_walker,
        silver_ranger,

        elven_mage,
        elven_wizard,
        oracle,
        spellsinger,
        elemental_summoner,
        elder,

        dark_fighter,
        palus_knight,
        assasin,
        shillien_knight,
        bladedancer,
        abyss_walker,
        phantom_ranger,
        dark_mage,
        dark_wizard,
        shillien_oracle,
        spellhowler,
        phantom_summoner,
        shillien_elder,

        orc_fighter,
        orc_raider,
        orc_monk,
        destroyer,
        tyrant,
        orc_mage,
        orc_shaman,
        overlord,
        warcryer,

        dwarven_fighter,
        scavenger,
        artisan,
        bounty_hunter,
        warsmith,

        dreadnought,
        duelist,
        phoenix_knight,
        hell_knight,
        adventurer,
        sagittarius,
        archmage,
        soultaker,
        arcana_lord,
        cardinal,
        hierophant,

        evas_templar,
        sword_muse,
        wind_rider,
        moonlight_sentinel,
        mystic_muse,
        elemental_master,
        evas_saint,

        shillien_templar,
        spectral_dancer,
        ghost_hunter,
        ghost_sentinel,
        storm_screamer,
        spectral_master,
        shillien_saint,

        titan,
        grand_khavatari,
        dominator,
        doomcryer,

        fortune_seeker,
        maestro
    };

    public enum JobLevel
    {
        newbie, first, second, third
    }
}
