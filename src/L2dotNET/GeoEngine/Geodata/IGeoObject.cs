namespace L2dotNET.GeoEngine.Geodata
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