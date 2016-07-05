using System.Collections.Generic;

namespace L2dotNET.GameService.Enums
{
    public class ClassId
    {
        public ClassIds Id { get; set; }

        public ClassRace ClassRace { get; }

        private ClassId(ClassIds classId, ClassRace raceId)
        {
            Id = classId;
            ClassRace = raceId;
        }

        public static IEnumerable<ClassId> Values
        {
            get
            {
                yield return HumanFighter;
                yield return Warrior;
                yield return Gladiator;
                yield return Warlord;
                yield return Knight;
                yield return Paladin;
                yield return DarkAvenger;
                yield return Rogue;
                yield return TreasureHunter;
                yield return Hawkeye;

                yield return HumanMystic;
                yield return HumanWizard;
                yield return Sorcerer;
                yield return Necromancer;
                yield return Warlock;
                yield return Cleric;
                yield return Bishop;
                yield return Prophet;

                yield return ElvenFighter;
                yield return ElvenKnight;
                yield return TempleKnight;
                yield return SwordSinger;
                yield return ElvenScout;
                yield return PlainsWalker;
                yield return SilverRanger;

                yield return ElvenMystic;
                yield return ElvenWizard;
                yield return Spellsinger;
                yield return ElementalSummoner;
                yield return ElvenOracle;
                yield return ElvenElder;

                yield return DarkFighter;
                yield return PalusKnight;
                yield return ShillienKnight;
                yield return Bladedancer;
                yield return Assassin;
                yield return AbyssWalker;
                yield return PhantomRanger;

                yield return DarkMystic;
                yield return DarkWizard;
                yield return Spellhowler;
                yield return PhantomSummoner;
                yield return ShillienOracle;
                yield return ShillienElder;

                yield return OrcFighter;
                yield return OrcRaider;
                yield return Destroyer;
                yield return Monk;
                yield return Tyrant;

                yield return OrcMystic;
                yield return OrcShaman;
                yield return Overlord;
                yield return Warcryer;

                yield return DwarvenFighter;
                yield return Scavenger;
                yield return BountyHunter;
                yield return Artisan;
                yield return Warsmith;
                yield return Duelist;
                yield return Dreadnought;
                yield return PhoenixKnight;
                yield return HellKnight;
                yield return Saggitarius;
                yield return Adventurer;
                yield return Archmage;
                yield return Soultaker;
                yield return ArcanaLord;
                yield return Cardinal;
                yield return Hierophant;

                yield return EvasTemplar;
                yield return SwordMuse;
                yield return WindRider;
                yield return MoonlightSentinel;
                yield return MysticMuse;
                yield return ElementalMaster;
                yield return EvasSaint;

                yield return ShillienTemplar;
                yield return SpectralDancer;
                yield return GhostHunter;
                yield return GhostSentinel;
                yield return StormScreamer;
                yield return SpectralMaster;
                yield return ShillienSaint;

                yield return Titan;
                yield return GrandKhavatari;
                yield return Dominator;
                yield return Doomcryer;

                yield return FortuneSeeker;
                yield return Maestro;
            }
        }

        public static readonly ClassId HumanFighter = new ClassId(ClassIds.HumanFighter, ClassRace.Human);
        public static readonly ClassId Warrior = new ClassId(ClassIds.Warrior, ClassRace.Human);
        public static readonly ClassId Gladiator = new ClassId(ClassIds.Gladiator, ClassRace.Human);
        public static readonly ClassId Warlord = new ClassId(ClassIds.Warlord, ClassRace.Human);
        public static readonly ClassId Knight = new ClassId(ClassIds.Knight, ClassRace.Human);
        public static readonly ClassId Paladin = new ClassId(ClassIds.Paladin, ClassRace.Human);
        public static readonly ClassId DarkAvenger = new ClassId(ClassIds.DarkAvenger, ClassRace.Human);
        public static readonly ClassId Rogue = new ClassId(ClassIds.Rogue, ClassRace.Human);
        public static readonly ClassId TreasureHunter = new ClassId(ClassIds.TreasureHunter, ClassRace.Human);
        public static readonly ClassId Hawkeye = new ClassId(ClassIds.Hawkeye, ClassRace.Human);

