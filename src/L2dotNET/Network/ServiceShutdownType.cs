namespace L2dotNET.Network
{
    /// <summary>
    /// Service shutdown types.
    /// .</summary>
    public enum ServiceShutdownType : byte
    {
        /// <summary>
        /// Regular shutdown (normal, without errors).
        /// .</summary>
        Usual,
        /// <summary>
        /// Shutdown on some exception inside service code, service may not operate any more.
        /// .</summary>
        OnCriticalError,
        /// <summary>
        /// Application domain exception occurred, program must be terminated.
        /// .</summary>
        OnAppDomainException
    }
}