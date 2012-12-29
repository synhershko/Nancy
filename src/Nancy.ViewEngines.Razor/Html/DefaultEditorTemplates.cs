using System;
using Nancy.Extensions;

namespace Nancy.ViewEngines.Razor.Html
{
    public interface IModelMetadata
    {
        
    }
    
    public interface IEditorTemplate
    {
        IHtmlString EditorTemplate<TModel>(HtmlHelpers<TModel> html, string htmlFieldName, object attributes = null);
    }

    public class DefaultEditorTemplates
    {
        public class DefaultStringEditorTemplate : IEditorTemplate
        {
            public IHtmlString EditorTemplate<TModel>(HtmlHelpers<TModel> html, string name, object attributes = null)
            {
                return html.TextBox(name, null
                                    /* TODO html.ViewContext.ViewData.TemplateInfo.FormattedModelValue */, attributes);
            }
        }

        public class DefaultPasswordEditorTemplate : IEditorTemplate
        {
            public IHtmlString EditorTemplate<TModel>(HtmlHelpers<TModel> html, string name, object attributes = null)
            {
                var atts = CollectionExtensions.DictionaryFromAnonymousObject(attributes);
                atts["password"] = "";
                return html.TextBox(name, null
                    /* TODO html.ViewContext.ViewData.TemplateInfo.FormattedModelValue */, attributes);
            }
        }

        public class DefaultIntegerEditorTemplate : IEditorTemplate
        {
            public IHtmlString EditorTemplate<TModel>(HtmlHelpers<TModel> html, string name, object attributes = null)
            {
                // TODO numeric formatting
                return html.TextBox(name, null
                    /* TODO html.ViewContext.ViewData.TemplateInfo.FormattedModelValue */, attributes);
            }
        }
    }
}
