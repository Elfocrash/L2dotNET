using System.Collections.Generic;
using L2dotNET.DataContracts.Shared.Enums;
using L2dotNET.Utility;

namespace L2dotNET.Enums
{
    public class ClassId
    {
        public ClassIds Id { get; set; }

        public ClassRace ClassRace { get; }

        public ClassId Parent { get; }

        private ClassId(ClassIds classId, ClassRace raceId, ClassId parent)
        {
            Id = classId;
            ClassRace = raceId;
            Parent = parent;
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

        public static readonly ClassId HumanFighter = new ClassId(ClassIds.HumanFighter, ClassRace.Human, null);
        public static readonly ClassId Warrior = new ClassId(ClassIds.Warrior, ClassRace.Human, HumanFighter);
        public static readonly ClassId Gladiator = new ClassId(ClassIds.Gladiator, ClassRace.Human, Warrior);
        public static readonly ClassId Warlord = new ClassId(ClassIds.Warlord, ClassRace.Human, Warrior);
        public static readonly ClassId Knight = new ClassId(ClassIds.Knight, ClassRace.Human, HumanFighter);
        public static readonly ClassId Paladin = new ClassId(ClassIds.Paladin, ClassRace.Human, Knight);
        public static readonly ClassId DarkAvenger = new ClassId(ClassIds.DarkAvenger, ClassRace.Human, Knight);
        public static readonly ClassId Rogue = new ClassId(ClassIds.Rogue, ClassRace.Human, HumanFighter);
        public static readonly ClassId TreasureHunter = new ClassId(ClassIds.TreasureHunter, ClassRace.Human, Rogue);
        public static readonly ClassId Hawkeye = new ClassId(ClassIds.Hawkeye, ClassRace.Human, Rogue);

        public static readonly ClassId HumanMystic = new ClassId(ClassIds.HumanMystic, ClassRace.Human, null);
        public static readonly ClassId HumanWizard = new ClassId(ClassIds.HumanWizard, ClassRace.Human, HumanMystic);
        public static readonly ClassId Sorcerer = new ClassId(ClassIds.Sorcerer, ClassRace.Human, HumanWizard);
        public static readonly ClassId Necromancer = new ClassId(ClassIds.Necromancer, ClassRace.Human, HumanWizard);
        public static readonly ClassId Warlock = new ClassId(ClassIds.Warlock, ClassRace.Human, HumanWizard);
        public static readonly ClassId Cleric = new ClassId(ClassIds.Cleric, ClassRace.Human, HumanMystic);
        public static readonly ClassId Bishop = new ClassId(ClassIds.Bishop, ClassRace.Human, Cleric);
        public static readonly ClassId Prophet = new ClassId(ClassIds.Prophet, ClassRace.Human, Cleric);

        public static readonly ClassId ElvenFighter = new ClassId(ClassIds.ElvenFighter, ClassRace.Elf, null);
        public static readonly ClassId ElvenKnight = new ClassId(ClassIds.ElvenKnight, ClassRace.Elf, ElvenFighter);
        public static readonly ClassId TempleKnight = new ClassId(ClassIds.TempleKnight, ClassRace.Elf, ElvenKnight);
        public static readonly ClassId SwordSinger = new ClassId(ClassIds.SwordSinger, ClassRace.Elf, ElvenKnight);
        public static readonly ClassId ElvenScout = new ClassId(ClassIds.ElvenScout, ClassRace.Elf, ElvenFighter);
        public static readonly ClassId PlainsWalker = new ClassId(ClassIds.PlainsWalker, ClassRace.Elf, ElvenScout);
        public static readonly ClassId SilverRanger = new ClassId(ClassIds.SilverRanger, ClassRace.Elf, ElvenScout);

        public static readonly ClassId ElvenMystic = new ClassId(ClassIds.ElvenMystic, ClassRace.Elf, null);
        public static readonly ClassId ElvenWizard = new ClassId(ClassIds.ElvenWizard, ClassRace.Elf, ElvenMystic);
        public static readonly ClassId Spellsinger = new ClassId(ClassIds.Spellsinger, ClassRace.Elf, ElvenWizard);
        public static readonly ClassId ElementalSummoner = new ClassId(ClassIds.ElementalSummoner, ClassRace.Elf, ElvenWizard);
        public static readonly ClassId ElvenOracle = new ClassId(ClassIds.ElvenOracle, ClassRace.Elf, ElvenMystic);
        public static readonly ClassId ElvenElder = new ClassId(ClassIds.ElvenElder, ClassRace.Elf, ElvenOracle);

        public static readonly ClassId DarkFighter = new ClassId(ClassIds.DarkFighter, ClassRace.DarkElf, null);
        public static readonly ClassId PalusKnight = new ClassId(ClassIds.PalusKnight, ClassRace.DarkElf, DarkFighter);
        public static readonly ClassId ShillienKnight = new ClassId(ClassIds.ShillienKnight, ClassRace.DarkElf, PalusKnight);
        public static readonly ClassId Bladedancer = new ClassId(ClassIds.Bladedancer, ClassRace.DarkElf, PalusKnight);
        public static readonly ClassId Assassin = new ClassId(ClassIds.Assassin, ClassRace.DarkElf, DarkFighter);
        public static readonly ClassId AbyssWalker = new ClassId(ClassIds.AbyssWalker, ClassRace.DarkElf, Assassin);
        public static readonly ClassId PhantomRanger = new ClassId(ClassIds.PhantomRanger, ClassRace.DarkElf, Assassin);

        public static readonly ClassId DarkMystic = new ClassId(ClassIds.DarkMystic, ClassRace.DarkElf, null);
        public static readonly ClassId DarkWizard = new ClassId(ClassIds.DarkWizard, ClassRace.DarkElf, DarkMystic);
        public static readonly ClassId Spellhowler = new ClassId(ClassIds.Spellhowler, ClassRace.DarkElf, DarkWizard);
        public static readonly ClassId PhantomSummoner = new ClassId(ClassIds.PhantomSummoner, ClassRace.DarkElf, DarkWizard);
        public static readonly ClassId ShillienOracle = new ClassId(ClassIds.ShillienOracle, ClassRace.DarkElf, DarkMystic);
        public static readonly ClassId ShillienElder = new ClassId(ClassIds.ShillienElder, ClassRace.DarkElf, ShillienOracle);

        public static readonly ClassId OrcFighter = new ClassId(ClassIds.OrcFighter, ClassRace.Orc, null);
        public static readonly ClassId OrcRaider = new ClassId(ClassIds.OrcRaider, ClassRace.Orc, OrcFighter);
        public static readonly ClassId Destroyer = new ClassId(ClassIds.Destroyer, ClassRace.Orc, OrcRaider);
        public static readonly ClassId Monk = new ClassId(ClassIds.Monk, ClassRace.Orc, OrcFighter);
        public static readonly ClassId Tyrant = new ClassId(ClassIds.Tyrant, ClassRace.Orc, Monk);

        public static readonly ClassId OrcMystic = new ClassId(ClassIds.OrcMystic, ClassRace.Orc, null);
        public static readonly ClassId OrcShaman = new ClassId(ClassIds.OrcShaman, ClassRace.Orc, OrcMystic);
        public static readonly ClassId Overlord = new ClassId(ClassIds.Overlord, ClassRace.Orc, OrcShaman);
        public static readonly ClassId Warcryer = new ClassId(ClassIds.Warcryer, ClassRace.Orc, OrcShaman);

        public static readonly ClassId DwarvenFighter = new ClassId(ClassIds.DwarvenFighter, ClassRace.Dwarf, null);
        public static readonly ClassId Scavenger = new ClassId(ClassIds.Scavenger, ClassRace.Dwarf, DwarvenFighter);
        public static readonly ClassId BountyHunter = new ClassId(ClassIds.BountyHunter, ClassRace.Dwarf, Scavenger);
        public static readonly ClassId Artisan = new ClassId(ClassIds.Artisan, ClassRace.Dwarf, DwarvenFighter);
        public static readonly ClassId Warsmith = new ClassId(ClassIds.Warsmith, ClassRace.Dwarf, Artisan);

        public static readonly ClassId Duelist = new ClassId(ClassIds.Duelist, ClassRace.Human, Gladiator);
        public static readonly ClassId Dreadnought = new ClassId(ClassIds.Dreadnought, ClassRace.Human, Warlord);
        public static readonly ClassId PhoenixKnight = new ClassId(ClassIds.PhoenixKnight, ClassRace.Human, Paladin);
        public static readonly ClassId HellKnight = new ClassId(ClassIds.HellKnight, ClassRace.Human, DarkAvenger);
        public static readonly ClassId Saggitarius = new ClassId(ClassIds.Saggitarius, ClassRace.Human, Hawkeye);
        public static readonly ClassId Adventurer = new ClassId(ClassIds.Adventurer, ClassRace.Human, TreasureHunter);
        public static readonly ClassId Archmage = new ClassId(ClassIds.Archmage, ClassRace.Human, Sorcerer);
        public static readonly ClassId Soultaker = new ClassId(ClassIds.Soultaker, ClassRace.Human, Necromancer);
        public static readonly ClassId ArcanaLord = new ClassId(ClassIds.ArcanaLord, ClassRace.Human, Warlock);
        public static readonly ClassId Cardinal = new ClassId(ClassIds.Cardinal, ClassRace.Human, Bishop);
        public static readonly ClassId Hierophant = new ClassId(ClassIds.Hierophant, ClassRace.Human, Prophet);

        public static readonly ClassId EvasTemplar = new ClassId(ClassIds.EvasTemplar, ClassRace.Elf, TempleKnight);
        public static readonly ClassId SwordMuse = new ClassId(ClassIds.SwordMuse, ClassRace.Elf, SwordSinger);
        public static readonly ClassId WindRider = new ClassId(ClassIds.WindRider, ClassRace.Elf, PlainsWalker);
        public static readonly ClassId MoonlightSentinel = new ClassId(ClassIds.MoonlightSentinel, ClassRace.Elf, SilverRanger);
        public static readonly ClassId MysticMuse = new ClassId(ClassIds.MysticMuse, ClassRace.Elf, Spellsinger);
        public static readonly ClassId ElementalMaster = new ClassId(ClassIds.ElementalMaster, ClassRace.Elf, ElementalSummoner);
        public static readonly ClassId EvasSaint = new ClassId(ClassIds.EvasSaint, ClassRace.Elf, ElvenElder);

        public static readonly ClassId ShillienTemplar = new ClassId(ClassIds.ShillienTemplar, ClassRace.DarkElf, ShillienKnight);
        public static readonly ClassId SpectralDancer = new ClassId(ClassIds.SpectralDancer, ClassRace.DarkElf, Bladedancer);
        public static readonly ClassId GhostHunter = new ClassId(ClassIds.GhostHunter, ClassRace.DarkElf, AbyssWalker);
        public static readonly ClassId GhostSentinel = new ClassId(ClassIds.GhostSentinel, ClassRace.DarkElf, PhantomRanger);
        public static readonly ClassId StormScreamer = new ClassId(ClassIds.StormScreamer, ClassRace.DarkElf, Spellhowler);
        public static readonly ClassId SpectralMaster = new ClassId(ClassIds.SpectralMaster, ClassRace.DarkElf, PhantomSummoner);
        public static readonly ClassId ShillienSaint = new ClassId(ClassIds.ShillienSaint, ClassRace.DarkElf, ShillienElder);

        public static readonly ClassId Titan = new ClassId(ClassIds.Titan, ClassRace.Orc, Destroyer);
        public static readonly ClassId GrandKhavatari = new ClassId(ClassIds.GrandKhavatari, ClassRace.Orc, Tyrant);
        public static readonly ClassId Dominator = new ClassId(ClassIds.Dominator, ClassRace.Orc, Overlord);
        public static readonly ClassId Doomcryer = new ClassId(ClassIds.Doomcryer, ClassRace.Orc, Warcryer);

        public static readonly ClassId FortuneSeeker = new ClassId(ClassIds.FortuneSeeker, ClassRace.Dwarf, BountyHunter);
        public static readonly ClassId Maestro = new ClassId(ClassIds.Maestro, ClassRace.Dwarf, Warsmith);

        public int Level()
        {
            if (Parent == null)
                return 0;

            return 1 + Parent.Level();
        }

        public override string ToString()
        {
            return $"Id: {((int)Id).ToString().PadLeft(3, ' ')}, Class: {Id.GetDescription()}, Race: {ClassRace.GetDescription()}, Level: {Level()}";
        }
    }
}