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
            return EditorFor(html, expression, null, null, null);
        }

        public static IHtmlString EditorFor<TModel, TValue>(this HtmlHelpers<TModel> html, Expression<Func<TModel, TValue>> expression, Object additionalViewData)
        {
            return EditorFor(html, expression, null, null, additionalViewData);
        }

        public static IHtmlString EditorFor<TModel, TValue>(this HtmlHelpers<TModel> html, Expression<Func<TModel, TValue>> expression, string templateName)
        {
            return EditorFor(html, expression, templateName, null, null);
        }

        public static IHtmlString EditorFor<TModel, TValue>(this HtmlHelpers<TModel> html, Expression<Func<TModel, TValue>> expression, string templateName, Object additionalViewData)
        {
            return EditorFor(html, expression, templateName, null, additionalViewData);
        }

        public static IHtmlString EditorFor<TModel, TValue>(this HtmlHelpers<TModel> html, Expression<Func<TModel, TValue>> expression, string templateName, string htmlFieldName)
        {
            return EditorFor(html, expression, templateName, htmlFieldName, null);
        }

        public static IHtmlString EditorFor<TModel, TValue>(this HtmlHelpers<TModel> html, Expression<Func<TModel, TValue>> expression, string templateName, string htmlFieldName, Object additionalViewData)
        {
            if (htmlFieldName == null)
            {
                // get the html field name; probably need to run this through convention first
                var mi = expression.GetTargetMemberInfo();
                htmlFieldName = mi.Name; /* TODO: normalize, conventions */
            }

            IEditorTemplate editor;
            DefaultEditorActions.TryGetValue(mi.ReflectedType.Name, out editor);
            if (editor != null)
                return editor.EditorTemplate(html, htmlFieldName, htmlAttributes);

            return NonEncodedHtmlString.Empty;
        }

        public static IHtmlString Editor<TModel>(this HtmlHelpers<TModel> html, string expression, string templateName = null, string htmlFieldName = null, Object additionalViewData = null)
        {
            
        }
    }
}
