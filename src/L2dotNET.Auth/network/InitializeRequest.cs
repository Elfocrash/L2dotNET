using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Auth.network
{
    /// <summary>
    /// Services communication initialization request.
    /// </summary>
    public struct InitializeRequest
    {
        /// <summary>
        /// Packet representation opcodes.
        /// </summary>
        public static readonly byte[] Opcodes =
        {
            ServiceLayer.Identity,
            ServiceLayer.InitializeRequest
        };

        /// <summary>
        /// Requester service unique id.
        /// </summary>
        public readonly byte ServiceId;

        /// <summary>
        /// Requester <see cref="ServiceType"/>.
        /// </summary>
        public readonly byte ServiceType;

        /// <summary>
        /// Initializes new instance of <see cref="InitializeRequest"/> struct.
        /// </summary>
        /// <param name="serviceId">Requester service unique id.</param>
        /// <param name="type">Requester <see cref="ServiceType"/>.</param>
        public InitializeRequest(byte serviceId, byte type)
        {
            ServiceId = serviceId;
            ServiceType = type;
        }

        /// <summary>
        /// Initializes new instance of <see cref="InitializeRequest"/> struct.
        /// </summary>
        /// <param name="data"><see cref="Packet"/> to initialize from.</param>
        public InitializeRequest(Packet data)
        {
            ServiceId = data.ReadByte();
            ServiceType = data.ReadByte();
        }

        /// <summary>
        /// Converts current struct to it's <see cref="Packet"/> equivalent.
        /// </summary>
        /// <returns><see cref="Packet"/> equivalent of current struct.</returns>
        public Packet ToPacket()
        {
            Packet p = new Packet(Opcodes);

            p.WriteByte
                (
                    ServiceId,
                    ServiceType
                );

            return p;
        }
    }
}
