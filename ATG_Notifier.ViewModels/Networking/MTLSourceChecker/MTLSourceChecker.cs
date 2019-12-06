using ATG_Notifier.ViewModels.Models;
using ATG_Notifier.ViewModels.Services;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.ViewModels.Networking
{
    public class MTLSourceChecker
    {
        private const string MTLSourcePollingUrl = "https://lnmtl.com/novel/against-the-gods";

        private readonly ILogService logService;
        private readonly IWebService webService;

        public MTLSourceChecker(IWebService webService, ILogService logService)
        {
            this.webService = webService;
            this.logService = logService;
        }

        public async Task<ChapterSourceCheckResult?> GetUpdateAsync(string mostRecentChapterId)
        {
            HtmlDocument doc;
            try
            {
                doc = await webService.DownloadHtmlContentAsync(MTLSourcePollingUrl);
            }
            catch (Exception ex)
            {
                throw new SourceCheckerOperationFailedException("The request could not be handled. One or more (network) errors occured.", ex);
            }

            var nodes = doc.DocumentNode?.SelectNodes("//div[@class='panel panel-default']");
            if (nodes == null)
            {
                logService.Log(LogType.Error, "The MTL chapter info could not be retrieved! The website's HTML layout changed.\n");
                throw new SourceCheckerOperationFailedException("The MTL chapter info could not be retrieved!");
            }

            foreach (var cNode in nodes)
            {
                if (cNode.SelectSingleNode("//h3[@class='panel-title']")?.InnerText != "Latest Chapters")
                {
                    continue;
                }

                HtmlNode? chapterNode = cNode.SelectSingleNode("//a[@class='chapter-link']");
                if (chapterNode == null)
                {
                    logService.Log(LogType.Error, "The MTL chapter info could not be retrieved! The website's HTML layout changed.\n");
                    throw new SourceCheckerOperationFailedException("The MTL chapter info could not be retrieved!");
                }

                // Retrieve the chapter url

                string chapterUrl = chapterNode.GetAttributeValue("href", null);
                if (chapterUrl == null)
                {
                    logService.Log(LogType.Error, "The MTL chapter url could not be retrieved! The website's HTML layout changed.\n");
                    throw new SourceCheckerOperationFailedException("The chapter URL could not be retrieved.");
                }

                // Check if a new chapter update is available

                string chapterId = chapterUrl;
                if (mostRecentChapterId == chapterId)
                {
                    return null;
                }

                // Retrieve the chapter number

                string? chapterNumberString = chapterNode.SelectSingleNode("//span[@class='badge chapter-badge']")?.InnerText;
                if (chapterNumberString == null 
                    || chapterNumberString.Length < 1 
                    || !int.TryParse(chapterNumberString.Substring(1), out int chapterNumber))
                {
                    logService.Log(LogType.Error, "The MTL chapter number could not be retrieved! The website's HTML layout changed.\n");
                    throw new SourceCheckerOperationFailedException("The MTL chapter number could not be retrieved!");
                }

                // Retrieve the chapter title

                string chapterTitle = "";
                if (chapterNode.ChildNodes == null || chapterNode.ChildNodes.Count < 3 || string.IsNullOrEmpty(chapterNode.ChildNodes[2]?.InnerText))
                {
                    logService.Log(LogType.Error, "The MTL chapter title could not be retrieved! The website's HTML layout changed.\n");
                    throw new SourceCheckerOperationFailedException("The MTL chapter title could not be retrieved!");
                }

                chapterTitle = chapterNode.ChildNodes[2].InnerText.Trim();

                // Check if chapter translation is available and release date

                DateTime? releaseTime = null;
                HtmlNode? metadataNode = cNode.SelectSingleNode("//td[@class='text-right']");
                if (metadataNode != null)
                {
                    if (metadataNode.SelectSingleNode("//span[@class='label label-warning']") is HtmlNode warningNode
                        && warningNode.InnerText == "Untranslated")
                    {
                        // Chapter not yet translated
                        logService.Log(LogType.Info, "Found a new MTL chapter update but the chapter is not yet translated.\n");
                        return null;
                    }

                    // Get release time

                    if (metadataNode.SelectSingleNode("//span[@class='label label-default']") is HtmlNode releaseDateNode
                        && DateTime.TryParse(releaseDateNode.InnerText, out DateTime tReleaseTime))
                    {
                        releaseTime = tReleaseTime;
                    }
                    else
                    {
                        logService.Log(LogType.Warning, "Could not retrieve the release time of the MTL chapter.\n");
                    }
                }
                else
                {
                    logService.Log(LogType.Warning, "Could not retrieve potential <untranslated> node and <elease time> node!\n");
                }

                var chapterProfileModel = new ChapterProfileModel()
                {
                    Number = chapterNumber,
                    Title = chapterTitle,
                    NumberAndTitleFallbackString = "",
                    Source = ChapterSource.Lnmtl,
                    Url = chapterUrl,
                    ReleaseTime = releaseTime,
                    AppArrivalTime = DateTime.Now,
                };

                return new ChapterSourceCheckResult(chapterId, chapterProfileModel);
            }

            return null;
        }
    }
}
