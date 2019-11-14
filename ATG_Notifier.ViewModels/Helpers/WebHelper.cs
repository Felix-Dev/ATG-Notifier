using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.ViewModels.Helpers
{
    public static class WebHelper
    {
        public static HtmlDocument GetHTMLFromText(string content)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);

            return htmlDoc;
        }
    }
}
