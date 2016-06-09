using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class CharacterSelected : GameServerNetworkPacket
    {
        private readonly int session;
        private readonly L2Player player;

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
            writeD(0x00); //??
            writeD(player.Sex);
            writeD((int)player.BaseClass.ClassId.ClassRace);

            writeD((int)player.ActiveClass.ClassId.Id);
            writeD(0x01); // active ??
            writeD(player.X);
            writeD(player.Y);

            writeD(player.Z);
            writeF(player.CurHP);
            writeF(player.CurMP);
            writeD(player.SP);

            writeQ(player.Exp);
            writeD(player.Level);
            writeD(player.Karma); // thx evill33t
            writeD(0); //?

            writeD(player.INT);
            writeD(player.STR);
            writeD(player.CON);
            writeD(player.MEN);

            writeD(player.DEX);
            writeD(player.WIT);
            for (int i = 0; i < 30; i++)
                writeD(0x00);

            writeD(0x00); // c3 work
            writeD(0x00); // c3 work

            writeD(0x00);

            writeD(0x00); // c3

            writeD((int)player.ActiveClass.ClassId.Id);

            writeD(0x00); // c3 InspectorBin
            writeD(0x00); // c3
            writeD(0x00); // c3
            writeD(0x00); // c3
        }
    }
}