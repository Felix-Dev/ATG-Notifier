using ATG_Notifier.Model;
using ATG_Notifier.UI;
using ATG_Notifier.UI.Model;
using ATG_Notifier.Utilities;
using ATG_Notifier.ViewModels.Services;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ATG_Notifier.Controller
{
    public class UpdateWorker
    {
        //private readonly string ATG_MAINPAGE_URL = "http://book.zongheng.com/book/408586.html";
        private const string ATG_NEWEST_CHAPTER_INFO_URL = "https://m.zongheng.com/h5/ajax/chapter/list?h5=1&bookId=408586&pageNum=1&pageSize=1&chapterId=0&asc=1";
        private const string ATG_CHAPTER_BASE_URL = "http://book.zongheng.com/chapter/408586/";

        private readonly EventWaitHandle jobRoundFinished;
        private readonly Thread workThread;
        private bool finishJob;

        private string chapterString;
        private string chapterUrl;

        private readonly ToastNotificationManager notifier = ToastNotificationManager.Instance;

        private readonly NotificationsController notificationsController = NotificationsController.Instance;

        private readonly WebService webService;

        private enum UpdateAvailable
        {
            NO,
            YES
        }

        #region Creation

        private static readonly Lazy<UpdateWorker> lazy = new Lazy<UpdateWorker>(() => new UpdateWorker());

        public static UpdateWorker Instance => lazy.Value;

        private UpdateWorker()
        {
            /* Create the worker thread used to check for updates. */
            workThread = new Thread(new ThreadStart(DoJob));
#if DEBUG
            workThread.Name = "worker"; // Assign worker thread a name for easier debugging.
#endif
            /* Create the job-round Event Handler */
            jobRoundFinished = new EventWaitHandle(true, EventResetMode.ManualReset);

            // TODO: Bad code.
            webService = new WebService(Program.CommonServices.LogService);
        }

        #endregion // Creation

        public void Start()
        {
            ThreadState state = workThread.ThreadState;

            // Only have one worker at a time.
            if (state == ThreadState.Unstarted || state == ThreadState.Stopped)
            {
                workThread.Start();
            }
        }

        public void Finish()
        {
            /* Wait until a running update process is finished. */
            jobRoundFinished.WaitOne();

            finishJob = true;

            workThread.Interrupt();

            /* Wait for the worker thread to terminate. */
            workThread.Join();

            /* Clean-up resources used by the event handler. */
            jobRoundFinished.Dispose();
        }

        private async void DoJob()
        {
            try
            {
                while (!finishJob)
                {
                    UpdateAvailable status = await CheckForUpdate2();
                    if (status == UpdateAvailable.YES)
                    {
                        //Settings settings = Settings.Instance;
#if DEBUG                        
                        MenuNotification notification = new MenuNotification()
                        {
                            Title = "ATG Chapter Update! (DEBUG)",
                            Body = chapterString,
                            Url = chapterUrl, /* settings.ChapterUrl */
                            ArrivalTime = DateTime.Now
                        };
#else
                        MenuNotification notification = new MenuNotification()
                        {
                            Title = "ATG Chapter Update!",
                            Body = chapterString,
                            Url = chapterUrl, /* settings.ChapterUrl */
                            ArrivalTime = DateTime.Now
                        };                        
#endif
                        notificationsController.Add(notification);

                        AppFeedbackManager.FlashApplicationTaskbarButton();

                        jobRoundFinished.Set();

                        if (!Settings.Instance.DoNotDisturb)
                        {
                            notifier.Show(notification);
                        }
                    }

#if DEBUG
                    Thread.Sleep(1 * 10 * 1000);
#else
                    /* Wait for 30 seconds until the next update check. */
                    Thread.Sleep(1 * 30 * 1000);
#endif
                }
            }
            catch (ThreadInterruptedException)
            {
                /* Terminate the worker thread (which is the current thread). */
                workThread.Abort();
            }
        }

        //        private UpdateCheckResult _CheckForUpdate()
        //        {
        //            HtmlDocument doc = webService.DownloadHtmlContent(ATG_MAINPAGE_URL);

        //            /* Obtain the URL of the latest published chapter. */
        //            HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@class='tit']//a");

        //            /* 
        //             * Normally, the node should not be "null". However, in some cases where the 
        //             * the user is re-directed to a another page - i.e. need to provide log-in
        //             * information first for internet access - the obtained website content is 
        //             * NOT the targeted webpage. In this case the node is likely to be "null".
        //             */
        //            if (node == null || node.Attributes["href"] == null)
        //            {
        //                Program.CommonServices.LogService.Log(LogType.Error, "The current chapter information wasn't found on the website!");
        //                return UpdateCheckResult.NO;
        //            }

        //            string newChapUrl = node.Attributes["href"].Value;

        //            /*
        //             * Compare that URL with the latest local chapter URL. If they differ
        //             * then a new chapter has been released and we update the local URL 
        //             * with the newest one. 
        //             * We also grab the chapter number of the newest chapter to print it out
        //             * in a notification.
        //             */
        //#if DEBUG
        //            if (true)
        //#else
        //            if (!newChapUrl.Equals(Settings.Instance.CurrentChapterUrl))
        //#endif
        //            {
        //                /*
        //                 * A new update was found. Make sure the app is not closed before
        //                 * this update was saved. It should be displayed to the user on next program launch.
        //                 * The update-job is set to "done" when the current update has been saved as a notification
        //                 * to be displayed.
        //                 * 
        //                 * If we don't synchronize the update process, the user might potentially miss out an
        //                 * update. Consider this scenario:
        //                 *      The user closes the app right after the new chapter url has been saved. Thus, the notification
        //                 *      for this update won't be shown and it also won't be shown on the next program startup because
        //                 *      it won't get recongnized as a new update.
        //                 * 
        //                 */
        //                jobRoundFinished.Reset();

        //                Settings.Instance.CurrentChapterUrl = newChapUrl;

        //                /* Obtain the chapter number of the newest chapter. */
        //                string chapterRaw = node.InnerText;

        //                string chapterInfo;

        //                /* 
        //                 * We assume that the string containing the chapter number and title looks like this:
        //                 * 
        //                 *      "第1208章 金乌圣剑"
        //                 */

        //                #region Chapter-info Regex

        //                Regex pattern = new Regex("(?<chapterInfo>[^\r\n]+)");
        //                Match match = pattern.Match(chapterRaw);
        //                if (!match.Success)
        //                {
        //                    chapterString = "Unrecognized chapter";
        //                    return UpdateCheckResult.YES;
        //                }

        //                StringBuilder chapterInfoBld = new StringBuilder(20);

        //                chapterInfo = match.Groups["chapterInfo"].Value;

        //                pattern = new Regex(@"第(?<chapter>\d+)章");
        //                match = pattern.Match(chapterInfo);
        //                if (match.Success)
        //                {
        //                    chapterInfoBld.Append($"Chapter {match.Groups["chapter"].Value}");
        //                }
        //                else
        //                {
        //                    chapterInfoBld.Append($"Chapter {chapterInfo.Substring(0, chapterInfo.IndexOf(' '))}");
        //                }

        //                pattern = new Regex(" (?<title>[^\n]+$)");
        //                match = pattern.Match(chapterInfo);
        //                if (match.Success)
        //                {
        //                    chapterInfoBld.Append($" - {match.Groups["title"].Value}");
        //                }
        //                else
        //                {
        //                    chapterInfoBld.Append($" - {chapterInfo.Substring(chapterInfo.IndexOf(' '))}");
        //                }

        //                chapterString = chapterInfoBld.ToString();

        //                #endregion // Chapter-info Regex

        //                return UpdateCheckResult.YES;
        //            }

        //            return UpdateCheckResult.NO;
        //        }

        private async Task<UpdateAvailable> CheckForUpdate2()
        {
            /* Obtain info latest published chapter. */
            var data = await webService.DownloadRawContentAsync2(ATG_NEWEST_CHAPTER_INFO_URL);

            JObject json;
            try
            {
                json = JObject.Parse(data);
            }
            catch (JsonReaderException)
            {
                Program.CommonServices.LogService.Log(LogType.Error, "Downloaded chapter info is not in JSON format!");
                return UpdateAvailable.NO;
            }

            string chapterNumberAndTitle = (string)json.SelectToken("chapterlist.chapters[0].chapterName");
            if (chapterNumberAndTitle == null)
            {
                Program.CommonServices.LogService.Log(LogType.Error, "The current chapter information could not be retrieved!");
                return UpdateAvailable.NO;
            }

            /*
             * Compare that URL with the latest local chapter URL. If they differ
             * then a new chapter has been released and we update the local URL 
             * with the newest one. 
             * We also grab the chapter number of the newest chapter to print it out
             * in a notification.
             */
#if DEBUG
            if (true)
#else
            if (!chapterNumberAndTitle.Equals(Settings.Instance.CurrentChapterNumberAndTitle))
#endif
            {
                /*
                 * A new update was found. Make sure the app is not closed before
                 * this update was saved. It should be displayed to the user on next program launch.
                 * The update-job is set to "done" when the current update has been saved as a notification
                 * to be displayed.
                 * 
                 * If we don't synchronize the update process, the user might potentially miss out an
                 * update. Consider this scenario:
                 *      The user closes the app right after the new chapter url has been saved. Thus, the notification
                 *      for this update won't be shown and it also won't be shown on the next program startup because
                 *      it won't get recongnized as a new update.
                 * 
                 */
                jobRoundFinished.Reset();

                Settings.Instance.CurrentChapterNumberAndTitle = chapterNumberAndTitle;

                /* Build chapter url. */
                var chapterId = (string)json.SelectToken("chapterlist.chapters[0].chapterId");
                chapterUrl = ATG_CHAPTER_BASE_URL + chapterId + ".html";

                /* Obtain separate chapter number and title of the newest chapter. */

                string chapterInfo;

                /* 
                 * We assume that the string containing the chapter number and title looks like this:
                 * 
                 *      "第1208章 金乌圣剑"
                 */

                #region Chapter-info Regex

                Regex pattern = new Regex("(?<chapterInfo>[^\r\n]+)");
                Match match = pattern.Match(chapterNumberAndTitle);
                if (!match.Success)
                {
                    chapterString = "Unrecognized chapter";
                    return UpdateAvailable.YES;
                }

                StringBuilder chapterInfoBld = new StringBuilder(20);

                chapterInfo = match.Groups["chapterInfo"].Value;

                pattern = new Regex(@"第(?<chapter>\d+)章");
                match = pattern.Match(chapterInfo);
                if (match.Success)
                {
                    chapterInfoBld.Append($"Chapter {match.Groups["chapter"].Value}");
                }
                else
                {
                    chapterInfoBld.Append($"Chapter {chapterInfo.Substring(0, chapterInfo.IndexOf(' '))}");
                }

                pattern = new Regex(" (?<title>[^\n]+$)");
                match = pattern.Match(chapterInfo);
                if (match.Success)
                {
                    chapterInfoBld.Append($" - {match.Groups["title"].Value}");
                }
                else
                {
                    chapterInfoBld.Append($" - {chapterInfo.Substring(chapterInfo.IndexOf(' '))}");
                }

                chapterString = chapterInfoBld.ToString();

                #endregion // Chapter-info Regex

                return UpdateAvailable.YES;
            }

            return UpdateAvailable.NO;
        }
    }
}