        public static readonly ClassId HumanMystic = new ClassId(ClassIds.HumanMystic, ClassRace.Human);
        public static readonly ClassId HumanWizard = new ClassId(ClassIds.HumanWizard, ClassRace.Human);
        public static readonly ClassId Sorcerer = new ClassId(ClassIds.Sorcerer, ClassRace.Human);
        public static readonly ClassId Necromancer = new ClassId(ClassIds.Necromancer, ClassRace.Human);
        public static readonly ClassId Warlock = new ClassId(ClassIds.Warlock, ClassRace.Human);
        public static readonly ClassId Cleric = new ClassId(ClassIds.Cleric, ClassRace.Human);
        public static readonly ClassId Bishop = new ClassId(ClassIds.Bishop, ClassRace.Human);
        public static readonly ClassId Prophet = new ClassId(ClassIds.Prophet, ClassRace.Human);

        public static readonly ClassId ElvenFighter = new ClassId(ClassIds.ElvenFighter, ClassRace.Elf);
        public static readonly ClassId ElvenKnight = new ClassId(ClassIds.ElvenKnight, ClassRace.Elf);
        public static readonly ClassId TempleKnight = new ClassId(ClassIds.TempleKnight, ClassRace.Elf);
        public static readonly ClassId SwordSinger = new ClassId(ClassIds.SwordSinger, ClassRace.Elf);
        public static readonly ClassId ElvenScout = new ClassId(ClassIds.ElvenScout, ClassRace.Elf);
        public static readonly ClassId PlainsWalker = new ClassId(ClassIds.PlainsWalker, ClassRace.Elf);
        public static readonly ClassId SilverRanger = new ClassId(ClassIds.SilverRanger, ClassRace.Elf);

        public static readonly ClassId ElvenMystic = new ClassId(ClassIds.ElvenMystic, ClassRace.Elf);
        public static readonly ClassId ElvenWizard = new ClassId(ClassIds.ElvenWizard, ClassRace.Elf);
        public static readonly ClassId Spellsinger = new ClassId(ClassIds.Spellsinger, ClassRace.Elf);
        public static readonly ClassId ElementalSummoner = new ClassId(ClassIds.ElementalSummoner, ClassRace.Elf);
        public static readonly ClassId ElvenOracle = new ClassId(ClassIds.ElvenOracle, ClassRace.Elf);
        public static readonly ClassId ElvenElder = new ClassId(ClassIds.ElvenElder, ClassRace.Elf);

        public static readonly ClassId DarkFighter = new ClassId(ClassIds.DarkFighter, ClassRace.DarkElf);
        public static readonly ClassId PalusKnight = new ClassId(ClassIds.PalusKnight, ClassRace.DarkElf);
        public static readonly ClassId ShillienKnight = new ClassId(ClassIds.ShillienKnight, ClassRace.DarkElf);
        public static readonly ClassId Bladedancer = new ClassId(ClassIds.Bladedancer, ClassRace.DarkElf);
        public static readonly ClassId Assassin = new ClassId(ClassIds.Assassin, ClassRace.DarkElf);
        public static readonly ClassId AbyssWalker = new ClassId(ClassIds.AbyssWalker, ClassRace.DarkElf);
        public static readonly ClassId PhantomRanger = new ClassId(ClassIds.PhantomRanger, ClassRace.DarkElf);

        public static readonly ClassId DarkMystic = new ClassId(ClassIds.DarkMystic, ClassRace.DarkElf);
        public static readonly ClassId DarkWizard = new ClassId(ClassIds.DarkWizard, ClassRace.DarkElf);
        public static readonly ClassId Spellhowler = new ClassId(ClassIds.Spellhowler, ClassRace.DarkElf);
        public static readonly ClassId PhantomSummoner = new ClassId(ClassIds.PhantomSummoner, ClassRace.DarkElf);
        public static readonly ClassId ShillienOracle = new ClassId(ClassIds.ShillienOracle, ClassRace.DarkElf);
        public static readonly ClassId ShillienElder = new ClassId(ClassIds.ShillienElder, ClassRace.DarkElf);

