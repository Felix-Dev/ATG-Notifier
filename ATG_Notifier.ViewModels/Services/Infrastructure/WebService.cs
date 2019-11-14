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

namespace ATG_Notifier.ViewModels.Services
{
    //public class WebService
    //{
    //    private readonly ILogService logService;

    //    public WebService(ILogService logService)
    //    {
    //        this.logService = logService;
    //    }

    //    // TODO: method name doesn't match method logic (could open *any* process right now)
    //    public static void OpenWebsite(string url)
    //    {
    //        if (url is null || (!url.StartsWith("http:") && !url.StartsWith("https:")))
    //        {
    //            throw new ArgumentException(nameof(url), "The specified url is not a valid HTTP URI scheme!");
    //        }

    //        Process.Start(url);
    //    }

    //    //public HtmlDocument DownloadHtmlContent(string url)
    //    //{

    //    //    HtmlWeb web = new HtmlWeb
    //    //    {
    //    //        PreRequest = delegate (HttpWebRequest webRequest)
    //    //        {
    //    //            webRequest.Timeout = 10 * 1000;
    //    //            return true;
    //    //        }
    //    //    };

    //    //    HtmlDocument doc = null;

    //    //    while (true)
    //    //    {
    //    //        try
    //    //        {
    //    //            doc = await web.Load(url);
    //    //            break;
    //    //        }
    //    //        catch (WebException ex)
    //    //        {
    //    //            /* In case there was a Network error, wait 20 seconds for a retry. */
    //    //            logService.Log(LogType.Warning, $"Error downloading website content: {ex} - {ex.Message}\n");
    //    //            Thread.Sleep(20 * 1000);
    //    //        }

    //    //        catch (Exception ex)
    //    //        {
    //    //            logService.Log(LogType.Error, $"Error downloading website content: {ex} - {ex.Message}\n");
    //    //            Thread.Sleep(20 * 1000);
    //    //        }
    //    //    }

    //    //    return doc;
    //    //}

    //    public async Task<HtmlDocument> DownloadHtmlContentAsync(string url)
    //    {

    //        HtmlWeb web = new HtmlWeb
    //        {
    //            PreRequest = delegate (HttpWebRequest webRequest)
    //            {
    //                webRequest.Timeout = 10 * 1000;
    //                return true;
    //            }
    //        };

    //        HtmlDocument doc = null;

    //        try
    //        {
    //            doc = await web.LoadFromWebAsync(url);
    //        }
    //        catch (WebException ex)
    //        {
    //            /* In case there was a Network error, wait 20 seconds for a retry. */
    //            logService.Log(LogType.Warning, $"Failed to download website content!\nException: {ex}\nTechnical details: {ex.Message}");
    //        }

    //        catch (Exception ex)
    //        {
    //            logService.Log(LogType.Error, $"Failed to download website content!\nException: {ex}\nTechnical details: {ex.Message}");
    //        }

    //        return doc;
    //    }

    //    //public async Task<string> DownloadRawContentAsync(string url)
    //    //{
    //    //    if (string.IsNullOrWhiteSpace(url))
    //    //    {
    //    //        throw new ArgumentException(nameof(url));
    //    //    }

    //    //    while (true)
    //    //    {
    //    //        // Call asynchronous network methods in a try/catch block to handle exceptions
    //    //        try
    //    //        {
    //    //            //return await httpClient.GetStringAsync(url);
    //    //        }
    //    //        catch (HttpRequestException ex)
    //    //        {
    //    //            /* In case there was a Network error, wait 20 seconds for a retry. */
    //    //            logService.Log(LogType.Error, $"Failed to download website content!\nException: {ex}\nTechnical details: {ex.Message}");
    //    //            Thread.Sleep(20 * 1000);
    //    //        }
    //    //        catch (TaskCanceledException tcex)
    //    //        {
    //    //            /* 
    //    //             * This exception is thrown when the request is cancelled or timed-out 
    //    //             * (see https://thomaslevesque.com/2018/02/25/better-timeout-handling-with-httpclient/) 
    //    //             * 
    //    //             * Since we don't cancel requests, the only cause can be a time-out.
    //    //             */

    //    //            logService.Log(LogType.Error, $"Failed to download website content (the request timed out)!\nException: {tcex}\nTechnical details: {tcex.Message}");
    //    //            Thread.Sleep(20 * 1000);
    //    //        }
    //    //    }
    //    //}

    //    public async Task<string> DownloadRawContentAsync2(string url)
    //    {
    //        if (string.IsNullOrWhiteSpace(url))
    //        {
    //            throw new ArgumentException(nameof(url));
    //        }

    //        while (true)
    //        {
    //            var httpClientHandler = new HttpClientHandler
    //            {
    //                UseCookies = false
    //            };

    //            using (var httpClient = new HttpClient(httpClientHandler))
    //            {
    //                using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
    //                {
    //                    request.Headers.TryAddWithoutValidation("Referer", "https://www.google.com");
    //                    request.Headers.TryAddWithoutValidation("Cookie", $"ZHID={MathUtility.GetRandomHexNumber(32).ToUpper()};");

    //                    // Call asynchronous network methods in a try/catch block to handle exceptions
    //                    try
    //                    {
    //                        var response = httpClient.SendAsync(request).Result;
    //                        return await response.Content.ReadAsStringAsync();
    //                    }
    //                    catch (Exception ex) when (ex is AggregateException || ex is WebException || ex is HttpRequestException || ex is InvalidOperationException)
    //                    {
    //                        /* In case there was a Network error, wait 20 seconds for a retry. */
    //                        logService.Log(LogType.Error, $"Failed to download website content!\nException: {ex}\nTechnical details: {ex.Message}");
    //                        Thread.Sleep(20 * 1000);
    //                    }
    //                    catch (TaskCanceledException tcex)
    //                    {
    //                        /* 
    //                         * This exception is thrown when the request is cancelled or timed-out 
    //                         * (see https://thomaslevesque.com/2018/02/25/better-timeout-handling-with-httpclient/) 
    //                         * 
    //                         * Since we don't cancel requests, the only cause can be a time-out.
    //                         */

    //                        logService.Log(LogType.Error, $"Failed to download website content (the request timed out)!\nException: {tcex}\nTechnical details: {tcex.Message}");
    //                        Thread.Sleep(20 * 1000);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}
