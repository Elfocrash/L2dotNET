using L2dotNET.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Scructs.Services
{
    /// <summary>
    /// Services communication initialization response.
    /// </summary>
    public struct InitializeResponse
    {
        /// <summary>
        /// Initialization rejection flag.
        /// </summary>
        public const byte Rejected = 0x00;

        /// <summary>
        /// Initialization acceptance flag.
        /// </summary>
        public const byte Accepted = 0x01;

        /// <summary>
        /// Packet representation opcodes
        /// </summary>
        public static readonly byte[] Opcodes = new byte[]
        {
            ServiceLayer.Identity,
            ServiceLayer.InitializeResponse
        };

        /// <summary>
        /// Remote service unique id.
        /// </summary>
        public readonly byte RemoteServiceID;

        /// <summary>
        /// Remote service <see cref="ServiceType"/>.
        /// </summary>
        public readonly byte RemoteServiceType;

        /// <summary>
        /// 1 if remote service accepted current connections, otherwise 0.
        /// </summary>
        public readonly byte Answer;

        /// <summary>
        /// Initializes new instance of <see cref="InitializeResponse"/> struct.
        /// </summary>
        /// <param name="answeringServiceID">Answering service unique id.</param>
        /// <param name="answeringServiceType">Answering <see cref="ServiceType"/>.</param>
        /// <param name="answer">"Answer" : 0 - rejected, 1 - accepted.</param>
        public InitializeResponse(byte answeringServiceID, byte answeringServiceType, byte answer)
        {
            RemoteServiceID = answeringServiceID;
            RemoteServiceType = answeringServiceType;
            Answer = answer;
        }

        /// <summary>
        /// Initializes new instance of <see cref="InitializeResponse"/> struct.
        /// </summary>
        /// <param name="data"><see cref="Packet"/> to initialize from.</param>
        public InitializeResponse(Packet data)
        {
            RemoteServiceID = data.ReadByte();
            RemoteServiceType = data.ReadByte();
            Answer = data.ReadByte();
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
                    RemoteServiceID,
                    RemoteServiceType,
                    Answer
                );

            return p;
        }
    }
}
