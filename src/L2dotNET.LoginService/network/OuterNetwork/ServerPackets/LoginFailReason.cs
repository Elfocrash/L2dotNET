namespace L2dotNET.LoginService.Network.OuterNetwork.ServerPackets
{
    public enum LoginFailReason
    {
        REASON_SYSTEM_ERROR = 0x01,
        REASON_PASS_WRONG = 0x02,
        REASON_USER_OR_PASS_WRONG = 0x03,
        REASON_ACCESS_FAILED = 0x04,
        REASON_ACCOUNT_IN_USE = 0x07,
        REASON_SERVER_OVERLOADED = 0x0f,
        REASON_SERVER_MAINTENANCE = 0x10,
        REASON_TEMP_PASS_EXPIRED = 0x11,
        REASON_DUAL_BOX = 0x23
    }
}