namespace L2dotNET.GameService.Model.Npcs.Ai
{
    public class AgitManagerAi
    {
        public string FnHi = "black001.htm";
        public string FnNotMyLord = "black002.htm";
        public string FnGetOut = "black003.htm";
        public string FnOptionList = "black005.htm";
        public string FnAlreadyHaveOption = "black009.htm";
        public string FnNeedLowGrade = "black011.htm";
        public string FnAfterOptionAdd = "black012.htm";
        public string FnAddHpRegen1 = "black013.htm";
        public string FnAddHpRegen2 = "black014.htm";
        public string FnAddMpRegen1 = "black015.htm";
        public string FnAddMpRegen2 = "black016.htm";
        public string FnAddTeleporter1 = "black017.htm";
        public string FnAddTeleporter2 = "black020.htm";
        public string FnMyPledge = "black019.htm";
        public string FnTeleportLevelZero = "black021.htm";
        public string FnWarehouse = "agitwarehouse.htm";
        public string FnFunction = "agitfunction.htm";
        public string FnManage = "agitdecomanage.htm";
        public string FnManageRegen = "agitdeco_ar01.htm";
        public string FnManageEtc = "agitdeco_ae01.htm";
        public string FnManageDeco = "agitdeco_ad01.htm";
        public string FnBanish = "agitbanish.htm";
        public string FnAfterBanish = "agitafterbanish.htm";
        public string FnDoor = "agitdoor.htm";
        public string FnAfterDoorOpen = "agitafterdooropen.htm";
        public string FnAfterDoorClose = "agitafterdoorclose.htm";
        public string FnDecoFunction = "agitdecofunction.htm";
        public string FnAfterSetDeco = "agitaftersetdeco.htm";
        public string FnAfterResetDeco = "agitafterresetdeco.htm";
        public string FnFailtoSetDeco = "agitfailtosetdeco.htm";
        public string FnFailtoResetDeco = "agitfailtoresetdeco.htm";
        public string FnDecoAlreadySet = "agitdecoalreadyset.htm";
        public string FnDecoReset = "agitresetdeco.htm";
        public string FnAgitBuff = "agitbuff";
        public string FnAfterBuff = "agitafterbuff.htm";
        public string FnAfterBuyItem = "agitafterbuyitem.htm";
        public string FnNoAuthority = "noauthority.htm";
        public string FnNotEnoughAdena = "agitnotenoughadena.htm";
        public string FnUnableToSell = "agitunabletosell.htm";
        public string FnFuncDisabled = "agitfuncdisabled.htm";
        public string FnNotEnoughMp = "agitnotenoughmp.htm";
        public string FnNeedCoolTime = "agitneedcooltime.htm";
        public string FnCostFail = "agitcostfail.htm";

        public const int DecotypeHpregen = 1;
        public const int DecotypeMpregen = 2;
        public const int DecotypeCpregen = 3;
        public const int DecotypeXprestore = 4;
        public const int DecotypeTeleport = 5;
        public const int DecotypeBroadcast = 6;
        public const int DecotypeCurtain = 7;
        public const int DecotypeHanging = 8;
        public const int DecotypeBuff = 9;
        public const int DecotypeOuterflag = 10;
        public const int DecotypePlatform = 11;
        public const int DecotypeItem = 12;

        public double[] RegenMax = { 0, 5000, 10000, 28000, 58000, 103000, 106000, 151000, 196000 };
        public double[] RegenPerSec = { 9.26, 18.52, 27.78, 61.11, 116.7, 200, 205.6, 288.9, 372.2 };
    }
}