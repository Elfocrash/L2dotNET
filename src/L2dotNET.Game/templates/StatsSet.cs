using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Game.templates
{
    public class StatsSet : Dictionary<string, object>
    {
        public StatsSet() : base() { }

        public StatsSet(int size) : base(size) { }

        public StatsSet(StatsSet set) : base(set) { }

        public void Set(string key, object value)
        {
            Add(key, value);
        }

        public void Set(string key, string value)
        {
            Add(key, value);
        }

        public void Set(string key, bool value)
        {
            Add(key, value);
        }

        public void Set(string key, int value)
        {
            Add(key, value);
        }

        public void Set(string key, int[] value)
        {
            Add(key, value);
        }

        public void Set(string key, long value)
        {
            Add(key, value);
        }

        public void Set(string key, double value)
        {
            Add(key, value);
        }

        public void Set(string key, Enum value)
        {
            Add(key, value);
        }

        public void Unset(string key)
        {
            Remove(key);
        }

        public bool GetBool(string key)
        {
            var val = base[key].ToString();
            bool toReturn;
            bool.TryParse(val, out toReturn);
            return toReturn;
        }

        public byte GetByte(string key)
        {
            var val = base[key].ToString();
            byte toReturn;
            byte.TryParse(val, out toReturn);
            return toReturn;
        }

        public double GetDouble(string key)
        {
            var val = base[key].ToString();
            double toReturn;
            double.TryParse(val, out toReturn);
            return toReturn;
        }

        public double GetDouble(string key, double defaultVal)
        {
            return defaultVal;
        }

        public float GetFloat(string key)
        {
            var val = base[key].ToString();
            float toReturn;
            float.TryParse(val, out toReturn);
            return toReturn;
        }

        public int GetInt(string key)
        {
            var val = base[key].ToString();
            int toReturn;
            int.TryParse(val, out toReturn);
            return toReturn;
        }

        public int GetInt(string key, int defaultVal)
        {
            return defaultVal;
        }

        public string GetString(string key)
        {
            var val = base[key].ToString();
            return val;
        }

        public string GetString(string key, string defaultVal)
        {
            return defaultVal;
        }


    }
}
