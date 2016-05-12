using L2dotNET.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Scructs.Services
{
    /// <summary>
    /// Set remote service settings response.
    /// </summary>
    public struct SetSettingsResponse
    {
        /// <summary>
        /// Remote service settings accepted indicator.
        /// </summary>
        public const byte Rejected = 0x00;

        /// <summary>
        /// Remote service settings rejected indicator. 
        /// </summary>
        public const byte Accepted = 0x01;

        /// <summary>
        /// Packet representation opcodes.
        /// </summary>
        public static readonly byte[] Opcodes =
        {
            ServiceLayer.Identity,
            ServiceLayer.SetSettingsResponse
        };

        /// <summary>
        /// Settings acceptance response.
        /// </summary>
        public readonly byte Response;

        /// <summary>
        /// Initializes new instance of <see cref="SetSettingsResponse"/> struct.
        /// </summary>
        /// <param name="response">Settings acceptance response.</param>
        public SetSettingsResponse(byte response)
        {
            Response = response;
        }

        /// <summary>
        /// Initializes new instance of <see cref="SetSettingsResponse"/> struct.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to initialize from.</param>
        public SetSettingsResponse(Packet p)
        {
            Response = p.ReadByte();
        }

        /// <summary>
        /// Converts current struct to it's <see cref="Packet"/> equivalent.
        /// </summary>
        /// <returns><see cref="Packet"/> equivalent of current struct.</returns>
        public Packet ToPacket()
        {
            Packet p = new Packet(Opcodes);

            p.WriteByte(Response);

            return p;
        }
    }
}
