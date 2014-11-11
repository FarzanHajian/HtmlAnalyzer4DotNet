using System;
using System.Collections.Generic;

namespace HtmlAnalyzer4DotNet
{
    public class HtmlData
    {
        public string Url { get; set; }

        public string RawHtml { get; set; }

        public string Title { get; set; }

        public List<string> MetadataList { get; set; }

        public List<string> AnchorList { get; set; }

        public List<string> ImageList { get; set; }

        public List<string> CssClassList { get; set; }

        public List<string> CssRuleList { get; set; }

        public HtmlData()
        {
            MetadataList = new List<string>();
            AnchorList = new List<string>();
            ImageList = new List<string>();
            CssClassList = new List<string>();
            CssRuleList = new List<string>();
        }

        public void Clear()
        {
            Url = null;
            RawHtml = null;
            Title = null;
            MetadataList.Clear();
            AnchorList.Clear();
            ImageList.Clear();
            CssClassList.Clear();
            CssRuleList.Clear();
        }
    }
}