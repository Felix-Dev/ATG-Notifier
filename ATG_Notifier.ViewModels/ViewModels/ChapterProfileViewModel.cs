using ATG_Notifier.ViewModels.Infrastructure;
using ATG_Notifier.ViewModels.Models;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ATG_Notifier.ViewModels.ViewModels
{
    public class ChapterProfileViewModel : ObservableObject
    {
        public ChapterProfileModel ChapterProfileModel { get; }

        public ChapterProfileViewModel(ChapterProfileModel chapterProfileModel)
        {
            ChapterProfileModel = chapterProfileModel ?? throw new ArgumentNullException(nameof(chapterProfileModel));
        }

        public long ChapterProfileId
        {
            get => ChapterProfileModel.ChapterProfileId;
            set => ChapterProfileModel.ChapterProfileId = value;
        }

        public int Number
        {
            get => ChapterProfileModel.Number;
            set => ChapterProfileModel.Number = value;
        }

        public string Title
        {
            get => ChapterProfileModel.Title;
            set => ChapterProfileModel.Title = value;
        }

        public string NumberAndTitleFallbackString
        {
            get => ChapterProfileModel.NumberAndTitleFallbackString;
            set => ChapterProfileModel.NumberAndTitleFallbackString = value;
        }

        public string NumberAndTitleDisplayString
        {
            get
            {
                return (this.Number == 0 || this.Title == null)
                    ? NumberAndTitleFallbackString
                    : $"Chapter {Number} - {Title}";
            }
        }

        public int WordCount
        {
            get => ChapterProfileModel.WordCount;
            set => ChapterProfileModel.WordCount = value;
        }

        public string Url
        {
            get => ChapterProfileModel.Url;
            set => ChapterProfileModel.Url = value;
        }

        public DateTime? ReleaseTime
        {
            get => ChapterProfileModel.ReleaseTime;
            set => ChapterProfileModel.ReleaseTime = value;
        }

        public DateTime ArrivalTime
        {
            get => ChapterProfileModel.AppArrivalTime;
            set => ChapterProfileModel.AppArrivalTime = value;
        }

        public bool IsRead
        {
            get => ChapterProfileModel.IsRead;
            set
            {
                if (value != ChapterProfileModel.IsRead)
                {
                    ChapterProfileModel.IsRead = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Uri SourceIcon
        {
            get
            {
                switch (ChapterProfileModel.Source)
                {
                    case ChapterSource.Zongheng:
                        return new Uri("ms-appx:///Assets/Images/logoZH.png");
                    case ChapterSource.Lnmtl:
                        return new Uri("ms-appx:///Assets/Images/logoLnmtl.png");
                    default:
                        // Use Placeholder image?
                        return null;
                }
            }
        }
    }
}
