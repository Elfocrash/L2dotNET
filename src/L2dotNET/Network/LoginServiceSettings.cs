namespace L2dotNET.Network
{
    /// <summary>
    /// Remote login service settings.
    /// </summary>
    public sealed class LoginServiceSettings : RemoteServiceSettings
    {
        /// <summary>
        /// Indicates if cache service can create non-existent user accounts automatically.
        /// </summary>
        public bool AutoCreateUser;

        /// <summary>
        /// Access level, given to newer created users by default.
        /// </summary>
        public byte DefaultAccessLevel;

        /// <summary>
        /// Initializes new instance of <see cref="LoginServiceSettings"/> class.
        /// </summary>
        /// <param name="serviceId">Service unique id.</param>
        /// <param name="autoCreateAccounts">True, if cache server may create users automatically.</param>
        /// <param name="defaultAccessLevel">Default access level for newer created user.</param>
        public LoginServiceSettings(byte serviceId, bool autoCreateAccounts, byte defaultAccessLevel)
        {
            ServiceUniqueId = serviceId;
            AutoCreateUser = autoCreateAccounts;
            DefaultAccessLevel = defaultAccessLevel;
        }

        /// <summary>
        /// Writes login service settings to provided <see cref="Packet"/>.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to write settings in.</param>
        public override void Write(ref Packet p)
        {
            p.WriteByte(ServiceUniqueId);
            p.InternalWriteBool(AutoCreateUser);
            p.WriteByte(DefaultAccessLevel);
        }

        /// <summary>
        /// Reads login service settings from <see cref="Packet"/>.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to read settings from.</param>
        public override void Read(Packet p)
        {
            ServiceUniqueId = p.ReadByte();
            AutoCreateUser = p.InternalReadBool();
            DefaultAccessLevel = p.ReadByte();
        }
    }
}