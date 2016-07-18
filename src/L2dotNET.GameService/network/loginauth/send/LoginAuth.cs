using L2dotNET.Network;

namespace L2dotNET.GameService.Network.LoginAuth.Send
{
    class LoginAuth : GameserverPacket
    {
        public override void Write()
        {
            WriteByte(0xA1);
            WriteShort(Config.Config.Instance.ServerConfig.Port);
            WriteString(Config.Config.Instance.ServerConfig.Host);
            WriteString("");
            WriteString(Config.Config.Instance.ServerConfig.AuthCode);
            WriteInt(0);
            WriteShort(Config.Config.Instance.ServerConfig.MaxPlayers);
            WriteByte(Config.Config.Instance.ServerConfig.IsGmOnly ? 0x01 : 0x00);
            WriteByte(Config.Config.Instance.ServerConfig.IsTestServer ? 0x01 : 0x00);
        }
    }
}