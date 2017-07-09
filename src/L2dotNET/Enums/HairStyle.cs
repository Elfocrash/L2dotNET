using System.Collections.Generic;
using System.Linq;
using L2dotNET.Utility;

namespace L2dotNET.Enums
{
    public class HairStyle
    {
        public HairStyleId Id { get; }

        public Gender[] Sex { get; }

        private HairStyle(HairStyleId classId, Gender[] sex)
        {
            Id = classId;
            Sex = sex;
        }

        public static IEnumerable<HairStyle> Values
        {
            get
            {
                yield return TypeA;
                yield return TypeB;
                yield return TypeC;
                yield return TypeD;
                yield return TypeE;
                yield return TypeF;
                yield return TypeG;
            }
        }

        public static readonly HairStyle TypeA = new HairStyle(HairStyleId.TypeA, new[] { Gender.Male, Gender.Female });
        public static readonly HairStyle TypeB = new HairStyle(HairStyleId.TypeB, new[] { Gender.Male, Gender.Female });
        public static readonly HairStyle TypeC = new HairStyle(HairStyleId.TypeC, new[] { Gender.Male, Gender.Female });
        public static readonly HairStyle TypeD = new HairStyle(HairStyleId.TypeD, new[] { Gender.Male, Gender.Female });
        public static readonly HairStyle TypeE = new HairStyle(HairStyleId.TypeE, new[] { Gender.Male, Gender.Female });
        public static readonly HairStyle TypeF = new HairStyle(HairStyleId.TypeF, new[] { Gender.Female });
        public static readonly HairStyle TypeG = new HairStyle(HairStyleId.TypeG, new[] { Gender.Female });

        public override string ToString()
        {
            return $"Id: {(int)Id}, Type: {Id.GetDescription()}, Genders: {string.Join(", ", Sex.Select(select => select.GetDescription()).ToArray())}";
        }
    }
}