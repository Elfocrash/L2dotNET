using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.GameService.Enums
{
    public class ClassId
    {
        private ClassIds _classId;
        public ClassIds Id { get { return _classId; }
            set { _classId = value; } }
        private ClassRace _raceId;
        public ClassRace ClassRace { get { return _raceId; } }

        ClassId(ClassIds classId, ClassRace raceId)
        {
            _classId = classId;
            _raceId = raceId;
        }

        public static IEnumerable<ClassId> Values
        {
            get
            {
                yield return HUMAN_FIGHTER;
                yield return WARRIOR;
                yield return GLADIATOR;
                yield return WARLORD;
                yield return KNIGHT;
                yield return PALADIN;
                yield return DARK_AVENGER;
                yield return ROGUE;
                yield return TREASURE_HUNTER;
                yield return HAWKEYE;

                yield return HUMAN_MYSTIC;
                yield return HUMAN_WIZARD;
                yield return SORCERER;
                yield return NECROMANCER;
                yield return WARLOCK;
                yield return CLERIC;
                yield return BISHOP;
                yield return PROPHET;

                yield return ELVEN_FIGHTER;
                yield return ELVEN_KNIGHT;
                yield return TEMPLE_KNIGHT;
                yield return SWORD_SINGER;
                yield return ELVEN_SCOUT;
                yield return PLAINS_WALKER;
                yield return SILVER_RANGER;

                yield return ELVEN_MYSTIC;
                yield return ELVEN_WIZARD;
                yield return SPELLSINGER;
                yield return ELEMENTAL_SUMMONER;
                yield return ELVEN_ORACLE;
                yield return ELVEN_ELDER;

                yield return DARK_FIGHTER;
                yield return PALUS_KNIGHT;
                yield return SHILLIEN_KNIGHT;
                yield return BLADEDANCER;
                yield return ASSASSIN;
                yield return ABYSS_WALKER;
                yield return PHANTOM_RANGER;

                yield return DARK_MYSTIC;
                yield return DARK_WIZARD;
                yield return SPELLHOWLER;
                yield return PHANTOM_SUMMONER;
                yield return SHILLIEN_ORACLE;
                yield return SHILLIEN_ELDER;

                yield return ORC_FIGHTER;
                yield return ORC_RAIDER;
                yield return DESTROYER;
                yield return MONK;
                yield return TYRANT;

                yield return ORC_MYSTIC;
                yield return ORC_SHAMAN;
                yield return OVERLORD;
                yield return WARCRYER;

                yield return DWARVEN_FIGHTER;
                yield return SCAVENGER;
                yield return BOUNTY_HUNTER;
                yield return ARTISAN;
                yield return WARSMITH;
                yield return DUELIST;
                yield return DREADNOUGHT;
                yield return PHOENIX_KNIGHT;
                yield return HELL_KNIGHT;
                yield return SAGGITARIUS;
                yield return ADVENTURER;
                yield return ARCHMAGE;
                yield return SOULTAKER;
                yield return ARCANA_LORD;
                yield return CARDINAL;
                yield return HIEROPHANT;

                yield return EVAS_TEMPLAR;
                yield return SWORD_MUSE;
                yield return WIND_RIDER;
                yield return MOONLIGHT_SENTINEL;
                yield return MYSTIC_MUSE;
                yield return ELEMENTAL_MASTER;
                yield return EVAS_SAINT;

                yield return SHILLIEN_TEMPLAR;
                yield return SPECTRAL_DANCER;
                yield return GHOST_HUNTER;
                yield return GHOST_SENTINEL;
                yield return STORM_SCREAMER;
                yield return SPECTRAL_MASTER;
                yield return SHILLIEN_SAINT;

                yield return TITAN;
                yield return GRAND_KHAVATARI;
                yield return DOMINATOR;
                yield return DOOMCRYER;

                yield return FORTUNE_SEEKER;
                yield return MAESTRO;
            }
        }

        public static readonly ClassId HUMAN_FIGHTER = new ClassId(ClassIds.HUMAN_FIGHTER, ClassRace.HUMAN);
        public static readonly ClassId WARRIOR = new ClassId(ClassIds.WARRIOR, ClassRace.HUMAN);
        public static readonly ClassId GLADIATOR = new ClassId(ClassIds.GLADIATOR, ClassRace.HUMAN);
        public static readonly ClassId WARLORD = new ClassId(ClassIds.WARLORD, ClassRace.HUMAN);
        public static readonly ClassId KNIGHT = new ClassId(ClassIds.KNIGHT, ClassRace.HUMAN);
        public static readonly ClassId PALADIN = new ClassId(ClassIds.PALADIN, ClassRace.HUMAN);
        public static readonly ClassId DARK_AVENGER = new ClassId(ClassIds.DARK_AVENGER, ClassRace.HUMAN);
        public static readonly ClassId ROGUE = new ClassId(ClassIds.ROGUE, ClassRace.HUMAN);
        public static readonly ClassId TREASURE_HUNTER = new ClassId(ClassIds.TREASURE_HUNTER, ClassRace.HUMAN);
        public static readonly ClassId HAWKEYE = new ClassId(ClassIds.HAWKEYE, ClassRace.HUMAN);

        public static readonly ClassId HUMAN_MYSTIC = new ClassId(ClassIds.HUMAN_MYSTIC, ClassRace.HUMAN);
        public static readonly ClassId HUMAN_WIZARD = new ClassId(ClassIds.HUMAN_WIZARD, ClassRace.HUMAN);
        public static readonly ClassId SORCERER = new ClassId(ClassIds.SORCERER, ClassRace.HUMAN);
        public static readonly ClassId NECROMANCER = new ClassId(ClassIds.NECROMANCER, ClassRace.HUMAN);
        public static readonly ClassId WARLOCK = new ClassId(ClassIds.WARLOCK, ClassRace.HUMAN);
        public static readonly ClassId CLERIC = new ClassId(ClassIds.CLERIC, ClassRace.HUMAN);
        public static readonly ClassId BISHOP = new ClassId(ClassIds.BISHOP, ClassRace.HUMAN);
        public static readonly ClassId PROPHET = new ClassId(ClassIds.PROPHET, ClassRace.HUMAN);

        public static readonly ClassId ELVEN_FIGHTER = new ClassId(ClassIds.ELVEN_FIGHTER, ClassRace.ELF);
        public static readonly ClassId ELVEN_KNIGHT = new ClassId(ClassIds.ELVEN_KNIGHT, ClassRace.ELF);
        public static readonly ClassId TEMPLE_KNIGHT = new ClassId(ClassIds.TEMPLE_KNIGHT, ClassRace.ELF);
        public static readonly ClassId SWORD_SINGER = new ClassId(ClassIds.SWORD_SINGER, ClassRace.ELF);
        public static readonly ClassId ELVEN_SCOUT = new ClassId(ClassIds.ELVEN_SCOUT, ClassRace.ELF);
        public static readonly ClassId PLAINS_WALKER = new ClassId(ClassIds.PLAINS_WALKER, ClassRace.ELF);
        public static readonly ClassId SILVER_RANGER = new ClassId(ClassIds.SILVER_RANGER, ClassRace.ELF);

        public static readonly ClassId ELVEN_MYSTIC = new ClassId(ClassIds.ELVEN_MYSTIC, ClassRace.ELF);
        public static readonly ClassId ELVEN_WIZARD = new ClassId(ClassIds.ELVEN_WIZARD, ClassRace.ELF);
        public static readonly ClassId SPELLSINGER = new ClassId(ClassIds.SPELLSINGER, ClassRace.ELF);
        public static readonly ClassId ELEMENTAL_SUMMONER = new ClassId(ClassIds.ELEMENTAL_SUMMONER, ClassRace.ELF);
        public static readonly ClassId ELVEN_ORACLE = new ClassId(ClassIds.ELVEN_ORACLE, ClassRace.ELF);
        public static readonly ClassId ELVEN_ELDER = new ClassId(ClassIds.ELVEN_ELDER, ClassRace.ELF);

        public static readonly ClassId DARK_FIGHTER = new ClassId(ClassIds.DARK_FIGHTER, ClassRace.DARK_ELF);
        public static readonly ClassId PALUS_KNIGHT = new ClassId(ClassIds.PALUS_KNIGHT, ClassRace.DARK_ELF);
        public static readonly ClassId SHILLIEN_KNIGHT = new ClassId(ClassIds.SHILLIEN_KNIGHT, ClassRace.DARK_ELF);
        public static readonly ClassId BLADEDANCER = new ClassId(ClassIds.BLADEDANCER, ClassRace.DARK_ELF);
        public static readonly ClassId ASSASSIN = new ClassId(ClassIds.ASSASSIN, ClassRace.DARK_ELF);
        public static readonly ClassId ABYSS_WALKER = new ClassId(ClassIds.ABYSS_WALKER, ClassRace.DARK_ELF);
        public static readonly ClassId PHANTOM_RANGER = new ClassId(ClassIds.PHANTOM_RANGER, ClassRace.DARK_ELF);

        public static readonly ClassId DARK_MYSTIC = new ClassId(ClassIds.DARK_MYSTIC, ClassRace.DARK_ELF);
        public static readonly ClassId DARK_WIZARD = new ClassId(ClassIds.DARK_WIZARD, ClassRace.DARK_ELF);
        public static readonly ClassId SPELLHOWLER = new ClassId(ClassIds.SPELLHOWLER, ClassRace.DARK_ELF);
        public static readonly ClassId PHANTOM_SUMMONER = new ClassId(ClassIds.PHANTOM_SUMMONER, ClassRace.DARK_ELF);
        public static readonly ClassId SHILLIEN_ORACLE = new ClassId(ClassIds.SHILLIEN_ORACLE, ClassRace.DARK_ELF);
        public static readonly ClassId SHILLIEN_ELDER = new ClassId(ClassIds.SHILLIEN_ELDER, ClassRace.DARK_ELF);

        public static readonly ClassId ORC_FIGHTER = new ClassId(ClassIds.ORC_FIGHTER, ClassRace.ORC);
        public static readonly ClassId ORC_RAIDER = new ClassId(ClassIds.ORC_RAIDER, ClassRace.ORC);
        public static readonly ClassId DESTROYER = new ClassId(ClassIds.DESTROYER, ClassRace.ORC);
        public static readonly ClassId MONK = new ClassId(ClassIds.MONK, ClassRace.ORC);
        public static readonly ClassId TYRANT = new ClassId(ClassIds.TYRANT, ClassRace.ORC);

        public static readonly ClassId ORC_MYSTIC = new ClassId(ClassIds.ORC_MYSTIC, ClassRace.ORC);
        public static readonly ClassId ORC_SHAMAN = new ClassId(ClassIds.ORC_SHAMAN, ClassRace.ORC);
        public static readonly ClassId OVERLORD = new ClassId(ClassIds.OVERLORD, ClassRace.ORC);
        public static readonly ClassId WARCRYER = new ClassId(ClassIds.WARCRYER, ClassRace.ORC);

        public static readonly ClassId DWARVEN_FIGHTER = new ClassId(ClassIds.DWARVEN_FIGHTER, ClassRace.DWARF);
        public static readonly ClassId SCAVENGER = new ClassId(ClassIds.SCAVENGER, ClassRace.DWARF);
        public static readonly ClassId BOUNTY_HUNTER = new ClassId(ClassIds.BOUNTY_HUNTER, ClassRace.DWARF);
        public static readonly ClassId ARTISAN = new ClassId(ClassIds.ARTISAN, ClassRace.DWARF);
        public static readonly ClassId WARSMITH = new ClassId(ClassIds.WARSMITH, ClassRace.DWARF);
        public static readonly ClassId DUELIST = new ClassId(ClassIds.DUELIST, ClassRace.HUMAN);
        public static readonly ClassId DREADNOUGHT = new ClassId(ClassIds.DREADNOUGHT, ClassRace.HUMAN);
        public static readonly ClassId PHOENIX_KNIGHT = new ClassId(ClassIds.PHOENIX_KNIGHT, ClassRace.HUMAN);
        public static readonly ClassId HELL_KNIGHT = new ClassId(ClassIds.HELL_KNIGHT, ClassRace.HUMAN);
        public static readonly ClassId SAGGITARIUS = new ClassId(ClassIds.SAGGITARIUS, ClassRace.HUMAN);
        public static readonly ClassId ADVENTURER = new ClassId(ClassIds.ADVENTURER, ClassRace.HUMAN);
        public static readonly ClassId ARCHMAGE = new ClassId(ClassIds.ARCHMAGE, ClassRace.HUMAN);
        public static readonly ClassId SOULTAKER = new ClassId(ClassIds.SOULTAKER, ClassRace.HUMAN);
        public static readonly ClassId ARCANA_LORD = new ClassId(ClassIds.ARCANA_LORD, ClassRace.HUMAN);
        public static readonly ClassId CARDINAL = new ClassId(ClassIds.CARDINAL, ClassRace.HUMAN);
        public static readonly ClassId HIEROPHANT = new ClassId(ClassIds.HIEROPHANT, ClassRace.HUMAN);

        public static readonly ClassId EVAS_TEMPLAR = new ClassId(ClassIds.EVAS_TEMPLAR, ClassRace.ELF);
        public static readonly ClassId SWORD_MUSE = new ClassId(ClassIds.SWORD_MUSE, ClassRace.ELF);
        public static readonly ClassId WIND_RIDER = new ClassId(ClassIds.WIND_RIDER, ClassRace.ELF);
        public static readonly ClassId MOONLIGHT_SENTINEL = new ClassId(ClassIds.MOONLIGHT_SENTINEL, ClassRace.ELF);
        public static readonly ClassId MYSTIC_MUSE = new ClassId(ClassIds.MYSTIC_MUSE, ClassRace.ELF);
        public static readonly ClassId ELEMENTAL_MASTER = new ClassId(ClassIds.ELEMENTAL_MASTER, ClassRace.ELF);
        public static readonly ClassId EVAS_SAINT = new ClassId(ClassIds.EVAS_SAINT, ClassRace.ELF);

        public static readonly ClassId SHILLIEN_TEMPLAR = new ClassId(ClassIds.SHILLIEN_TEMPLAR, ClassRace.DARK_ELF);
        public static readonly ClassId SPECTRAL_DANCER = new ClassId(ClassIds.SPECTRAL_DANCER, ClassRace.DARK_ELF);
        public static readonly ClassId GHOST_HUNTER = new ClassId(ClassIds.GHOST_HUNTER, ClassRace.DARK_ELF);
        public static readonly ClassId GHOST_SENTINEL = new ClassId(ClassIds.GHOST_SENTINEL, ClassRace.DARK_ELF);
        public static readonly ClassId STORM_SCREAMER = new ClassId(ClassIds.STORM_SCREAMER, ClassRace.DARK_ELF);
        public static readonly ClassId SPECTRAL_MASTER = new ClassId(ClassIds.SPECTRAL_MASTER, ClassRace.DARK_ELF);
        public static readonly ClassId SHILLIEN_SAINT = new ClassId(ClassIds.SHILLIEN_SAINT, ClassRace.DARK_ELF);

        public static readonly ClassId TITAN = new ClassId(ClassIds.TITAN, ClassRace.ORC);
        public static readonly ClassId GRAND_KHAVATARI = new ClassId(ClassIds.GRAND_KHAVATARI, ClassRace.ORC);
        public static readonly ClassId DOMINATOR = new ClassId(ClassIds.DOMINATOR, ClassRace.ORC);
        public static readonly ClassId DOOMCRYER = new ClassId(ClassIds.DOOMCRYER, ClassRace.ORC);

        public static readonly ClassId FORTUNE_SEEKER = new ClassId(ClassIds.FORTUNE_SEEKER, ClassRace.DWARF);
        public static readonly ClassId MAESTRO = new ClassId(ClassIds.MAESTRO, ClassRace.DWARF);

    }

    public enum ClassIds
    {
        HUMAN_FIGHTER = 0,
        WARRIOR = 1,
        GLADIATOR = 2,
        WARLORD = 3,
        KNIGHT = 4,
        PALADIN = 5,
        DARK_AVENGER = 6,
        ROGUE = 7,
        TREASURE_HUNTER = 8,
        HAWKEYE = 9,

        HUMAN_MYSTIC = 10,
        HUMAN_WIZARD = 11,
        SORCERER = 12,
        NECROMANCER = 13,
        WARLOCK = 14,
        CLERIC = 15,
        BISHOP = 16,
        PROPHET = 17,

        ELVEN_FIGHTER = 18,
        ELVEN_KNIGHT = 19,
        TEMPLE_KNIGHT = 20,
        SWORD_SINGER = 21,
        ELVEN_SCOUT = 22,
        PLAINS_WALKER = 23,
        SILVER_RANGER = 24,

        ELVEN_MYSTIC = 25,
        ELVEN_WIZARD = 26,
        SPELLSINGER = 27,
        ELEMENTAL_SUMMONER = 28,
        ELVEN_ORACLE = 29,
        ELVEN_ELDER = 30,

        DARK_FIGHTER = 31,
        PALUS_KNIGHT = 32,
        SHILLIEN_KNIGHT = 33,
        BLADEDANCER = 34,
        ASSASSIN = 35,
        ABYSS_WALKER = 36,
        PHANTOM_RANGER = 37,

        DARK_MYSTIC = 38,
        DARK_WIZARD = 39,
        SPELLHOWLER = 40,
        PHANTOM_SUMMONER = 41,
        SHILLIEN_ORACLE = 42,
        SHILLIEN_ELDER = 43,

        ORC_FIGHTER = 44,
        ORC_RAIDER = 45,
        DESTROYER = 46,
        MONK = 47,
        TYRANT = 48,

        ORC_MYSTIC = 49,
        ORC_SHAMAN = 50,
        OVERLORD = 51,
        WARCRYER = 52,

        DWARVEN_FIGHTER = 53,
        SCAVENGER = 54,
        BOUNTY_HUNTER = 55,
        ARTISAN = 56,
        WARSMITH = 57,
        DUELIST = 88,
        DREADNOUGHT = 89,
        PHOENIX_KNIGHT = 90,
        HELL_KNIGHT = 91,
        SAGGITARIUS = 92,
        ADVENTURER = 93,
        ARCHMAGE = 94,
        SOULTAKER = 95,
        ARCANA_LORD = 96,
        CARDINAL = 97,
        HIEROPHANT = 98,

        EVAS_TEMPLAR = 99,
        SWORD_MUSE = 100,
        WIND_RIDER = 101,
        MOONLIGHT_SENTINEL = 102,
        MYSTIC_MUSE = 103,
        ELEMENTAL_MASTER = 104,
        EVAS_SAINT = 105,

        SHILLIEN_TEMPLAR = 106,
        SPECTRAL_DANCER = 107,
        GHOST_HUNTER = 108,
        GHOST_SENTINEL = 109,
        STORM_SCREAMER = 110,
        SPECTRAL_MASTER = 111,
        SHILLIEN_SAINT = 112,

        TITAN = 113,
        GRAND_KHAVATARI = 114,
        DOMINATOR = 115,
        DOOMCRYER = 116,

        FORTUNE_SEEKER = 117,
        MAESTRO = 118
    }

}
