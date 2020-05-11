using System;
using System.Collections.Generic;

namespace SJMC.Helpers
{
    public static class SettingsHelper<T> where T : class
    {

        public static List<T> StringJsonToList(string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(value);
        }

        public static string ListToStringJson(List<T> value)
        {
            var saveString = Newtonsoft.Json.JsonConvert.SerializeObject(value);

            return saveString;
        }

        public static string StringJsonToListDefault()
        {
            var newItem = new List<T>();

            var stringDefault = Newtonsoft.Json.JsonConvert.SerializeObject(newItem);

            return stringDefault;
        }

    }
}
