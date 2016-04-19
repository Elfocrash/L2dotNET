using System;
using System.Collections.Generic;
using System.Linq;
using L2dotNET.Game.db;
using L2dotNET.Game.network.l2send;

namespace L2dotNET.Game.managers
{
    public class MinigameRankManager
    {
        private static MinigameRankManager instance = new MinigameRankManager();
        public static MinigameRankManager getInstance()
        {
            return instance;
        }

        public List<MinigameMember> _top = new List<MinigameMember>();
        public byte _topMax = 100;

        public void showRank(L2Player player)
        {
            if (!Cfg.MinigameRankEnabled)
            {
                player.sendActionFailed();
                player.sendMessage("Minigame ranks disabled.");
                return;
            }

            int cur = -1;
            var sort = from m in _top where m.Score > 0 orderby m.Score select m;
            short c = 0;
            List<MinigameMember> rank = new List<MinigameMember>();
            foreach (MinigameMember m in sort)
            {
                c++;
             //   m.Top = c;
                if (cur == -1)
                    if (m.ID == player.ObjID)
                        cur = c;

                
                if (c <= _topMax)
                    rank.Add(m);

                if (c >= 1000)
                    break;
            }

            player.sendPacket(new ExBR_MinigameLoadScores(rank, cur == -1 ? 0 : cur, 0, player.LastMinigameScore));
        }

        public void insertMe(L2Player player, int m_CurrentScore)
        {
            if (!Cfg.MinigameRankEnabled)
            {
                player.sendActionFailed();
                player.sendMessage("Minigame ranks disabled.");
                return;
            }

            bool found = false;
            foreach(MinigameMember m in _top)
                if (m.ID == player.ObjID)
                {
                    found = true;
                    if (m.Score >= m_CurrentScore)
                    {
                        player.sendMessage("Your max score was " + m.Score + ". Nothing to bet.");
                        return;
                    }
                    else
                    {
                        m.Score = m_CurrentScore;
                        player.sendMessage("You bet your best score " + m.Score + " with +"+(m_CurrentScore-m.Score)+" points.");

                        SQL_Block sqb = new SQL_Block("minigame_rank");
                        sqb.param("score", m_CurrentScore);
                        sqb.param("achived", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                        sqb.where("playerId", player.ObjID);
                        sqb.sql_update(false);
                    }
                }

            if (!found)
            {
                player.sendMessage("Saved score " + m_CurrentScore + "");

                MinigameMember m = new MinigameMember();
                m.ID = player.ObjID;
                m.Name = player.Name;
                m.Score = m_CurrentScore;
                _top.Add(m);

                SQL_Block sqb = new SQL_Block("minigame_rank");
                sqb.param("playerId", player.ObjID);
                sqb.param("score", m_CurrentScore);
                sqb.param("achived", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                sqb.sql_insert(false);
            }

            player.LastMinigameScore = m_CurrentScore;
        }
    }

    public class MinigameMember
    {
        public int Top, Score, ID;
        public string Name;
    }
}

