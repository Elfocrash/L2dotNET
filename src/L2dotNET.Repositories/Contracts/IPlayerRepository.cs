﻿using System.Threading.Tasks;
using L2dotNET.DataContracts;

namespace L2dotNET.Repositories.Contracts
{
    public interface IPlayerRepository
    {
        Task<bool> CheckIfPlayerNameExists(string name);

        Task<CharacterContract> GetPlayerModelBySlotId(string accountName, int slotId);
    }
}