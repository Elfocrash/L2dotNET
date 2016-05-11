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
            //check if the dictionary contains the key
            if (base.ContainsKey(key))
            {
                var val = base[key].ToString();
                bool toReturn;
                if (bool.TryParse(val, out toReturn))
                    return toReturn;
                return defaultVal;
            }
            else
            {
                log.Info($"Key '{ key }' was not found in the dictionary! The function will return the 'defaultValue' parameter.");
                //if key doesn't exists,
                //returns the defaultValue var
                return defaultVal;
            }
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
            //check if the dictionary contains the key
            if (base.ContainsKey(key))
            {
                var val = base[key].ToString();
                byte toReturn;
                if (byte.TryParse(val, out toReturn))
                    return toReturn;
                return defaultVal;
            }
            else
            {
                log.Info($"Key '{ key }' was not found in the dictionary! The function will return the 'defaultValue' parameter.");
                //if key doesn't exists,
                //returns the defaultValue var
                return defaultVal;
            }
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
            //check if the dictionary contains the key
            if (base.ContainsKey(key))
            {
                var val = base[key].ToString();
                double toReturn;
                if (double.TryParse(val, out toReturn))
                    return toReturn;
                return defaultVal;
            }
            else
            {
                log.Info($"Key '{ key }' was not found in the dictionary! The function will return the 'defaultValue' parameter.");
                //if key doesn't exists,
                //returns the defaultValue var
                return defaultVal;
            }
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
            //check if the dictionary contains the key
            if (base.ContainsKey(key))
            {
                var val = base[key].ToString();
                float toReturn;
                if (float.TryParse(val, out toReturn))
                    return toReturn;
                return defaultVal;
            }
            else
            {
                log.Info($"Key '{ key }' was not found in the dictionary! The function will return the 'defaultValue' parameter.");
                //if key doesn't exists,
                //returns the defaultValue var
                return defaultVal;
            }
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
            //check if the dictionary contains the key
            if (base.ContainsKey(key))
            {
                var val = base[key].ToString();
                int toReturn;
                if (int.TryParse(val, out toReturn))
                    return toReturn;
                return defaultVal;
            }
            else
            {
                log.Info($"Key '{ key }' was not found in the dictionary! The function will return the 'defaultValue' parameter.");
                //if key doesn't exists,
                //returns the defaultValue var
                return defaultVal;
            }
        }

        public string GetString(string key)
        {
            var val = base[key].ToString();
            return val;
        }

        public string GetString(string key, string defaultVal)
        {
            //check if the dictionary contains the key
            if (base.ContainsKey(key))
            {
                var val = base[key].ToString();
                return val;
            }
            else
            {
                log.Info($"Key '{ key }' was not found in the dictionary! The function will return the 'defaultValue' parameter.");
                //if key doesn't exists,
                //returns the defaultValue var
                return defaultVal;
            }
        }

        public void Set<T>(string key, T value)
        {
            Add(key, value);
        }

        ///<summary>Gets the 'value' from dictionary based on 'key' parameter and converts it to the type of T.</summary>
        ///<typeparam name="T"></typeparam>
        ///<param name="key">Specific 'key' to get the 'value' from dictionary.</param>
        ///<param name="defaultValue">Optional. When not specified, has a value of 'default(T)'.</param>
        ///<returns>Converted value when Parse not fails, if it fails then returns 'defaultValue'.</returns>
        public T Get<T>(string key, T defaultValue = default(T)) where T : struct, IConvertible
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                log.Info($"Key is 'Null, Empty or White-space'! The function will return the 'defaultValue' parameter.");            
                return defaultValue;
            }

            //check if the dictionary contains the key
            if (base.ContainsKey(key))
            {
                string value = base[key].ToString();

                if (typeof(T).IsEnum)
                {
                    T result;

                    if (Enum.TryParse<T>(value, out result))
                    {
                        if (Enum.IsDefined(typeof(T), result)) //checks if enum name exists
                            return result; //if it returned true, returns converted value
                        return Enum.GetValues(typeof(T)).Cast<T>().FirstOrDefault(); //if it returned false, returns the first enum from list. Default value is always the enum with "0" value, but not every enum has it.
                    }
                    else
                    {
                        //throw new Exception($"Conversion of key '{ key }' failed! Cannot convert value '{ value }' to '{ typeof(T).FullName }'!");
                        return Enum.GetValues(typeof(T)).Cast<T>().FirstOrDefault(); //if it returned false, returns the first enum from list. Default value is always 0, but not every enum has a "0" value
                    }
                }
                else
                {
                    //find the TryParse method.
                    MethodInfo parseMethod = typeof(T).GetMethod("TryParse",
                                                                 //We want the public static one
                                                                 BindingFlags.Public | BindingFlags.Static,
                                                                 Type.DefaultBinder,
                                                                 //where the arguments are (string, out T)
                                                                 new[] { typeof(string), typeof(T).MakeByRefType() },
                                                                 null);

                    if (parseMethod == null)
                    {
                        //You need to know this so you can parse manually
                        log.Error($"'{ typeof(T).FullName }' doesn't have a 'TryParse(string s, out { typeof(T).FullName } result)' function! The function will return the 'defaultValue' parameter.");
                        return defaultValue;
                    }

                    //create the parameter list for the function call
                    object[] args = new object[] { value, default(T) };

                    //and then call the function.
                    if ((bool)parseMethod.Invoke(null, args))
                        return (T)args[1]; //if it returned true, returns converted value
                    else
                    {
                        log.Info($"Conversion of key '{ key }' failed! Cannot convert value '{ value }' to '{ typeof(T).FullName }'!");
                        return default(T); //if it returned false, returns default value of 'T'
                    }
                }
            }
            else
            {
                //if key doesn't exists,
                //returns the defaultValue var,
                //when not specified returns the default value of 'T'
                log.Info($"Key '{ key }' was not found in the dictionary! The function will return the 'defaultValue' parameter.");                
                return defaultValue;
            }
        }


    }
}
