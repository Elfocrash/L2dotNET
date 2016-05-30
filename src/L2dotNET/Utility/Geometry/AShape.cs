using L2dotNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Utility
{
    public abstract class AShape
    {

        public abstract int GetSize();

        public abstract double GetArea();

        public abstract double GetVolume();

        public abstract bool IsInside(int x, int y);

        public abstract bool IsInside(int x, int y, int z);

        public abstract Location GetRandomLocation();
    }
}
