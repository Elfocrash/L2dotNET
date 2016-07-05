using System.Collections.Generic;

namespace L2dotNET.GameService.Enums
{
    public class CrystalType
    {
        public static readonly CrystalType None = new CrystalType(CrystalTypeId.None, 0, 0, 0);
        public static readonly CrystalType D = new CrystalType(CrystalTypeId.D, 1458, 11, 90);
        public static readonly CrystalType C = new CrystalType(CrystalTypeId.C, 1459, 6, 45);
        public static readonly CrystalType B = new CrystalType(CrystalTypeId.B, 1460, 11, 67);
        public static readonly CrystalType A = new CrystalType(CrystalTypeId.A, 1461, 19, 144);
        public static readonly CrystalType S = new CrystalType(CrystalTypeId.S, 1462, 25, 250);

        public CrystalTypeId Id { get; set; }
        public int CrystalId { get; set; }
        public int CrystalEnchantBonusArmor { get; set; }
        public int CrystalEnchantBonusWeapon { get; set; }

        public CrystalType(CrystalTypeId id, int crystalId, int crystalEnchantBonusArmor, int crystalEnchantBonusWeapon)
        {
            Id = id;
            CrystalId = crystalId;
            CrystalEnchantBonusArmor = crystalEnchantBonusArmor;
            CrystalEnchantBonusWeapon = crystalEnchantBonusWeapon;
        }

        public static IEnumerable<CrystalType> Values
        {
            get
            {
                yield return None;
                yield return D;
                yield return C;
                yield return B;
                yield return A;
                yield return S;
            }
        }
    }

    public enum CrystalTypeId
    {
        None,
        D,
        C,
        B,
        A,
        S
    }
}