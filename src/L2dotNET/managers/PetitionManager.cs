using System;
using L2dotNET.Models.player;

namespace L2dotNET.managers
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