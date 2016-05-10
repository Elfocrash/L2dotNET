using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace L2dotNET.Game.templates
{
    public class StatsSet : Dictionary<string, object>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StatsSet));

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

        public bool GetBool(string key, bool defaultVal)
        {
            var val = base[key].ToString();
            bool toReturn;
            if (bool.TryParse(val, out toReturn))
                return toReturn;
            return defaultVal;
        }

        public byte GetByte(string key)
        {
            var val = base[key].ToString();
            byte toReturn;
            byte.TryParse(val, out toReturn);
            return toReturn;
        }

        public byte GetByte(string key, byte defaultVal)
        {
            var val = base[key].ToString();
            byte toReturn;
            if (byte.TryParse(val, out toReturn))
                return toReturn;
            return defaultVal;
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
            var val = base[key].ToString();
            double toReturn;
            if (double.TryParse(val, out toReturn))
                return toReturn;
            return defaultVal;
        }

        public float GetFloat(string key)
        {
            var val = base[key].ToString();
            float toReturn;
            float.TryParse(val, out toReturn);
            return toReturn;
        }

        public float GetFloat(string key, float defaultVal)
        {
            var val = base[key].ToString();
            float toReturn;
            if (float.TryParse(val, out toReturn))
                return toReturn;
            return defaultVal;
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
            var val = base[key].ToString();
            int toReturn;
            if (int.TryParse(val, out toReturn))
                return toReturn;
            return defaultVal;
        }

        public string GetString(string key)
        {
            var val = base[key].ToString();
            return val;
        }

        public string GetString(string key, string defaultVal)
        {
            var val = base[key].ToString();
            if (string.IsNullOrWhiteSpace(val))
                return defaultVal;
            return val;
        }

        public T Get<T>(string key, T defaultValue = default(T)) where T : struct, IConvertible
        {
            //check if the dictionary contains the key
            if (base.ContainsKey(key))
            {
                string value = base[key].ToString();

                // find the TryParse method.
                MethodInfo parseMethod = typeof(T).GetMethod("TryParse",
                                                             // We want the public static one
                                                             BindingFlags.Public | BindingFlags.Static,
                                                             Type.DefaultBinder,
                                                             // where the arguments are (string, out T)
                                                             new[] { typeof(string), typeof(T).MakeByRefType() },
                                                             null);

                if (parseMethod == null)
                {
                    // You need to know this so you can parse manually
                    log.Error($"'{ typeof(T).FullName }' doesn't have a 'TryParse(string s, out { typeof(T).FullName } result)' function! The function will return the 'defaultValue' parameter.");
                    return defaultValue;
                }

                // create the parameter list for the function call
                object[] args = new object[] { value, default(T) };

                // and then call the function.
                if ((bool)parseMethod.Invoke(null, args))
                    return (T)args[1]; // if it returned true, returns converted value
                else
                {
                    log.Info($"Conversion of key '{ key }' failed! Cannot convert value '{ value }' to '{ typeof(T).FullName }'!");
                    return default(T); // if it returned false, returns default value of 'T'
                }
            }
            else
            {
                log.Info($"Key '{ key }' was not found in the dictionary! The function will return the 'defaultValue' parameter.");
                //if key doesn't exists,
                //returns the defaultValue var,
                //when not specified returns the default value of 'T'
                return defaultValue;
            }
        }


    }
}
