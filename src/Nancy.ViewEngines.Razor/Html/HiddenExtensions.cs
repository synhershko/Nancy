using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Nancy.Extensions;

namespace Nancy.ViewEngines.Razor.Html
{
    public static class HiddenExtensions
    {
        public static IHtmlString HiddenFor<TModel, TProperty>(this HtmlHelpers<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return HiddenFor(htmlHelper, expression, null);
        }

        public static IHtmlString HiddenFor<TModel, TProperty>(this HtmlHelpers<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Object htmlAttributes)
        {
            return HiddenFor(htmlHelper, expression, CollectionExtensions.DictionaryFromAnonymousObject(htmlAttributes));
        }

        public static IHtmlString HiddenFor<TModel, TProperty>(this HtmlHelpers<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, Object> htmlAttributes)
        {
            // get the html field name; probably need to run this through convention first
            var mi = expression.GetTargetMemberInfo();
            var htmlFieldName = mi.Name; /* TODO: normalize, conventions */

            return Hidden(htmlHelper, htmlFieldName, /* TODO: get value from Model */ null, htmlAttributes);
        }

        public static IHtmlString Hidden<TModel>(this HtmlHelpers<TModel> htmlHelper, string name)
        {
            return Hidden(htmlHelper, name, null, null);
        }

        public static IHtmlString Hidden<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, Object value)
        {
            return Hidden(htmlHelper, name, value, null);
        }

        public static IHtmlString Hidden<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, Object value, Object htmlAttributes)
        {
            return Hidden(htmlHelper, name, value, CollectionExtensions.DictionaryFromAnonymousObject(htmlAttributes));
        }

        public static IHtmlString Hidden<TModel>(this HtmlHelpers<TModel> htmlHelper, string name, Object value, IDictionary<string, Object> htmlAttributes)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(@"<input type=""hidden"" name=""{0}""", name);

            if (htmlAttributes != null)
                foreach (var htmlAttribute in htmlAttributes)
                {
                    sb.AppendFormat(@" {0}=""{1}""", htmlAttribute.Key, htmlAttribute.Value);
                }

            if (value != null)
                sb.AppendFormat(@" value=""{0}""", value);

            sb.Append("/>");
            return new NonEncodedHtmlString(sb.ToString());
        }
    }
}
