using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using UniversalForumClient.Core;

namespace UniversalForumClient.Extension
{
    public static partial class Extensions
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

    public static partial class Extensions
    {
        public static Image[] LookupImages(this object post, bool isRecur = true)
        {
            return LookupImages(new object[] { post }, isRecur: isRecur);
        }

        public static Image[] LookupImages(this IEnumerable<object> contents, bool isRecur = true)
        {
            var images = new List<Image>();

            foreach(var content in contents)
            {
                if (content is Image)
                {
                    images.Add(content as Image);
                }
                else
                {
                    if (isRecur == false)
                    {
                        continue;
                    }

                    var pros = content.GetType().GetProperties();
                    var proContents = pros.Where(w => w.Name == "Contents");

                    if (proContents.Count() > 0)
                    {
                        var proContent = proContents.First();
                        var proContentValue = proContent.GetValue(content);

                        var enumContent = proContentValue as IEnumerable<object>;

                        if (enumContent != null)
                        {
                            var childImages = enumContent.LookupImages();
                            images.AddRange(childImages);
                        }
                    }
                }
            }

            return images.ToArray();
        }
    }
}
