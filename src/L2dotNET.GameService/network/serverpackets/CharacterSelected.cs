using L2dotNET.GameService.Model.Player;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class CharacterSelected : GameServerNetworkPacket
    {
        private readonly int _session;
        private readonly L2Player _player;

        public CharacterSelected(L2Player player, int session)
        {
            this._player = player;
            this._session = session;
        }

        protected internal override void Write()
        {
            WriteC(0x15);

            WriteS(_player.Name);
            WriteD(_player.ObjId);
            WriteS(_player.Title);
            WriteD(_session);

            WriteD(_player.ClanId);
            WriteD(0x00); //??
            WriteD(_player.Sex);
            WriteD((int)_player.BaseClass.ClassId.ClassRace);

            WriteD((int)_player.ActiveClass.ClassId.Id);
            WriteD(0x01); // active ??
            WriteD(_player.X);
            WriteD(_player.Y);

            WriteD(_player.Z);
            WriteF(_player.CurHp);
            WriteF(_player.CurMp);
            WriteD(_player.Sp);

            WriteQ(_player.Exp);
            WriteD(_player.Level);
            WriteD(_player.Karma); // thx evill33t
            WriteD(0); //?

            WriteD(_player.Int);
            WriteD(_player.Str);
            WriteD(_player.Con);
            WriteD(_player.Men);

            WriteD(_player.Dex);
            WriteD(_player.Wit);
            for (int i = 0; i < 30; i++)
                WriteD(0x00);

            WriteD(0x00); // c3 work
            WriteD(0x00); // c3 work

            WriteD(0x00);

            WriteD(0x00); // c3

            WriteD((int)_player.ActiveClass.ClassId.Id);

            WriteD(0x00); // c3 InspectorBin
            WriteD(0x00); // c3
            WriteD(0x00); // c3
            WriteD(0x00); // c3
        }
    }
}