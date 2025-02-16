using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MVC.TagHelpers
{
    public class DisplayDateTimeTagHelper : TagHelper
    {
        public DateTime DateTime { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder tag = new TagBuilder("span");

            tag.InnerHtml.Append(DateTime.ToLocalTime().ToString("dd/MM/yyyy - HH:mm"));

            output.Content.AppendHtml(tag);
        }
    }
}