        public static readonly ClassId OrcFighter = new ClassId(ClassIds.OrcFighter, ClassRace.Orc);
        public static readonly ClassId OrcRaider = new ClassId(ClassIds.OrcRaider, ClassRace.Orc);
        public static readonly ClassId Destroyer = new ClassId(ClassIds.Destroyer, ClassRace.Orc);
        public static readonly ClassId Monk = new ClassId(ClassIds.Monk, ClassRace.Orc);
        public static readonly ClassId Tyrant = new ClassId(ClassIds.Tyrant, ClassRace.Orc);

        public static readonly ClassId OrcMystic = new ClassId(ClassIds.OrcMystic, ClassRace.Orc);
        public static readonly ClassId OrcShaman = new ClassId(ClassIds.OrcShaman, ClassRace.Orc);
        public static readonly ClassId Overlord = new ClassId(ClassIds.Overlord, ClassRace.Orc);
        public static readonly ClassId Warcryer = new ClassId(ClassIds.Warcryer, ClassRace.Orc);

        public static readonly ClassId DwarvenFighter = new ClassId(ClassIds.DwarvenFighter, ClassRace.Dwarf);
        public static readonly ClassId Scavenger = new ClassId(ClassIds.Scavenger, ClassRace.Dwarf);
        public static readonly ClassId BountyHunter = new ClassId(ClassIds.BountyHunter, ClassRace.Dwarf);
        public static readonly ClassId Artisan = new ClassId(ClassIds.Artisan, ClassRace.Dwarf);
        public static readonly ClassId Warsmith = new ClassId(ClassIds.Warsmith, ClassRace.Dwarf);
        public static readonly ClassId Duelist = new ClassId(ClassIds.Duelist, ClassRace.Human);
        public static readonly ClassId Dreadnought = new ClassId(ClassIds.Dreadnought, ClassRace.Human);
        public static readonly ClassId PhoenixKnight = new ClassId(ClassIds.PhoenixKnight, ClassRace.Human);
        public static readonly ClassId HellKnight = new ClassId(ClassIds.HellKnight, ClassRace.Human);
        public static readonly ClassId Saggitarius = new ClassId(ClassIds.Saggitarius, ClassRace.Human);
        public static readonly ClassId Adventurer = new ClassId(ClassIds.Adventurer, ClassRace.Human);
        public static readonly ClassId Archmage = new ClassId(ClassIds.Archmage, ClassRace.Human);
        public static readonly ClassId Soultaker = new ClassId(ClassIds.Soultaker, ClassRace.Human);
        public static readonly ClassId ArcanaLord = new ClassId(ClassIds.ArcanaLord, ClassRace.Human);
        public static readonly ClassId Cardinal = new ClassId(ClassIds.Cardinal, ClassRace.Human);
        public static readonly ClassId Hierophant = new ClassId(ClassIds.Hierophant, ClassRace.Human);

        public static readonly ClassId EvasTemplar = new ClassId(ClassIds.EvasTemplar, ClassRace.Elf);
        public static readonly ClassId SwordMuse = new ClassId(ClassIds.SwordMuse, ClassRace.Elf);
        public static readonly ClassId WindRider = new ClassId(ClassIds.WindRider, ClassRace.Elf);
        public static readonly ClassId MoonlightSentinel = new ClassId(ClassIds.MoonlightSentinel, ClassRace.Elf);
        public static readonly ClassId MysticMuse = new ClassId(ClassIds.MysticMuse, ClassRace.Elf);
        public static readonly ClassId ElementalMaster = new ClassId(ClassIds.ElementalMaster, ClassRace.Elf);
        public static readonly ClassId EvasSaint = new ClassId(ClassIds.EvasSaint, ClassRace.Elf);

