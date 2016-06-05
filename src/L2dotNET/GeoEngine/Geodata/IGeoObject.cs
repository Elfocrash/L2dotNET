using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.GeoEngine
{
    public interface IGeoObject
    {
        int GetGeoX();

        int GetGeoY();

        int GetGeoZ();

        int GetHeight();

        byte[][] GetObjectGeoData();
    }
}
