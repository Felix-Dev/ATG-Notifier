using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.ViewModels.Services
{
    public interface IWebService
    {
        Task<HtmlDocument> DownloadHtmlContentAsync(string url, int retryAttempts = 0);

        Task<string> DownloadRawContentAsync(string url, int retryAttempts = 0);
    }
}
