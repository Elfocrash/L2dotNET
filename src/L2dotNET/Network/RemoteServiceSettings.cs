namespace L2dotNET.Network
{
    /// <summary>
    /// Base class for remote service settings.
    /// .</summary>
    public class RemoteServiceSettings
    {
        /// <summary>
        /// Remote service unique id.
        /// .</summary>
        public byte ServiceUniqueID;

        /// <summary>
        /// Writes service settings to provided <see cref="Packet"/> struct.
        /// .</summary>
        /// <param name="p"><see cref="Packet"/> to write settings in.</param>
        public virtual void Write(ref Packet p) { }

        /// <summary>
        /// Reads remote service settings from provided <see cref="Packet"/> struct.
        /// .</summary>
        /// <param name="p"><see cref="Packet"/> to read settings from.</param>
        public virtual void Read(Packet p) { }
    }
}