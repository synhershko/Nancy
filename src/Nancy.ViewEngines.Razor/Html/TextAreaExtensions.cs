using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Nancy.Extensions;

namespace Nancy.ViewEngines.Razor.Html
{
    public static class TextAreaExtensions
    {
        public static IHtmlString TextAreaFor<TModel, TProperty>(this HtmlHelpers<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return TextAreaFor(htmlHelper, expression, -1, -1, null);
        }

        public static IHtmlString TextAreaFor<TModel, TProperty>(this HtmlHelpers<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Object htmlAttributes)
        {
            return TextAreaFor(htmlHelper, expression, -1, -1, CollectionExtensions.DictionaryFromAnonymousObject(htmlAttributes));
        }

        public static IHtmlString TextAreaFor<TModel, TProperty>(this HtmlHelpers<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, Object> htmlAttributes)
        {
            return TextAreaFor(htmlHelper, expression, -1, -1, htmlAttributes);
        }

        public static IHtmlString TextAreaFor<TModel, TProperty>(this HtmlHelpers<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
            int rows, int columns, Object htmlAttributes)
        {
            return TextAreaFor(htmlHelper, expression, rows, columns, CollectionExtensions.DictionaryFromAnonymousObject(htmlAttributes));
        }

        public static IHtmlString TextAreaFor<TModel, TProperty>(this HtmlHelpers<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
            int rows, int columns, IDictionary<string, Object> htmlAttributes)
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

            return TextArea(htmlHelper, htmlFieldName, /* TODO: get value from Model */ null, rows, columns, htmlAttributes);
        }

        public static IHtmlString TextArea<TModel>(this HtmlHelpers<TModel> htmlHelper, string name)
        {
            return TextArea(htmlHelper, name, null, -1, -1, null);
        }

        public static IHtmlString TextArea<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, IDictionary<string, Object> htmlAttributes)
        {
            return TextArea(htmlHelper, name, null, -1, -1, htmlAttributes);
        }

        public static IHtmlString TextArea<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, Object htmlAttributes)
        {
            return TextArea(htmlHelper, name, null, -1, -1, CollectionExtensions.DictionaryFromAnonymousObject(htmlAttributes));
        }

        public static IHtmlString TextArea<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, string value)
        {
            return TextArea(htmlHelper, name, value, -1, -1, null);
        }

        public static IHtmlString TextArea<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, string value, Object htmlAttributes)
        {
            return TextArea(htmlHelper, name, value, -1 , -1, CollectionExtensions.DictionaryFromAnonymousObject(htmlAttributes));
        }

        public static IHtmlString TextArea<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, string value, IDictionary<string, Object> htmlAttributes)
        {
            return TextArea(htmlHelper, name, value, -1, -1, htmlAttributes);
        }

        public static IHtmlString TextArea<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, string value, int rows, int columns, Object htmlAttributes)
        {
            return TextArea(htmlHelper, name, value, rows, columns, CollectionExtensions.DictionaryFromAnonymousObject(htmlAttributes));
        }

        public static IHtmlString TextArea<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, string value, int rows, int columns, IDictionary<string, Object> htmlAttributes)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"<textarea name=""{0}""", name);

            if (htmlAttributes != null)
                foreach (var htmlAttribute in htmlAttributes)
                {
                    sb.AppendFormat(@" {0}=""{1}""", htmlAttribute.Key, htmlAttribute.Value);
                }

            if (rows >= 0)
                sb.AppendFormat(@" rows=""{0}""", rows);

            if (columns >= 0)
                sb.AppendFormat(@" cols=""{0}""", columns);

            sb.AppendFormat(@">{0}</textarea>", value ?? string.Empty);
            return new NonEncodedHtmlString(sb.ToString());
        }
    }
}
