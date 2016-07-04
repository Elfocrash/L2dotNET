using System.Collections.Generic;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class ExReplyDominionInfo : GameServerNetworkPacket
    {
        private readonly List<string> _names = new List<string>();

        public ExReplyDominionInfo()
        {
            _names.Add("gludio");
            _names.Add("dion");
            _names.Add("giran");
            _names.Add("oren");
            _names.Add("aden");
            _names.Add("innadril");
            _names.Add("goddard");
            _names.Add("rune");
            _names.Add("schuttgart");
        }

        protected internal override void Write()
        {
            WriteC(0xfe);
            WriteH(0x92);
            WriteD(_names.Count);

            byte x = 81;
            foreach (string str in _names)
            {
                WriteD(x); // Territory Id
                WriteS(str + "_dominion"); // territory name
                WriteS("");
                WriteD(0); // Emblem Count
                //  for(int i:t.getOwnedWardIds())
                //    writeD(i); // Emblem ID - should be in for loop for emblem count
                WriteD(0);

                x++;
            }
        }
    }
}