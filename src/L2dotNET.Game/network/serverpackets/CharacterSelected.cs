
namespace L2dotNET.Game.network.l2send
{
    class CharacterSelected : GameServerNetworkPacket
    {
        private int session;
        private L2Player player;

        public CharacterSelected(L2Player player, int session)
        {
            this.player = player;
            this.session = session;
        }

        protected internal override void write()
        {
            writeC(0x15);

            writeS(player.Name);
            writeD(player.ObjID); 
            writeS(player.Title);
            writeD(session);

            writeD(player.ClanId);
            writeD(0x00);  //??
            writeD(player.Sex);
            writeD(player.BaseClass.race);

            writeD(player.ActiveClass.id);
            writeD(0x01); // active ??
            writeD(player.X);
            writeD(player.Y);

            writeD(player.Z);
            writeF(player.CurHP);
            writeF(player.CurMP);
            writeD(player.SP);

            writeQ(player.Exp);
            writeD(player.Level);
            writeD(player.Karma);	// thx evill33t
            writeD(0);	//?

            writeD(player.getINT());
            writeD(player.getSTR());
            writeD(player.getCON());
            writeD(player.getMEN());

            writeD(player.getDEX());
            writeD(player.getWIT());
            for (int i = 0; i < 30; i++)
            {
                writeD(0x00);
            }
            writeD(0x00); // c3 work
            writeD(0x00); // c3 work

            writeD(0x00);

            writeD(0x00); // c3

            writeD(player.ActiveClass.id);

            writeD(0x00); // c3 InspectorBin
            writeD(0x00); // c3
            writeD(0x00); // c3
            writeD(0x00); // c3
        }
    }
}
