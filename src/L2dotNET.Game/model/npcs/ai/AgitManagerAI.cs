namespace L2dotNET.GameService.model.npcs.ai
{
    public class AgitManagerAI
    {
        public string fnHi = "black001.htm";
        public string fnNotMyLord = "black002.htm";
        public string fnGetOut = "black003.htm";
        public string fnOptionList = "black005.htm";
        public string fnAlreadyHaveOption = "black009.htm";
        public string fnNeedLowGrade = "black011.htm";
        public string fnAfterOptionAdd = "black012.htm";
        public string fnAddHPRegen1 = "black013.htm";
        public string fnAddHPRegen2 = "black014.htm";
        public string fnAddMPRegen1 = "black015.htm";
        public string fnAddMPRegen2 = "black016.htm";
        public string fnAddTeleporter1 = "black017.htm";
        public string fnAddTeleporter2 = "black020.htm";
        public string fnMyPledge = "black019.htm";
        public string fnTeleportLevelZero = "black021.htm";
        public string fnWarehouse = "agitwarehouse.htm";
        public string fnFunction = "agitfunction.htm";
        public string fnManage = "agitdecomanage.htm";
        public string fnManageRegen = "agitdeco_ar01.htm";
        public string fnManageEtc = "agitdeco_ae01.htm";
        public string fnManageDeco = "agitdeco_ad01.htm";
        public string fnBanish = "agitbanish.htm";
        public string fnAfterBanish = "agitafterbanish.htm";
        public string fnDoor = "agitdoor.htm";
        public string fnAfterDoorOpen = "agitafterdooropen.htm";
        public string fnAfterDoorClose = "agitafterdoorclose.htm";
        public string fnDecoFunction = "agitdecofunction.htm";
        public string fnAfterSetDeco = "agitaftersetdeco.htm";
        public string fnAfterResetDeco = "agitafterresetdeco.htm";
        public string fnFailtoSetDeco = "agitfailtosetdeco.htm";
        public string fnFailtoResetDeco = "agitfailtoresetdeco.htm";
        public string fnDecoAlreadySet = "agitdecoalreadyset.htm";
        public string fnDecoReset = "agitresetdeco.htm";
        public string fnAgitBuff = "agitbuff";
        public string fnAfterBuff = "agitafterbuff.htm";
        public string fnAfterBuyItem = "agitafterbuyitem.htm";
        public string fnNoAuthority = "noauthority.htm";
        public string fnNotEnoughAdena = "agitnotenoughadena.htm";
        public string fnUnableToSell = "agitunabletosell.htm";
        public string fnFuncDisabled = "agitfuncdisabled.htm";
        public string fnNotEnoughMP = "agitnotenoughmp.htm";
        public string fnNeedCoolTime = "agitneedcooltime.htm";
        public string fnCostFail = "agitcostfail.htm";

        public const int decotype_hpregen = 1;
        public const int decotype_mpregen = 2;
        public const int decotype_cpregen = 3;
        public const int decotype_xprestore = 4;
        public const int decotype_teleport = 5;
        public const int decotype_broadcast = 6;
        public const int decotype_curtain = 7;
        public const int decotype_hanging = 8;
        public const int decotype_buff = 9;
        public const int decotype_outerflag = 10;
        public const int decotype_platform = 11;
        public const int decotype_item = 12;

        public double[] regenMax = new double[] { 0, 5000, 10000, 28000, 58000, 103000, 106000, 151000, 196000 };
        public double[] regenPerSec = new double[] { 9.26, 18.52, 27.78, 61.11, 116.7, 200, 205.6, 288.9, 372.2 };
    }
}