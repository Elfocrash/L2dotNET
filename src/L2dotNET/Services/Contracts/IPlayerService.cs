﻿using L2dotNET.Models.Player;

namespace L2dotNET.Services.Contracts
{
    public interface IPlayerService
    {
        L2Player GetPlayerByLogin(int objId);

        bool CheckIfPlayerNameExists(string name);

        void CreatePlayer(L2Player player);

        void UpdatePlayer(L2Player player);

        L2Player GetPlayerBySlotId(string accountName, int slotId);

        bool MarkToDeleteChar(int objId, long deletetime);

        bool MarkToRestoreChar(int objId);

        bool DeleteCharByObjId(int objId);

        L2Player RestorePlayer(int id, GameClient client);

        int GetDaysRequiredToDeletePlayer();
    }
}