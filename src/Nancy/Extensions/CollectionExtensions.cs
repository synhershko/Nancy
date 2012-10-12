using System.ComponentModel;

namespace Nancy.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    public static class CollectionExtensions
    {
        public static IDictionary<string, object> DictionaryFromAnonymousObject(object values)
        {
            var ret = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            if (values != null)
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(values);
                foreach (PropertyDescriptor prop in props)
                {
                    object val = prop.GetValue(values);
                    ret.Add(prop.Name, val);
                }
            }
            return ret;
        }

        public static IDictionary<string, IEnumerable<string>> ToDictionary(this NameValueCollection source)
        {
            return source.AllKeys.ToDictionary<string, string, IEnumerable<string>>(key => key, source.GetValues);
        }

        public static NameValueCollection ToNameValueCollection(this IDictionary<string, IEnumerable<string>> source)
        {
            var collection = new NameValueCollection();

            foreach (var key in source.Keys)
            {
                foreach (var value in source[key])
                {
                    collection.Add(key, value);
                }
            }

            return collection;
        }

        public static IDictionary<string, string> Merge(this IEnumerable<IDictionary<string, string>> dictionaries)
        {
            var output =
                new Dictionary<string, string>(StaticConfiguration.CaseSensitive ? StringComparer.InvariantCulture : StringComparer.InvariantCultureIgnoreCase);

            foreach (var dictionary in dictionaries.Where(d => d != null))
            {
                foreach (var kvp in dictionary)
                {
                    if (!output.ContainsKey(kvp.Key))
                    {
                        output.Add(kvp.Key, kvp.Value);
                    }
                }
            }

            return output;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var knownKeys = new HashSet<TKey>();

            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}