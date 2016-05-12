using L2dotNET.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Scructs.Services
{
    /// <summary>
    /// Set remote service settings request.
    /// </summary>
    public struct SetSettingsRequest
    {
        /// <summary>
        /// Packet representation opcodes.
        /// </summary>
        public static readonly byte[] Opcodes =
        {
            ServiceLayer.Identity,
            ServiceLayer.SetSettingsRequest
        };

        /// <summary>
        /// Creates <see cref="Packet"/> that contains provided <see cref="RemoteServiceSettings"/> data.
        /// </summary>
        /// <param name="settings"><see cref="RemoteServiceSettings"/> to create <see cref="Packet"/> from.</param>
        /// <returns><see cref="Packet"/> that contains provided <see cref="RemoteServiceSettings"/> data.</returns>
        public Packet ToPacket(RemoteServiceSettings settings)
        {
            Packet p = new Packet(Opcodes);
            settings.Write(ref p);
            return p;
        }

        /// <summary>
        /// Reads <see cref="RemoteServiceSettings"/> from provided <see cref="Packet"/> struct.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to read <see cref="RemoteServiceSettings"/> from.</param>
        /// <param name="t">Remote <see cref="ServiceType"/>.</param>
        /// <returns>><see cref="RemoteServiceSettings"/> readed from provided <see cref="Packet"/>.</returns>
        public static RemoteServiceSettings FromPacket(Packet p, ServiceType t)
        {
            switch (t)
            {
                case ServiceType.LoginService:
                    return new LoginServiceSettings(p.ReadByte(), p.InternalReadBool(), p.ReadByte());
                default:
                    return null;
            }
        }
    }
}
