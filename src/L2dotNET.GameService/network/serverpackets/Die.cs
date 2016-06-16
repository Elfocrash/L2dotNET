using System.Collections.Generic;
using L2dotNET.GameService.Model.Npcs;
using L2dotNET.GameService.Model.Player;
using L2dotNET.GameService.World;

namespace L2dotNET.GameService.Network.Serverpackets
{
    class Die : GameServerNetworkPacket
    {
        private readonly int sId;
        private int m_nVillage;
        private int m_nAgit;
        private const int m_nBattleCamp = 0;
        private int m_nCastle;
        private readonly int m_Spoil;
        private int m_nOriginal;
        private int m_nFotress;
        private const int m_nAgathion = 0;
        private const bool m_bShow = false;

        private List<int> _items;

        public void addItem(int id)
        {
            if (_items == null)
                _items = new List<int>();

            _items.Add(id);
        }

        public Die(L2Character cha)
        {
            sId = cha.ObjID;

            if (cha is L2Player)
                DiePlayer((L2Player)cha);
            else if (cha is L2Warrior)
                m_Spoil = ((L2Warrior)cha).spoilActive ? 1 : 0;
        }

        private void DiePlayer(L2Player player)
        {
            m_nVillage = 1;
            m_nOriginal = player.Builder;

            if (player.ClanId > 0)
            {
                m_nAgit = player.Clan.HideoutID > 0 ? 1 : 0;
                m_nCastle = player.Clan.CastleID > 0 ? 1 : 0;
                m_nFotress = player.Clan.FortressID > 0 ? 1 : 0;
            }

            addItem(57);
        }

        protected internal override void write()
        {
            writeC(0x06);
            writeD(sId);
            writeD(m_nVillage); //0
            writeD(m_nAgit); //1
            writeD(m_nCastle); //2
            writeD(m_nBattleCamp); //4
            writeD(m_Spoil);
            writeD(m_nOriginal); //5
            writeD(m_nFotress); //3

            writeC(0);
            //writeC(m_bShow ? 1 : 0);
            writeD(m_nAgathion); //21
            writeD(_items == null ? 0 : _items.Count); //22+

            if (_items != null)
                foreach (int id in _items)
                    writeD(id);
        }
    }
}