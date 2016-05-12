using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.LoginService.Network.OuterNetwork
{
    class PlayFail : SendBasePacket
    {
        public enum PlayFailReason
        {
            REASON_SYSTEM_ERROR = 0x01,
            REASON_USER_OR_PASS_WRONG = 0x02,
            REASON3 = 0x03,
            REASON4 = 0x04,
            REASON_TOO_MANY_PLAYERS = 0x0f
        }
        PlayFailReason _reason;
        public PlayFail(LoginClient Client, PlayFailReason reason)
        {
            base.makeme(Client);
            _reason = reason;
        }

        protected internal override void write()
        {
            writeC(0x06);
            writeD((byte)_reason);
        }
    }
}
