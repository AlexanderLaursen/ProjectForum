using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Formats.Asn1;
using MVC.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Webserver.TagHelpers
{
    /// <summary>
    /// Pagination der bruger ViewContext og PageInfo til at lave pagination
    /// </summary>
    public class PaginationTagHelper : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }
        public PageInfo PageInfo { get; set; }
        public int Id { get; set; }
        private bool PreviousPageExists => PageInfo.CurrentPage != 1;
        private bool NextPageExists => !(PageInfo.TotalPages <= PageInfo.CurrentPage);
        public string SearchString { get; set; }
        public int Iterator { get; set; } = 1;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Handle edgecase in comments with no items
            if (PageInfo.TotalItems == 0)
            {
                PageInfo.TotalItems = 1;
            }

            // Sætter første link til at være 2 mindre end denne side
            Iterator = PageInfo.CurrentPage - 2;
            if (Iterator < 1)
                Iterator = 1;

            // Ydre html tags
            TagBuilder navTag = new TagBuilder("nav");
            TagBuilder ulTag = new TagBuilder("ul");
            ulTag.AddCssClass("pagination");

            // Producerer tilbage knappen
            TagBuilder prevTag;
            if (PreviousPageExists)
            {
                prevTag = TagBuilderHelper(PageInfo.CurrentPage - 1, "Tilbage");
            }
            else
            {
                // Tilføjer disabled css
                prevTag = TagBuilderHelper(PageInfo.CurrentPage - 1, "Tilbage", "disabled");
            }
            ulTag.InnerHtml.AppendHtml(prevTag);

            // Hvis første side ikke vises indsættes der "..." med href til første side
            if (Iterator > 1)
            {
                TagBuilder firstPageTag = TagBuilderHelper(1, "...");
                ulTag.InnerHtml.AppendHtml(firstPageTag);
            }

            // Producerer (max) 5 knapper med href og CSS startende fra Iterator
            for (int i = Iterator; i <= PageInfo.TotalPages && i < Iterator + 5; i++)
            {
                TagBuilder tag;
                if (i == PageInfo.CurrentPage)
                {
                    // Tilføjer active CSS hvis det er nuværende side
                    tag = TagBuilderHelper(i, i.ToString(), "active");
                }
                else
                {
                    tag = TagBuilderHelper(i, i.ToString());
                }

                ulTag.InnerHtml.AppendHtml(tag);
            }

            // Hvis sidste side ikke vises indsættes der "..." med href til første side
            if (PageInfo.CurrentPage + 3 < PageInfo.TotalPages)
            {
                TagBuilder lastTag = TagBuilderHelper(PageInfo.TotalPages, "...");
                ulTag.InnerHtml.AppendHtml(lastTag);
            }

            // Producerer frem knappen
            TagBuilder nextTag;
            if (NextPageExists)
            {
                nextTag = TagBuilderHelper(PageInfo.CurrentPage + 1, "Frem");
            }
            else
            {
                nextTag = TagBuilderHelper(PageInfo.CurrentPage + 1, "Frem", "disabled");
            }
            ulTag.InnerHtml.AppendHtml(nextTag);

            navTag.InnerHtml.AppendHtml(ulTag);
            output.Content.AppendHtml(navTag);
        }

        /// <summary>
        /// Generer URL baseret på ViewContext (home, search eller topscorer
        /// </summary>
        /// <param name="targetPage">Sidenummer man gerne vil linke til</param>
        /// <returns></returns>
        private string GenerateUrl(int targetPage)
        {
            string currentUrl = ViewContext?.HttpContext.Request.Path.ToString()!;


            if (currentUrl.StartsWith("/Category"))
            {
                string url = $"/Category/{Id}/posts?page={targetPage}";
                if (PageInfo.PageSize != 10)
                {
                    url += $"&pageSize={PageInfo.PageSize}";
                }

                return url;
            }

            if (currentUrl.StartsWith("/Post/"))
            {
                string url = $"/Post/{Id}?page={targetPage}";
                if (PageInfo.PageSize != 10)
                {
                    url += $"&pageSize={PageInfo.PageSize}";
                }

                return url;
            }

            if (currentUrl.StartsWith("/CommentHistory"))
            {
                string url = $"/CommentHistory/{Id}?page={targetPage}";
                if (PageInfo.PageSize != 10)
                {
                    url += $"&pageSize={PageInfo.PageSize}";
                }
                return url;
            }

            if (currentUrl.StartsWith("/PostHistory"))
            {
                string url = $"/PostHistory/{Id}?page={targetPage}";
                if (PageInfo.PageSize != 10)
                {
                    url += $"&pageSize={PageInfo.PageSize}";
                }
                return url;
            }


            return string.Empty;
        }

        /// <summary>
        /// Producerer en knap med CSS og HREF
        /// </summary>
        /// <param name="targetPage"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private TagBuilder TagBuilderHelper(int targetPage, string text)
        {
            TagBuilder liTag = new TagBuilder("li");
            TagBuilder aTag = new TagBuilder("a");

            aTag.AddCssClass("page-link");
            liTag.AddCssClass("page-item");

            aTag.Attributes["href"] = GenerateUrl(targetPage);
            aTag.InnerHtml.Append(text);

            liTag.InnerHtml.AppendHtml(aTag);
            return liTag;
        }

        /// <summary>
        /// Producerer en knap med CSS og HREF samt ekstra CSS
        /// </summary>
        /// <param name="targetPage"></param>
        /// <param name="text"></param>
        /// <param name="extraCSS"></param>
        /// <returns></returns>
        private TagBuilder TagBuilderHelper(int targetPage, string text, string extraCSS)
        {
            TagBuilder liTag = new TagBuilder("li");
            TagBuilder aTag = new TagBuilder("a");

            aTag.AddCssClass("page-link");
            liTag.AddCssClass("page-item");
            liTag.AddCssClass(extraCSS);

            aTag.Attributes["href"] = GenerateUrl(targetPage);
            aTag.InnerHtml.Append(text);

            liTag.InnerHtml.AppendHtml(aTag);
            return liTag;
        }
    }
}
