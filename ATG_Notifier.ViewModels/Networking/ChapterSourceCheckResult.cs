using ATG_Notifier.ViewModels.Models;

namespace ATG_Notifier.ViewModels.Networking
{
    public class ChapterSourceCheckResult
    {
        public ChapterSourceCheckResult(string sourceChapterId, ChapterProfileModel chapterProfileModel)
        {
            this.SourceChapterId = sourceChapterId;
            this.ChapterProfileModel = chapterProfileModel;
        }

        public string SourceChapterId { get; }

        public ChapterProfileModel ChapterProfileModel { get; }
    }
}
