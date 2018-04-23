﻿using L2dotNET.Models.Player;

namespace L2dotNET.Handlers
{
    public interface IAdminCommandHandler
    {
        void Request(L2Player admin, string alias);

        void Register(object processor);

        void Initialize();
    }
}