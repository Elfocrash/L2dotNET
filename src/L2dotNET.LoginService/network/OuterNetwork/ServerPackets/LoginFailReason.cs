namespace L2dotNET.LoginService.Network.OuterNetwork.ServerPackets
{
    public enum LoginFailReason
    {
        ReasonSystemError = 0x01,
        ReasonPassWrong = 0x02,
        ReasonUserOrPassWrong = 0x03,
        ReasonAccessFailed = 0x04,
        ReasonAccountInUse = 0x07,
        ReasonServerOverloaded = 0x0f,
        ReasonServerMaintenance = 0x10,
        ReasonTempPassExpired = 0x11,
        ReasonDualBox = 0x23
    }
}