using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.network.l2recv
{
    class CharacterSelected : GameServerNetworkRequest
    {
        public CharacterSelected(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

	    private int _charSlot;

	    private int _unk1; 	// new in C4
	    private int _unk2;	// new in C4
	    private int _unk3;	// new in C4
	    private int _unk4;	// new in C4

        public override void read()
        {
            _charSlot = readD();
            _unk1 = readH();
            _unk2 = readD();
            _unk3 = readD();
            _unk4 = readD();
        }

        public override void run()
        {
            L2Player pl = null;
            foreach (L2Player player in getClient()._accountChars)
            {
                if (player._slotId == _charSlot)
                {
                    pl = player;
                    break;
                }
            }

            if (pl == null)
            {
                Console.WriteLine("no char for slot " + _charSlot + "??");
                return;
            }

            getClient().CurrentPlayer = pl;
            getClient().sendPacket(new L2dotNET.Game.network.l2send.CharacterSelected(pl, 0));
        }
    }
}
