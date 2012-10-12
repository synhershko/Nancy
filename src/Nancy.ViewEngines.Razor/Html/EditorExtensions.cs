using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Nancy.Extensions;

namespace Nancy.ViewEngines.Razor.Html
{
    public static class EditorExtensions
    {
        // TODO: Support templates for read-only mode
        private static readonly Dictionary<string, IEditorTemplate> DefaultEditorActions =
            new Dictionary<string, IEditorTemplate>(StringComparer.OrdinalIgnoreCase)
            {
                //{ "HiddenInput", DefaultEditorTemplates.HiddenInputTemplate },
                //{ "MultilineText", DefaultEditorTemplates.MultilineTextTemplate },
                { "Password", new DefaultEditorTemplates.DefaultPasswordEditorTemplate() },
                { "Text", new DefaultEditorTemplates.DefaultStringEditorTemplate() },
                //{ "Collection", DefaultEditorTemplates.CollectionTemplate },
                //{ typeof(bool).Name, DefaultEditorTemplates.BooleanTemplate },
                { typeof(int).Name, new DefaultEditorTemplates.DefaultIntegerEditorTemplate() },
                //{ typeof(byte).Name, DefaultEditorTemplates.ByteTemplate },
                //{ typeof(short).Name, DefaultEditorTemplates.ShortTemplate},
                //{ typeof(long).Name, DefaultEditorTemplates.LongTemplate },
                //{ typeof(double).Name, DefaultEditorTemplates.DoubleTemplate },
                //{ typeof(float).Name, DefaultEditorTemplates.FloatTemplate },
                //{ typeof(decimal).Name, DefaultEditorTemplates.DecimalTemplate },
                { typeof(string).Name, new DefaultEditorTemplates.DefaultStringEditorTemplate() },
                //{ typeof(object).Name, DefaultEditorTemplates.ObjectTemplate },
            };

        private static T GetPropertyValue<T>(this object obj, string propName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            PropertyInfo pi = obj.GetType().GetProperty(propName,
                                                        BindingFlags.Public | BindingFlags.NonPublic
                                                        | BindingFlags.Instance);
            if (pi == null)
            {
                throw new ArgumentOutOfRangeException("propName",
                                                      string.Format("Property {0} was not found in Type {1}",
                                                                    propName,
                                                                    obj.GetType().FullName));
            }

            return (T)pi.GetValue(obj, null);
        }

        public static IHtmlString EditorFor<TModel, TValue>(this HtmlHelpers<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return EditorFor(html, expression, null);
        }

        public static IHtmlString EditorFor<TModel, TValue>(this HtmlHelpers<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return EditorFor(html, expression, CollectionExtensions.DictionaryFromAnonymousObject(htmlAttributes));
        }

        public static IHtmlString EditorFor<TModel, TValue>(this HtmlHelpers<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        {
            // get the html field name; probably need to run this through convention first
            var mi = expression.GetTargetMemberInfo();
            string htmlFieldName = mi.Name; /* TODO: normalize, conventions */

            IEditorTemplate editor;
            DefaultEditorActions.TryGetValue(mi.ReflectedType.Name, out editor);
            if (editor != null)
                return editor.EditorTemplate(html, htmlFieldName, htmlAttributes);

            return NonEncodedHtmlString.Empty;
        }
    }
}
