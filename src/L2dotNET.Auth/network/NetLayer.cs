using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Auth.network
{
    /// <summary>
    /// Network layers.
    /// </summary>
    public enum NetLayer : byte
    {
        /// <summary>
        /// Services communication layer.
        /// </summary>
        Service = ServiceLayer.Identity,

        /// <summary>
        /// User data layer.
        /// </summary>
        UserData = UserDataLayer.Identity
    }

    /// <summary>
    /// Class, that contains service layer opcodes.
    /// </summary>
    public static class ServiceLayer
    {
        /// <summary>
        /// Layer identifier.
        /// </summary>
        public const byte Identity = 0x00;

        /// <summary>
        /// <see cref="L2.Net.Structs.Services.InitializeRequest"/> packet opcode.
        /// </summary>
        public const byte InitializeRequest = 0x00;

        /// <summary>
        /// <see cref="L2.Net.Structs.Services.InitializeResponse"/> packet opcode.
        /// </summary>
        public const byte InitializeResponse = 0x01;

        /// <summary>
        /// <see cref="L2.Net.Structs.Services.SetSettingsRequest"/> packet opcode.
        /// </summary>
        public const byte SetSettingsRequest = 0x03;

        /// <summary>
        /// <see cref="L2.Net.Structs.Services.SetSettingsResponse"/> packet opcode.
        /// </summary>
        public const byte SetSettingsResponse = 0x04;
    }

    /// <summary>
    /// Class, that contains user data layer opcodes.
    /// </summary>
    public static class UserDataLayer
    {
        /// <summary>
        /// Layer identifier.
        /// </summary>
        public const byte Identity = 0x01;

        /// <summary>
        /// <see cref="L2.Net.Structs.Services.UserAuthenticationRequest"/> packet opcode.
        /// </summary>
        public const byte AuthenticateUser = 0x00;

        /// <summary>
        /// <see cref="L2.Net.Structs.Services.UserAuthenticationResponse"/> packet opcode.
        /// </summary>
        public const byte UserAuthenticationResponse = 0x01;

        /// <summary>
        /// <see cref="L2.Net.Structs.Services.CacheUserSessionRequest"/> packet opcode.
        /// </summary>
        public const byte CacheUserSessionRequest = 0x02;

        /// <summary>
        /// <see cref="L2.Net.Structs.Services.CacheUserSessionResponse"/> packet opcode.
        /// </summary>
        public const byte CacheUserSessionResponse = 0x03;

        /// <summary>
        /// <see cref="L2.Net.Structs.Services.UnCacheUser"/> packet opcode.
        /// </summary>
        public const byte UnCacheUser = 0x04;

        /// <summary>
        /// <see cref="L2.Net.Structs.Services.WorldsListRequest"/> packet opcode.
        /// </summary>
        public const byte WorldsListRequest = 0x05;

        /// <summary>
        /// <see cref="L2.Net.Structs.Services.WorldsListResponse"/> packet opcode.
        /// </summary>
        public const byte WorldsListResponse = 0x06;

        /// <summary>
        /// <see cref="L2.Net.Structs.Services.JoinWorldRequest"/> packet opcode.
        /// </summary>
        public const byte JoinWorldRequest = 0x07;

        /// <summary>
        /// <see cref="L2.Net.Structs.Services.JoinWorldResponse"/> packet opcode.
        /// </summary>
        public const byte JoinWorldResponse = 0x08;


    }

    /// <summary>
    /// Class, that contains world-cache packet opcodes.
    /// </summary>
    public static class WorldDataLayer
    {
        /// <summary>
        /// Layer identifier.
        /// </summary>
        public const byte Identity = 0x02;

        /// <summary>
        /// <see cref="L2.Net.Structs.Services.SetWorldActiveRequest"/> packet opcode.
        /// </summary>
        public const byte SetWorldActiveRequest = 0x00;

        /// <summary>
        /// <see cref="L2.Net.Structs.Services.SetWorldActiveResponse"/> packet opcode.
        /// </summary>
        public const byte SetWorldActiveResponse = 0x01;

    }

    /// <summary>
    /// Class, that contains npc data layer opcodes.
    /// </summary>
    public static class NpcDataLayer
    {
        /// <summary>
        /// Layer identifier.
        /// </summary>
        public const byte Identity = 0x03;
    }
}
