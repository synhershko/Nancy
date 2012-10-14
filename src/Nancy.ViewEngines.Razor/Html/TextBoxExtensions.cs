using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Nancy.Extensions;

namespace Nancy.ViewEngines.Razor.Html
{
    public static class TextBoxExtensions
    {
        public static IHtmlString TextBoxFor<TModel, TValue>(this HtmlHelpers<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return TextBoxFor(html, expression, null);
        }

        public static IHtmlString TextBoxFor<TModel, TValue>(this HtmlHelpers<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return TextBoxFor(html, expression, CollectionExtensions.DictionaryFromAnonymousObject(htmlAttributes));
        }

        public static IHtmlString TextBoxFor<TModel, TValue>(this HtmlHelpers<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        {
            // get the html field name; probably need to run this through convention first
            var mi = expression.GetTargetMemberInfo();
            string htmlFieldName = mi.Name; /* TODO: normalize, conventions */

            // TODO: support getting DisplayName from Model metadata
            //ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string labelText = /*metadata.DisplayName ?? metadata.PropertyName ??*/ htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return NonEncodedHtmlString.Empty;
            }

            // TODO: add more htmlAttributes based on ModelMetadata
            // TODO: support value, format

            return TextBox(html, htmlFieldName, /* TODO: get value from Model */ null, htmlAttributes);
        }

        public static IHtmlString TextBox<TModel>(this HtmlHelpers<TModel> html, string name, Object value)
        {
            return TextBox(html, name, value, null, null);
        }

        public static IHtmlString TextBox<TModel>(this HtmlHelpers<TModel> html, string name, Object value, IDictionary<string, object> htmlAttributes)
        {
            return TextBox(html, name, value, null, htmlAttributes);
        }

        public static IHtmlString TextBox<TModel>(this HtmlHelpers<TModel> html, string name, Object value, Object htmlAttributes)
        {
            return TextBox(html, name, value, null, CollectionExtensions.DictionaryFromAnonymousObject(htmlAttributes));
        }

        public static IHtmlString TextBox<TModel>(this HtmlHelpers<TModel> html, string name, Object value, string format)
        {
            return TextBox(html, name, value, format, null);
        }

        public static IHtmlString TextBox<TModel>(this HtmlHelpers<TModel> html, string name, Object value, string format, Object htmlAttributes)
        {
            return TextBox(html, name, value, format, CollectionExtensions.DictionaryFromAnonymousObject(htmlAttributes));
        }

        public static IHtmlString TextBox<TModel>(this HtmlHelpers<TModel> html, string name, Object value, string format, IDictionary<string, object> htmlAttributes)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"<input type=""text"" name=""{0}""", name);

            if (htmlAttributes != null)
                foreach (var htmlAttribute in htmlAttributes)
                {
                    sb.AppendFormat(@" {0}=""{1}""", htmlAttribute.Key, htmlAttribute.Value);
                }

            if (value != null)
                sb.AppendFormat(@" value=""{0}""", value); // TODO: support format

            sb.Append("/>");
            return new NonEncodedHtmlString(sb.ToString());
        }
    }
}
