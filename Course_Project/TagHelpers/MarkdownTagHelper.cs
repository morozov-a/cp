﻿using HeyRed.MarkdownSharp;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course_Project.TagHelpers
{
    public class MarkdownTagHelper : TagHelper
    {
        public string Source { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //add wrapper span with well known class
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "markdown-contents");

            //Add processed markup as the inner contents. Notice the usage of SetHtmlContent so the processed markup is not encoded
            var markdown = new Markdown();
            output.Content.SetHtmlContent(markdown.Transform(Source));
        }
    }
}