        public static readonly ClassId ShillienTemplar = new ClassId(ClassIds.ShillienTemplar, ClassRace.DarkElf);
        public static readonly ClassId SpectralDancer = new ClassId(ClassIds.SpectralDancer, ClassRace.DarkElf);
        public static readonly ClassId GhostHunter = new ClassId(ClassIds.GhostHunter, ClassRace.DarkElf);
        public static readonly ClassId GhostSentinel = new ClassId(ClassIds.GhostSentinel, ClassRace.DarkElf);
        public static readonly ClassId StormScreamer = new ClassId(ClassIds.StormScreamer, ClassRace.DarkElf);
        public static readonly ClassId SpectralMaster = new ClassId(ClassIds.SpectralMaster, ClassRace.DarkElf);
        public static readonly ClassId ShillienSaint = new ClassId(ClassIds.ShillienSaint, ClassRace.DarkElf);

        public static readonly ClassId Titan = new ClassId(ClassIds.Titan, ClassRace.Orc);
        public static readonly ClassId GrandKhavatari = new ClassId(ClassIds.GrandKhavatari, ClassRace.Orc);
        public static readonly ClassId Dominator = new ClassId(ClassIds.Dominator, ClassRace.Orc);
        public static readonly ClassId Doomcryer = new ClassId(ClassIds.Doomcryer, ClassRace.Orc);

        public static readonly ClassId FortuneSeeker = new ClassId(ClassIds.FortuneSeeker, ClassRace.Dwarf);
        public static readonly ClassId Maestro = new ClassId(ClassIds.Maestro, ClassRace.Dwarf);
    }

    public enum ClassIds
    {
        HumanFighter = 0,
        Warrior = 1,
        Gladiator = 2,
        Warlord = 3,
        Knight = 4,
        Paladin = 5,
        DarkAvenger = 6,
        Rogue = 7,
        TreasureHunter = 8,
        Hawkeye = 9,

        HumanMystic = 10,
        HumanWizard = 11,
        Sorcerer = 12,
        Necromancer = 13,
        Warlock = 14,
        Cleric = 15,
        Bishop = 16,
        Prophet = 17,

        ElvenFighter = 18,
        ElvenKnight = 19,
        TempleKnight = 20,
        SwordSinger = 21,
        ElvenScout = 22,
        PlainsWalker = 23,
        SilverRanger = 24,

        ElvenMystic = 25,
        ElvenWizard = 26,
        Spellsinger = 27,
        ElementalSummoner = 28,
        ElvenOracle = 29,
        ElvenElder = 30,

        DarkFighter = 31,
        PalusKnight = 32,
        ShillienKnight = 33,
        Bladedancer = 34,
        Assassin = 35,
        AbyssWalker = 36,
        PhantomRanger = 37,

        DarkMystic = 38,
        DarkWizard = 39,
        Spellhowler = 40,
        PhantomSummoner = 41,
        ShillienOracle = 42,
        ShillienElder = 43,

        OrcFighter = 44,
        OrcRaider = 45,
        Destroyer = 46,
        Monk = 47,
        Tyrant = 48,

        OrcMystic = 49,
        OrcShaman = 50,
        Overlord = 51,
        Warcryer = 52,

        DwarvenFighter = 53,
        Scavenger = 54,
        BountyHunter = 55,
        Artisan = 56,
        Warsmith = 57,
        Duelist = 88,
        Dreadnought = 89,
        PhoenixKnight = 90,
        HellKnight = 91,
        Saggitarius = 92,
        Adventurer = 93,
        Archmage = 94,
        Soultaker = 95,
        ArcanaLord = 96,
        Cardinal = 97,
        Hierophant = 98,

        EvasTemplar = 99,
        SwordMuse = 100,
        WindRider = 101,
        MoonlightSentinel = 102,
        MysticMuse = 103,
        ElementalMaster = 104,
        EvasSaint = 105,

        ShillienTemplar = 106,
        SpectralDancer = 107,
        GhostHunter = 108,
        GhostSentinel = 109,
        StormScreamer = 110,
        SpectralMaster = 111,
        ShillienSaint = 112,

        Titan = 113,
        GrandKhavatari = 114,
        Dominator = 115,
        Doomcryer = 116,

        FortuneSeeker = 117,
        Maestro = 118
    }
}