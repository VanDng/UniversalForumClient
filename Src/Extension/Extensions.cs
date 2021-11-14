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

            JsonSerializerOptions jso = new JsonSerializerOptions();
            jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            var jsonString = JsonSerializer.Serialize(newObjects, jso);

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
                if (property.Name == "Contents")
                {
                    var objContents = property.GetValue(target);
                    var contents = objContents as object[];

                    var newContents = new List<object>();
                    foreach (var content in contents)
                    {
                        var newContent = CreateExpandObject(content);
                        newContents.Add(newContent);
                    }

                    dictionary.Add(property.Name, newContents);
                }
                else
                {
                    dictionary.Add(property.Name, property.GetValue(target));
                }
            }

            return expando;
        }
    }
}
