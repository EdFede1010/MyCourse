using Microsoft.AspNetCore.Razor.TagHelpers;
using MyCourses.Models.Entities;

namespace MyCourses.Customization.TagHelpers
{
    public class RatingTagHelper : TagHelper
    {
        public double Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //double value = (double)context.AllAttributes["value"].Value;


            for (int i = 1; i <= 5; i++)
            {

                if (Value >= i)
                {
                    output.Content.AppendHtml("<i class=\"bi bi-star-fill\"></i>");
                }
                else if (Value > i - 1)
                {
                    output.Content.AppendHtml("<i class=\"bi bi-star-half\"></i>");
                }
                else
                {
                    output.Content.AppendHtml("<i class=\"bi bi-star\"></i>");
                }
            }
        }
    }
}