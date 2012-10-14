using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Nancy.Extensions;

namespace Nancy.ViewEngines.Razor.Html
{
    public static class CheckBoxExtensions
    {
        public static IHtmlString CheckBoxFor<TModel>(this HtmlHelpers<TModel> htmlHelper, Expression<Func<TModel, bool>> expression)
        {
            return CheckBoxFor(htmlHelper, expression, null);
        }

        public static IHtmlString CheckBoxFor<TModel>(this HtmlHelpers<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, Object htmlAttributes)
        {
            return CheckBoxFor(htmlHelper, expression, CollectionExtensions.DictionaryFromAnonymousObject(htmlAttributes));
        }

        public static IHtmlString CheckBoxFor<TModel>(this HtmlHelpers<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, IDictionary<string, Object> htmlAttributes)
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

            return CheckBox(htmlHelper, htmlFieldName, /* TODO: get value from Model */ false, htmlAttributes);
        }

        public static IHtmlString CheckBox<TModel>(this HtmlHelpers<TModel> htmlHelper, string name)
        {
            return CheckBox(htmlHelper, name, false, null);
        }

        public static IHtmlString CheckBox<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, bool isChecked)
        {
            return CheckBox(htmlHelper, name, isChecked, null);
        }

        public static IHtmlString CheckBox<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, Object htmlAttributes)
        {
            return CheckBox(htmlHelper, name, false, CollectionExtensions.DictionaryFromAnonymousObject(htmlAttributes));
        }

        public static IHtmlString CheckBox<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, IDictionary<string, Object> htmlAttributes)
        {
            return CheckBox(htmlHelper, name, false, htmlAttributes);
        }

        public static IHtmlString CheckBox<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, bool isChecked, Object htmlAttributes)
        {
            return CheckBox(htmlHelper, name, isChecked, CollectionExtensions.DictionaryFromAnonymousObject(htmlAttributes));
        }

        public static IHtmlString CheckBox<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, bool isChecked, IDictionary<string, Object> htmlAttributes)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"<input type=""checkbox"" name=""{0}""", name);

            if (htmlAttributes != null)
                foreach (var htmlAttribute in htmlAttributes)
                {
                    sb.AppendFormat(@" {0}=""{1}""", htmlAttribute.Key, htmlAttribute.Value);
                }

            if (isChecked)
                sb.Append(@" checked=""checked""");

            sb.Append("/>");
            return new NonEncodedHtmlString(sb.ToString());
        }
    }
}
