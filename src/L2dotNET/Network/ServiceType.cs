namespace L2dotNET.Network
{
    /// <summary>
    /// Services types.
    /// </summary>
    public enum ServiceType : byte
    {
        /// <summary>
        /// Undefined service type.
        /// </summary>
        Undefined = 0x00,
        /// <summary>
        /// Login service type.
        /// </summary>
        LoginService = 0x01,
        /// <summary>
        /// Cache service type.
        /// </summary>
        CacheService = 0x02,
        /// <summary>
        /// Game service type.
        /// </summary>
        GameService = 0x03,
        /// <summary>
        /// Npc service type.
        /// </summary>
        NpcService = 0x04
    }
}