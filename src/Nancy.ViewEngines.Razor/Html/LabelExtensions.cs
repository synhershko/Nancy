using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Nancy.Extensions;

namespace Nancy.ViewEngines.Razor.Html
{
    public static class LabelExtensions
    {
        public static IHtmlString LabelFor<TModel, TValue>(this HtmlHelpers<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return LabelFor(html, expression, null);
        }

        public static IHtmlString LabelFor<TModel, TValue>(this HtmlHelpers<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return LabelFor(html, expression, CollectionExtensions.DictionaryFromAnonymousObject(htmlAttributes));
        }

        public static IHtmlString LabelFor<TModel, TValue>(this HtmlHelpers<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
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

            var sb = new StringBuilder();
            sb.AppendFormat(@"<label for=""{0}""", htmlFieldName);

            if (htmlAttributes != null)
                foreach (var htmlAttribute in htmlAttributes)
                {
                    sb.AppendFormat(@" {0}=""{1}""", htmlAttribute.Key, htmlAttribute.Value);
                }

            sb.AppendFormat(">{0}</label>", labelText);
            return new NonEncodedHtmlString(sb.ToString());
        }
    }
}
