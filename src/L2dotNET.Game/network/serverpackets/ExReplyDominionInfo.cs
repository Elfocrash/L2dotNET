using System.Collections.Generic;

namespace L2dotNET.GameService.network.l2send
{
    class ExReplyDominionInfo : GameServerNetworkPacket
    {
        private readonly List<string> names = new List<string>();
        public ExReplyDominionInfo()
        {
            names.Add("gludio");
            names.Add("dion");
            names.Add("giran");
            names.Add("oren");
            names.Add("aden");
            names.Add("innadril");
            names.Add("goddard");
            names.Add("rune");
            names.Add("schuttgart");
        }
        protected internal override void write()
        {
            writeC(0xfe);
            writeH(0x92);
            writeD(names.Count);

            byte x = 81;
            foreach (string str in names)
            {
                writeD(x); // Territory Id
			    writeS(str + "_dominion"); // territory name
			    writeS("");
			    writeD(0); // Emblem Count
			  //  for(int i:t.getOwnedWardIds())
				//    writeD(i); // Emblem ID - should be in for loop for emblem count
			    writeD(0);

                x++;
            }
        }
    }
}
