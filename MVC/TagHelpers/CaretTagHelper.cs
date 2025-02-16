using Common.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MVC.TagHelpers
{
    public class CaretTagHelper : TagHelper
    {
        public SortDirection SortDirection { get; set; }
        public bool Active { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder tag = new TagBuilder("i");
            tag.AddCssClass("bi");
            if (SortDirection == SortDirection.Asc)
            {
                if (Active)
                {
                    tag.AddCssClass("bi-caret-up-fill");
                }
                else
                {
                    tag.AddCssClass("bi-caret-up");
                }
            }
            else
            {
                if (Active)
                {
                    tag.AddCssClass("bi-caret-down-fill");
                }
                else
                {
                    tag.AddCssClass("bi-caret-down");
                }
            }

            output.Content.AppendHtml(tag);
        }
    }
}
