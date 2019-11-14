using ATG_Notifier.ViewModels.Helpers;
using ATG_Notifier.ViewModels.Services;
using ATG_Notifier.ViewModels.Utilities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Popups;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace ATG_Notifier.UWP.Services
{
    public class WebService : IWebService
    {
        private static HttpBaseProtocolFilter filter;
        private static HttpClient httpClient;
        private static CancellationTokenSource cts;

        private readonly ILogService logService;

        public WebService(ILogService logService)
        {
            this.logService = logService ?? throw new ArgumentNullException(nameof(logService));

            filter = new HttpBaseProtocolFilter
            {
                CookieUsageBehavior = HttpCookieUsageBehavior.NoCookies,
            };

            httpClient = new HttpClient(filter);

            var headers = httpClient.DefaultRequestHeaders;
            headers.TryAppendWithoutValidation("Referer", "https://www.google.com");
            headers.TryAppendWithoutValidation("Cookie", $"ZHID={MathUtility.GetRandomHexNumber(32).ToUpper()};");

            cts = new CancellationTokenSource();
        }

        public async Task<HtmlDocument> DownloadHtmlContentAsync(string url, int retryAttempts = 0)
        {
            string chapterHtml = await DownloadRawContentAsync(url, retryAttempts);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(chapterHtml);

            return doc;
        }

        public async Task<string> DownloadRawContentAsync(string url, int retryAttempts = 0)
        {
            if (!UriHelper.TryCreateWebUri(url, out Uri uri))
            {
                throw new ArgumentException(nameof(url), "The specified url is not a valid HTTP URI scheme!");
            }

            if (retryAttempts < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(retryAttempts), "The number of possible retry attempts cannot be negative!");
            }

            // TODO: Implement retry attempts (using cancellation tokens?)

            try
            {
                var response = await httpClient.TryGetStringAsync(uri).AsTask(cts.Token);
                if (response.Succeeded)
                {
                    return await response.ResponseMessage.Content.ReadAsStringAsync().AsTask(cts.Token);
                }
                else
                {
                    throw response.ExtendedError;
                }
            }
            catch (TaskCanceledException tcex)
            {
                throw tcex;
            }
        }
    }
}
