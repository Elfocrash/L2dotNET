using System;
using L2dotNET.Models.Player;

namespace L2dotNET.Managers
{
    class PetitionManager
    {
        private static readonly PetitionManager Instance = new PetitionManager();

        public static PetitionManager GetInstance()
        {
            return Instance;
        }

        internal void Petitionlink(L2Player player, string p)
        {
            throw new NotImplementedException();
        }
    }
}