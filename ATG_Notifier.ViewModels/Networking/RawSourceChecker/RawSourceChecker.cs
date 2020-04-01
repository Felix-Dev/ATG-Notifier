using ATG_Notifier.ViewModels.Models;
using ATG_Notifier.ViewModels.Services;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ATG_Notifier.ViewModels.Networking
{
    public class RawSourceChecker
    {
        private const string ATGRawSourcePollingUrl = "https://m.zongheng.com/h5/ajax/chapter/list?h5=1&bookId=408586&pageNum=1&pageSize=1&chapterId=0&asc=1";
        private const string ATGRawSourceBaseUrl = "http://book.zongheng.com/chapter/408586/";

        private readonly ILogService logService;
        private readonly IWebService webService;

        public RawSourceChecker(IWebService webService, ILogService logService)
        {
            this.webService = webService;
            this.logService = logService;
        }

        public async Task<ChapterSourceCheckResult?> GetUpdateAsync(string mostRecentChapterId)
        {
            string data;
            try
            {
                data = await webService.DownloadRawContentAsync(ATGRawSourcePollingUrl, 0);
            }
            catch (Exception ex) when (ex is AggregateException || ex is WebException || ex is HttpRequestException || ex is InvalidOperationException
                                       || ex is OperationCanceledException)
            {
                throw new SourceCheckerOperationFailedException("The request could not be handled. One or more (network) errors occured.", ex);
            }

            // The obtained data is saved in a JSON format. Parse the JSON string to obtain the latest chapter data.
            JsonDocument document;
            try
            {
                document = JsonDocument.Parse(data);
            }
            catch (JsonException)
            {
                logService.Log(LogType.Error, "Downloaded chapter info is not in JSON format!\n");
                return null;
            }

            // We assume the following JSON layout: 
            // chapterId = chapterlist.chapters[0].chapterId
            // chapterName = chapterlist.chapters[0].chapterName

            if (!document.RootElement.TryGetProperty("chapterlist", out JsonElement chapterListNode)
                || !chapterListNode.TryGetProperty("chapters", out JsonElement chapters)
                || chapters.GetArrayLength() == 0)
            {
                logService.Log(LogType.Error, "Downloaded chapter info has a different structure! The app might need to be updated.\n");
                return null;
            }

            // Obtain the chapter ID used to determine if a new chapter is available.
            if (!chapters[0].TryGetProperty("chapterId", out JsonElement idElement))
            {
                logService.Log(LogType.Error, "The current chapter ID could not be retrieved!\n");
                return null;
            }
            string chapterId = idElement.ToString();

            // Obtain the chapter number and title.
            if (!chapters[0].TryGetProperty("chapterName", out JsonElement nameElement))
            {
                logService.Log(LogType.Error, "The current chapter name could not be retrieved!\n");
                return null;
            }
            var chapterNumberAndTitle = nameElement.ToString();

            // Dispose of the JSON document object to prevent memory leaks.
            document.Dispose();

            // Check if a new chapter has been released. If the chapter ID differs, a new chapter is available and we proceed to fetch chapter-specific data. 
            // If not, we hve nothing to do here any longer and return.
            if (mostRecentChapterId == chapterId)
            {
                return null;
            }

            // Build chapter url.          
            string chapterUrl = ATGRawSourceBaseUrl + chapterId + ".html";

            // Next code block: Obtain chapter word count & release time.

            int wordCount = 0;
            DateTime? releaseTime = null;

            HtmlDocument doc;
            try
            {
                doc = await webService.DownloadHtmlContentAsync(chapterUrl);
            }
            catch (SourceCheckerOperationFailedException ex)
            {
                throw ex;
            }

            // Read chapter word & release date if possible.
            var nodes = doc.DocumentNode.SelectNodes("//div[@class='bookinfo']//span");   
            if (nodes?.Count > 0)
            {
                // Read chapter word count if possible.
                if (!int.TryParse(nodes[0].InnerText, out wordCount))
                {
                    logService.Log(LogType.Warning, $"Word count for chapter <{chapterId}> could not be obtained (perhaps the HTML layout changed?)!\n");
                }

                // Read chapter release date is possible.
                if (nodes.Count > 1 && DateTime.TryParse(nodes[1].InnerText, out DateTime time))
                {
                    releaseTime = time;
                }
                else
                {
                    releaseTime = null;
                    logService.Log(LogType.Warning, $"Release time for chapter <{chapterId}> could not be obtained (perhaps the HTML layout changed?)!\n");
                }
            }

            // Next code block: Obtain separate chapter number and title of the newest chapter.

            string chapterInfo;
            string chapterString;

            int chapterNumber = 0;
            string? chapterTitle = null;

            #region Chapter-info Regex

            // Regex info: We assume the string containing the chapter number and title looks like this:
            //      "第1208章 金乌圣剑"

            Regex pattern = new Regex("(?<chapterInfo>[^\r\n]+)");
            Match match = pattern.Match(chapterNumberAndTitle);
            if (!match.Success)
            {
                var errorChapterProfileModel = new ChapterProfileModel()
                {
                    Number = 0,
                    Title = null,
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
