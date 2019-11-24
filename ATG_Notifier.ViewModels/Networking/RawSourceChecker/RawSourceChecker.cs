using ATG_Notifier.ViewModels.Models;
using ATG_Notifier.ViewModels.Services;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ATG_Notifier.ViewModels.Networking
{
    public class RawSourceChecker
    {
        private const string ATGRawSourcePollingUrl = "https://m.zongheng.com/h5/ajax/chapter/list?h5=1&bookId=408586&pageNum=1&pageSize=1&chapterId=0&asc=1";
        private const string ATGRawSourceBaseUrl = "http://book.zongheng.com/chapter/408586/";

        private const string JsonChapterNumberTitlePath = "chapterlist.chapters[0].chapterName";
        private const string JsonChapterIdPath = "chapterlist.chapters[0].chapterId";

        private readonly ILogService logService;
        private readonly IWebService webService;

        public RawSourceChecker(IWebService webService, ILogService logService)
        {
            this.webService = webService ?? throw new ArgumentNullException(nameof(webService));
            this.logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        public async Task<ChapterSourceCheckResult> GetUpdateAsync(string mostRecentChapterId)
        {
            string data;
            try
            {
                data = await webService.DownloadRawContentAsync(ATGRawSourcePollingUrl, 0);
            }
            catch (Exception ex) when (ex is AggregateException || ex is WebException || ex is HttpRequestException || ex is InvalidOperationException
                                       || ex is TaskCanceledException)
            {
                throw new SourceCheckerOperationFailedException("The request could not be handled. One or more (network) errors occured.", ex);
            }

            // The obtained data is saved in a JSON format. Parse the JSON string to obtain the latest chapter data.
            JObject json;
            try
            {
                json = JObject.Parse(data);
            }
            catch (JsonReaderException)
            {
                logService.Log(LogType.Error, "Downloaded chapter info is not in JSON format!\n");
                return null;
            }

            var chapterNumberAndTitle = (string)json.SelectToken(JsonChapterNumberTitlePath);
            if (chapterNumberAndTitle == null)
            {
                logService.Log(LogType.Error, "The current chapter information could not be retrieved!\n");
                return null;
            }

            var chapterId = (string)json.SelectToken(JsonChapterIdPath);
            if (chapterId == null)
            {
                logService.Log(LogType.Error, "The current chapter ID could not be retrieved!\n");
                return null;
            }

            // TODO: Check here if the latest retrieved chapter info differs from the most-recently obtained latest
            // chapter info.
            if (mostRecentChapterId == chapterId)
            {
                return null;
            }

            /* Build chapter url. */           
            string chapterUrl = ATGRawSourceBaseUrl + chapterId + ".html";

            /* Obtain chapter word count & release time */

            int wordCount = 0;
            DateTime? releaseTime = null;

            HtmlDocument doc = null;
            try
            {
                doc = await webService.DownloadHtmlContentAsync(chapterUrl);
            }
            catch (SourceCheckerOperationFailedException ex)
            {
                /* In case there was a Network error, wait 20 seconds for a retry. */
                logService.Log(LogType.Warning, $"Checking for a raw source chapter update failed!\nException: {ex.InnerException}\nTechnical details: {ex.InnerException.Message}");
            }

            // Read chapter word & release date if possible.
            var nodes = doc?.DocumentNode?.SelectNodes("//div[@class='bookinfo']//span");   
            if (nodes?.Count > 0)
            {
                // Read chapter word count if possible
                if (!int.TryParse(nodes[0].InnerText, out wordCount))
                {
                    logService.Log(LogType.Warning, $"Word count for chapter <{chapterId}> could not be obtained (perhaps the HTML layout changed?)!\n");
                }

                // Read chapter release date is possible.
                if (nodes?.Count > 1 && DateTime.TryParse(nodes[1].InnerText, out DateTime time))
                {
                    releaseTime = time;
                }
                else
                {
                    releaseTime = null;
                    logService.Log(LogType.Warning, $"Release time for chapter <{chapterId}> could not be obtained (perhaps the HTML layout changed?)!\n");
                }
            }

            /* Obtain separate chapter number and title of the newest chapter. */

            string chapterInfo;
            string chapterString;

            int chapterNumber = 0;
            string chapterTitle = null;

            #region Chapter-info Regex

            /* 
             * We assume that the string containing the chapter number and title looks like this:
             * 
             *      "第1208章 金乌圣剑"
             */
            Regex pattern = new Regex("(?<chapterInfo>[^\r\n]+)");
            Match match = pattern.Match(chapterNumberAndTitle);
            if (!match.Success)
            {
                var errorChapterProfileModel = new ChapterProfileModel()
                {
                    Number = 0,
                    Title = "",
                    NumberAndTitleFallbackString = chapterNumberAndTitle,
                    WordCount = 0,
                    Url = chapterUrl,
                    ReleaseTime = null,
                    AppArrivalTime = DateTime.Now,
                };

                return new ChapterSourceCheckResult(chapterId, errorChapterProfileModel);
            }

            StringBuilder chapterInfoBld = new StringBuilder(20);

            chapterInfo = match.Groups["chapterInfo"].Value;

            pattern = new Regex(@"第(?<chapter>\d+)章");
            match = pattern.Match(chapterInfo);
            if (match.Success)
            {
                chapterNumber = int.Parse(match.Groups["chapter"].Value);
                chapterInfoBld.Append($"{match.Groups["chapter"].Value}");
            }
            else
            {
                chapterInfoBld.Append($"{chapterInfo.Substring(0, chapterInfo.IndexOf(' '))}");
            }

            pattern = new Regex(" (?<title>[^\n]+$)");
            match = pattern.Match(chapterInfo);
            if (match.Success)
            {
                chapterTitle = match.Groups["title"].Value;
                chapterInfoBld.Append($" {match.Groups["title"].Value}");
            }
            else
            {
                chapterInfoBld.Append($" {chapterInfo.Substring(chapterInfo.IndexOf(' '))}");
            }

            chapterString = chapterInfoBld.ToString();

            #endregion // Chapter-info Regex

            var chapterProfileModel = new ChapterProfileModel()
            {
                Number = chapterNumber,
                Title = chapterTitle,
                NumberAndTitleFallbackString = chapterString,
                WordCount = wordCount,
                Source = ChapterSource.Zongheng,
                Url = chapterUrl,
                ReleaseTime = releaseTime,
                AppArrivalTime = DateTime.Now,
            };

            return new ChapterSourceCheckResult(chapterId, chapterProfileModel);
        }
    }
}
