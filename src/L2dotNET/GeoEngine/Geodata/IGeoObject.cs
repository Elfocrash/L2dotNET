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