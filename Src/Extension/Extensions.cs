using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace UniversalForumClient.Extension
{
    public static class Extensions
    {
        public static string Serialize(this IEnumerable<object> objects)
        {
            var newObjects = objects.Select(s => CreateExpandObject(s));

            var jsonString = JsonSerializer.Serialize(newObjects);

            return jsonString;
        }

        private static ExpandoObject CreateExpandObject(object target)
        {
            var expando = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)expando;

            string fullType = target.GetType().ToString();
            
            string typeName = fullType;
            int dotIdx = typeName.LastIndexOf(".");
            if (dotIdx > 0)
            {
                typeName = typeName.Substring(dotIdx + 1);
            }

            dictionary.Add("Type", typeName);

            foreach (var property in target.GetType().GetProperties())
            {
                dictionary.Add(property.Name, property.GetValue(target));
            }

            return expando;
        }
    }
}
