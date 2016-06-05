namespace L2dotNET.Utility
{
    /// <summary>
    /// Logger source.
    /// </summary>
    public enum Source
    {
        /// <summary>
        /// Source unspecified.
        /// </summary>
        Unspecified,

        /// <summary>
        /// Source is logger.
        /// </summary>
        Logger,

        /// <summary>
        /// Source is current service.
        /// </summary>
        Service,

        /// <summary>
        /// Source is related to inner network.
        /// </summary>
        InnerNetwork,

        /// <summary>
        /// Source is related to outer network.
        /// </summary>
        OuterNetwork,

        /// <summary>
        /// Source is <see cref="L2.Net.Network.Services.Firewall"/> object.
        /// </summary>
        Firewall,
        /// <summary>
        /// <see cref="L2.Net.Network.Services.Listener"/> class.
        /// </summary>
        Listener,
        /// <summary>
        /// Debug message.
        /// </summary>
        Debug,
        /// <summary>
        /// Data provider message.
        /// </summary>
        DataProvider,
        /// <summary>
        /// Data provider in shadow mode.
        /// </summary>
        DataProviderShadow,
        /// <summary>
        /// Geodata engine message.
        /// </summary>
        Geodata,
        /// <summary>
        /// Geodata engine message in shadow mode.
        /// </summary>
        GeodataShadow,
        /// <summary>
        /// World message.
        /// </summary>
        World,
        /// <summary>
        /// World message in shadow mode.
        /// </summary>
        WorldShadow,
        /// <summary>
        /// Scripts compiler message.
        /// </summary>
        ScriptsCompiler
    }
}