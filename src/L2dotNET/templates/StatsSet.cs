using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;

namespace L2dotNET.Templates
{
    public class StatsSet : Dictionary<string, object>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public StatsSet() { }

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

        public void Set<T>(string key, T value)
        {
            Add(key, value);
        }

        public void Unset(string key)
        {
            Remove(key);
        }

        public bool GetBool(string key, bool defaultValue = default(bool))
        {
            //check if the dictionary contains the key
            if (!ContainsKey(key))
                return defaultValue;

            string value = base[key].ToString();
            bool toReturn;
            return bool.TryParse(value, out toReturn) ? toReturn : defaultValue;
            // else
            // Log.Error($"Conversion of key '{ key }' failed! Cannot convert value '{ value }' to '{ typeof(bool).FullName }'! The function will return the 'defaultValue' parameter.");
            // Log.Warn($"Key '{ key }' was not found in the dictionary! The function will return the 'defaultValue' parameter.");
            //if key doesn't exists,
            //returns the defaultValue var
        }

        public byte GetByte(string key, byte defaultValue = default(byte))
        {
            //check if the dictionary contains the key
            if (!ContainsKey(key))
                return defaultValue;

            string value = base[key].ToString();
            byte toReturn;
            return byte.TryParse(value, out toReturn) ? toReturn : defaultValue;
            //else
            //     Log.Error($"Conversion of key '{ key }' failed! Cannot convert value '{ value }' to '{ typeof(byte).FullName }'! The function will return the 'defaultValue' parameter.");
            //Log.Warn($"Key '{ key }' was not found in the dictionary! The function will return the 'defaultValue' parameter.");
            //if key doesn't exists,
            //returns the defaultValue var
        }

        public int[] GetIntegerArray(string key)
        {
            object val = this[key];

            if (val is int[])
                return (int[])val;
            if (val is int)
                return new[] { int.Parse(val.ToString()) };

            if (!(val is string))
                return null;

            string[] vals = ((string)val).Split(';');

            int[] result = new int[vals.Length];

            int i = 0;
            foreach (string v in vals)
                result[i++] = int.Parse(v);

            return result;
        }

        public int GetInt(string key, int defaultValue = default(int))
        {
            //check if the dictionary contains the key
            if (!ContainsKey(key))
                return defaultValue;

            string value = base[key].ToString();
            int toReturn;
            return int.TryParse(value, out toReturn) ? toReturn : defaultValue;
            //else
            // Log.Error($"Conversion of key '{ key }' failed! Cannot convert value '{ value }' to '{ typeof(int).FullName }'! The function return the 'defaultValue' parameter.");
            //Log.Warn($"Key '{ key }' was not found in the dictionary! The function will return the 'defaultValue' parameter.");
            //if key doesn't exists,
            //returns the defaultValue var
        }

        public float GetFloat(string key, float defaultValue = default(float))
        {
            //check if the dictionary contains the key
            if (!ContainsKey(key))
                return defaultValue;

            string value = base[key].ToString();
            float toReturn;
            return float.TryParse(value, out toReturn) ? toReturn : defaultValue;
            // else
            //   Log.Error($"Conversion of key '{ key }' failed! Cannot convert value '{ value }' to '{ typeof(float).FullName }'! The function return the 'defaultValue' parameter.");
            // Log.Warn($"Key '{ key }' was not found in the dictionary! The function will return the 'defaultValue' parameter.");
            //if key doesn't exists,
            //returns the defaultValue var
        }

        public double GetDouble(string key, double defaultValue = default(double))
        {
            //check if the dictionary contains the key
            if (!ContainsKey(key))
                return defaultValue;

            string value = base[key].ToString();
            double toReturn;
            return double.TryParse(value, out toReturn) ? toReturn : defaultValue;
            //else
            //    Log.Error($"Conversion of key '{ key }' failed! Cannot convert value '{ value }' to '{ typeof(double).FullName }'! The function will return the 'defaultValue' parameter.");
            //Log.Warn($"Key '{ key }' was not found in the dictionary! The function will return the 'defaultValue' parameter.");
            //if key doesn't exists,
            //returns the defaultValue var
        }

        public string GetString(string key, string defaultValue = default(string))
        {
            //check if the dictionary contains the key
            if (!ContainsKey(key))
                return defaultValue;

            string value = base[key].ToString();
            return value;
            //Log.Warn($"Key '{ key }' was not found in the dictionary! The function will return the 'defaultValue' parameter.");
            //if key doesn't exists,
            //returns the defaultValue var
        }

        ///<summary>Gets the 'value' from dictionary based on 'key' parameter and converts it to the type of T.</summary>
        ///<typeparam name="T"></typeparam>
        ///<param name="key">Specific 'key' to get the 'value' from dictionary.</param>
        ///<param name="defaultValue">Optional. When not specified, has a value of 'default(T)'.</param>
        ///<returns>Converted value when Parse not fails, if it fails then returns 'defaultValue'.</returns>
        public T Get<T>(string key, T defaultValue = default(T)) where T : struct, IConvertible
        {
            if (string.IsNullOrWhiteSpace(key))
                return defaultValue;

            //check if the dictionary contains the key
            if (!ContainsKey(key))
                return defaultValue;

            string value = base[key].ToString();

            if (typeof(T).IsEnum) //block for Enum handling
            {
                T result;

                if (Enum.TryParse(value, out result))
                    return Enum.IsDefined(typeof(T), result) ? result : Enum.GetValues(typeof(T)).Cast<T>().FirstOrDefault();
                //  Log.Error($"Conversion of key '{ key }' failed! Cannot convert value '{ value }' to '{ typeof(T).FullName }'! The function will return the first enum element.");
                return Enum.GetValues(typeof(T)).Cast<T>().FirstOrDefault(); //if it returned false, returns the first element from enum. Default value is always 0, but not every enum has it.
            }
            //find the TryParse method.
            MethodInfo parseMethod = typeof(T).GetMethod("TryParse",
                                                         //We want the public static one
                                                         BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder,
                                                         //where the arguments are (string, out T)
                                                         new[] { typeof(string), typeof(T).MakeByRefType() }, null);

            if (parseMethod == null)
                return defaultValue;

            //create the parameter list for the function call
            object[] args = { value, default(T) };

            //and then call the function.
            if ((bool)parseMethod.Invoke(null, args))
                return (T)args[1]; //if it returned true, returns converted value
            // Log.Error($"Conversion of key '{ key }' failed! Cannot convert value '{ value }' to '{ typeof(T).FullName }'! The function will return the 'defaultValue' parameter.");
            return defaultValue; //if it returned false, returns defaultValue' parameter."
            //if key doesn't exists,
            //returns the defaultValue var,
            //when not specified returns the default value of 'T'
            // Log.Warn($"Key '{ key }' was not found in the dictionary! The function will return the 'defaultValue' parameter.");
        }

        public string[] GetStringArray(string key)
        {
            object val = this[key];

            if (val is string[])
                return (string[])val;
            if (val is string)
                return ((string)val).Split(';');

            return null;
            //throw new IllegalArgumentException($"StatsSet : String array required, but found: {val} for key: {key}.");
        }
    }
}