using ATG_Notifier.ViewModels.Helpers;
using ATG_Notifier.ViewModels.Services;
using ATG_Notifier.ViewModels.Utilities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Services
{
    internal class WebService : IWebService
    {
        private readonly ILogService logService;

        public WebService(ILogService logService)
        {
            this.logService = logService;
        }

        public async Task<HtmlDocument> DownloadHtmlContentAsync(string url, int retryAttempts)
        {
            string chapterHtml = await DownloadRawContentAsync(url);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(chapterHtml);

            return doc;
        }

        //public HtmlDocument DownloadHtmlContent(string url)
        //{

        //    HtmlWeb web = new HtmlWeb
        //    {
        //        PreRequest = delegate (HttpWebRequest webRequest)
        //        {
        //            webRequest.Timeout = 10 * 1000;
        //            return true;
        //        }
        //    };

        //    HtmlDocument doc = null;

        //    while (true)
        //    {
        //        try
        //        {
        //            doc = await web.Load(url);
        //            break;
        //        }
        //        catch (WebException ex)
        //        {
        //            /* In case there was a Network error, wait 20 seconds for a retry. */
        //            logService.Log(LogType.Warning, $"Error downloading website content: {ex} - {ex.Message}\n");
        //            Thread.Sleep(20 * 1000);
        //        }

        //        catch (Exception ex)
        //        {
        //            logService.Log(LogType.Error, $"Error downloading website content: {ex} - {ex.Message}\n");
        //            Thread.Sleep(20 * 1000);
        //        }
        //    }

        //    return doc;
        //}

        //public async Task<string> DownloadRawContentAsync(string url)
        //{
        //    if (string.IsNullOrWhiteSpace(url))
        //    {
        //        throw new ArgumentException(nameof(url));
        //    }

        //    while (true)
        //    {
        //        // Call asynchronous network methods in a try/catch block to handle exceptions
        //        try
        //        {
        //            //return await httpClient.GetStringAsync(url);
        //        }
        //        catch (HttpRequestException ex)
        //        {
        //            /* In case there was a Network error, wait 20 seconds for a retry. */
        //            logService.Log(LogType.Error, $"Failed to download website content!\nException: {ex}\nTechnical details: {ex.Message}");
        //            Thread.Sleep(20 * 1000);
        //        }
        //        catch (TaskCanceledException tcex)
        //        {
        //            /* 
        //             * This exception is thrown when the request is cancelled or timed-out 
        //             * (see https://thomaslevesque.com/2018/02/25/better-timeout-handling-with-httpclient/) 
        //             * 
        //             * Since we don't cancel requests, the only cause can be a time-out.
        //             */

        //            logService.Log(LogType.Error, $"Failed to download website content (the request timed out)!\nException: {tcex}\nTechnical details: {tcex.Message}");
        //            Thread.Sleep(20 * 1000);
        //        }
        //    }
        //}

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
       
            while (retryAttempts-- >= 0)
            {
                var httpClientHandler = new HttpClientHandler
                {
                    UseCookies = false,
                };

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(15);

                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), uri))
                    {
                        request.Headers.TryAddWithoutValidation("Referer", "https://www.google.com");
                        request.Headers.TryAddWithoutValidation("Cookie", $"ZHID={MathUtility.GetRandomHexNumber(32).ToUpper()};");

                        try
                        {
                            HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);
                            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        }
                        catch (Exception ex) when (retryAttempts >= 0 && (ex is AggregateException || ex is WebException || ex is HttpRequestException || ex is InvalidOperationException))
                        {
                            /* In case there was a Network error, wait 20 seconds for a retry. */
                            logService.Log(LogType.Error, $"Failed to download website content!\nException: {ex}\nTechnical details: {ex.Message}");
                            Thread.Sleep(20 * 1000);
                        }
                        //catch (ThreadInterruptedException thintrex)
                        //{
                            
                        //}
                    }
                }
            }

            // We should not reach here based on our exception handling above (instead, the 
            // unhandled exception should be passed to the caller)
           return "";
        }

        public async Task<string> DownloadRawContentAsync2(string url)
        {
            if (!UriHelper.TryCreateWebUri(url, out Uri uri))
            {
                throw new ArgumentException(nameof(url), "The specified url is not a valid HTTP URI scheme!");
            }

            while (true)
            {
                var httpClientHandler = new HttpClientHandler
                {
                    UseCookies = false
                };

                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), uri))
                    {
                        request.Headers.TryAddWithoutValidation("Referer", "https://www.google.com");
                        request.Headers.TryAddWithoutValidation("Cookie", $"ZHID={MathUtility.GetRandomHexNumber(32).ToUpper()};");

                        // Call asynchronous network methods in a try/catch block to handle exceptions
                        try
                        {
                            var response = httpClient.SendAsync(request).Result;
                            return await response.Content.ReadAsStringAsync();
                        }
                        catch (Exception ex) when (ex is AggregateException || ex is WebException || ex is HttpRequestException || ex is InvalidOperationException)
                        {
                            /* In case there was a Network error, wait 20 seconds for a retry. */
                            logService.Log(LogType.Error, $"Failed to download website content!\nException: {ex}\nTechnical details: {ex.Message}");
                            Thread.Sleep(20 * 1000);
                        }
                        catch (TaskCanceledException tcex)
                        {
                            /* 
                             * This exception is thrown when the request is cancelled or timed-out 
                             * (see https://thomaslevesque.com/2018/02/25/better-timeout-handling-with-httpclient/) 
                             * 
                             * Since we don't cancel requests, the only cause can be a time-out.
                             */

                            logService.Log(LogType.Error, $"Failed to download website content (the request timed out)!\nException: {tcex}\nTechnical details: {tcex.Message}");
                            Thread.Sleep(20 * 1000);
                        }
                        catch (ThreadInterruptedException)
                        {
                            logService.Log(LogType.Info, "The web service was interrupted, perhaps as an intent to shutdown the notifier by the user.");
                            Thread.CurrentThread.Abort();
                        }
                        catch (ThreadAbortException)
                        {

                        }
                    }
                }
            }
        }
    }
}
