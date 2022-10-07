﻿using AngleSharp.Html.Dom;
using Ganss.XSS;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace MyCourses.Customization.TagHelpers
{
    [HtmlTargetElement(Attributes = "html-sanitize")]
    public class HtmlSanitizeTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            //Otteniamo il contenuto del tag
            TagHelperContent tagHelperContent = await output.GetChildContentAsync(NullHtmlEncoder.Default);
            string content = tagHelperContent.GetContent(NullHtmlEncoder.Default);

            var sanitizer = CreateSanitizer();
            content = sanitizer.Sanitize(content);

            //Reimpostiamo il contenuto del tag
            output.Content.SetHtmlContent(content);
        }

        private static HtmlSanitizer CreateSanitizer()
        {
            var sanitizer = new HtmlSanitizer();
            
            //Tag consentiti
            sanitizer.AllowedTags.Clear();
            sanitizer.AllowedTags.Add("b");
            sanitizer.AllowedTags.Add("i");
            sanitizer.AllowedTags.Add("p");
            sanitizer.AllowedTags.Add("br");
            sanitizer.AllowedTags.Add("ul");
            sanitizer.AllowedTags.Add("ol");
            sanitizer.AllowedTags.Add("li");
            sanitizer.AllowedTags.Add("span");
            sanitizer.AllowedTags.Add("iframe");

            //Attributi consentiti
            sanitizer.AllowedAttributes.Clear();
            sanitizer.AllowedAttributes.Add("src");
            sanitizer.AllowedAttributes.Add("style");
            sanitizer.AllowDataAttributes = false;

            //Stili consentiti
            sanitizer.AllowedCssProperties.Clear();
            sanitizer.AllowedCssProperties.Add("background-color:rgb");

            sanitizer.FilterUrl += FilterUrl;
            sanitizer.PostProcessNode += ProcessIFrame;

            return sanitizer;
        }

        private static void FilterUrl (object sender, FilterUrlEventArgs filterUrlEventArgs)
        {
            if (!filterUrlEventArgs.OriginalUrl.StartsWith("//www.youtube.com/") && !filterUrlEventArgs.OriginalUrl.StartsWith("https://www.youtube.com/"))
            {
                filterUrlEventArgs.SanitizedUrl = null;
            }
        }

        private static void ProcessIFrame(object sender, PostProcessNodeEventArgs postProcessNodeEventArgs)
        {
            var iframe = postProcessNodeEventArgs.Node as IHtmlInlineFrameElement;
            if (iframe == null)
            {
                return;
            }
            var container = postProcessNodeEventArgs.Document.CreateElement("span");
            container.ClassName = "video-container";
            container.AppendChild(iframe.Clone(true));
            postProcessNodeEventArgs.ReplacementNodes.Add(container);
        }
    }
}
