using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.GeoEngine
{
    public interface IBlockDynamic
    {
        void AddGeoObject(IGeoObject obj);

        void RemoveGeoObject(IGeoObject obj);
    }
}
