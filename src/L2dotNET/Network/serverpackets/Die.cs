using System.Collections.Generic;
using L2dotNET.Models;
using L2dotNET.Models.Npcs;
using L2dotNET.Models.Player;

namespace L2dotNET.Network.serverpackets
{
    class Die : GameserverPacket
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
            _sId = cha.CharacterId;

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

            //if (player.ClanId > 0)
            //{
            //    _mNAgit = player.Clan.HideoutId > 0 ? 1 : 0;
            //    _mNCastle = player.Clan.CastleId > 0 ? 1 : 0;
            //    _mNFotress = player.Clan.FortressId > 0 ? 1 : 0;
            //}

            AddItem(57);
        }

        public override void Write()
        {
            WriteByte(0x06);
            WriteInt(_sId);
            WriteInt(_mNVillage); //0
            WriteInt(_mNAgit); //1
            WriteInt(_mNCastle); //2
            WriteInt(MNBattleCamp); //4
            WriteInt(_mSpoil);
            WriteInt(_mNOriginal); //5
            WriteInt(_mNFotress); //3
            WriteByte(0);
            //writeC(m_bShow ? 1 : 0);
            WriteInt(MNAgathion); //21
            WriteInt(_items?.Count ?? 0); //22+

            _items?.ForEach(WriteInt);
        }
    }
}