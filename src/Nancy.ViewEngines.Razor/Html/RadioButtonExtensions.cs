using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Nancy.Extensions;

namespace Nancy.ViewEngines.Razor.Html
{
    public static class RadioButtonExtensions
    {
        public static IHtmlString RadioButtonFor<TModel, TProperty>(this HtmlHelpers<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Object value)
        {
            return RadioButtonFor(htmlHelper, expression, value, null);
        }

        public static IHtmlString RadioButtonFor<TModel, TProperty>(this HtmlHelpers<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Object value, Object htmlAttributes)
        {
            return RadioButtonFor(htmlHelper, expression, value, CollectionExtensions.DictionaryFromAnonymousObject(htmlAttributes));
        }

        public static IHtmlString RadioButtonFor<TModel, TProperty>(this HtmlHelpers<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Object value, IDictionary<string, Object> htmlAttributes)
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

            return RadioButton(htmlHelper, htmlFieldName, /* TODO: get value from Model */ false, htmlAttributes);
        }

        public static IHtmlString RadioButton<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, Object value)
        {
            return RadioButton(htmlHelper, name, value, false, null);
        }

        public static IHtmlString RadioButton<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, Object value, bool isChecked)
        {
            return RadioButton(htmlHelper, name, value, isChecked, null);
        }

        public static IHtmlString RadioButton<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, Object value, IDictionary<string, Object> htmlAttributes)
        {
            return RadioButton(htmlHelper, name, value, false, htmlAttributes);
        }
        
        public static IHtmlString RadioButton<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, Object value, Object htmlAttributes)
        {
            return RadioButton(htmlHelper, name, value, false, CollectionExtensions.DictionaryFromAnonymousObject(htmlAttributes));
        }

        public static IHtmlString RadioButton<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, Object value, bool isChecked, Object htmlAttributes)
        {
            return RadioButton(htmlHelper, name, value, isChecked, CollectionExtensions.DictionaryFromAnonymousObject(htmlAttributes));
        }

        public static IHtmlString RadioButton<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, Object value, bool isChecked, IDictionary<string, Object> htmlAttributes)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"<input type=""radio"" name=""{0}""", name);

            if (htmlAttributes != null)
                foreach (var htmlAttribute in htmlAttributes)
                {
                    sb.AppendFormat(@" {0}=""{1}""", htmlAttribute.Key, htmlAttribute.Value);
                }

            if (value != null)
                sb.AppendFormat(@" value=""{0}""", value);

            if (isChecked)
                sb.Append(@" checked=""checked""");

            sb.Append("/>");
            return new NonEncodedHtmlString(sb.ToString());
        }
    }
}
