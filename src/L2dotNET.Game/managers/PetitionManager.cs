using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L2dotNET.Game.managers
{
    class PetitionManager
    {
        private static PetitionManager instance = new PetitionManager();
        public static PetitionManager getInstance()
        {
            return instance;
        }

        internal void petitionlink(L2Player player, string p)
        {
            throw new NotImplementedException();
        }
    }
}
