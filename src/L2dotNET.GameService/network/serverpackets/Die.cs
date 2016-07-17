using System.Collections.Generic;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class Die : GameServerNetworkPacket
    {
        private readonly int _sId;
        private int _mNVillage;
        private int _mNAgit;
        private const int MNBattleCamp = 0;
        private int _mNCastle;
        private readonly int _mSpoil;
        private int _mNOriginal;
        private int _mNFotress;
        private const int MNAgathion = 0;
        private const bool MBShow = false;

        private List<int> _items;

        public void AddItem(int id)
        {
            if (_items == null)
                _items = new List<int>();

            _items.Add(id);
        }

        public Die(L2Character cha)
        {
            _sId = cha.ObjId;

            if (cha is L2Player)
                DiePlayer((L2Player)cha);
            else
            {
                if (cha is L2Warrior)
                    _mSpoil = ((L2Warrior)cha).SpoilActive ? 1 : 0;
            }
        }

        private void DiePlayer(L2Player player)
        {
            _mNVillage = 1;
            _mNOriginal = player.Builder;

            if (player.ClanId > 0)
            {
                _mNAgit = player.Clan.HideoutId > 0 ? 1 : 0;
                _mNCastle = player.Clan.CastleId > 0 ? 1 : 0;
                _mNFotress = player.Clan.FortressId > 0 ? 1 : 0;
            }

            AddItem(57);
        }

        protected internal override void Write()
        {
            WriteC(0x06);
            WriteD(_sId);
            WriteD(_mNVillage); //0
            WriteD(_mNAgit); //1
            WriteD(_mNCastle); //2
            WriteD(MNBattleCamp); //4
            WriteD(_mSpoil);
            WriteD(_mNOriginal); //5
            WriteD(_mNFotress); //3

            WriteC(0);
            //writeC(m_bShow ? 1 : 0);
            WriteD(MNAgathion); //21
            WriteD(_items?.Count ?? 0); //22+

            if (_items == null)
                return;

            foreach (int id in _items)
                WriteD(id);
        }
    }
}