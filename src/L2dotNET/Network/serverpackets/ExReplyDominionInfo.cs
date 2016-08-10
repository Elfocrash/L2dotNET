using System.Collections.Generic;

namespace L2dotNET.Network.serverpackets
{
    class ExReplyDominionInfo : GameserverPacket
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

        public override void Write()
        {
            WriteByte(0xfe);
            WriteShort(0x92);
            WriteInt(_names.Count);

            byte x = 81;
            foreach (string str in _names)
            {
                WriteInt(x); // Territory Id
                WriteString($"{str}_dominion"); // territory name
                WriteString(string.Empty);
                WriteInt(0); // Emblem Count
                //  for(int i:t.getOwnedWardIds())
                //    writeD(i); // Emblem ID - should be in for loop for emblem count
                WriteInt(0);

                x++;
            }
        }
    }
}