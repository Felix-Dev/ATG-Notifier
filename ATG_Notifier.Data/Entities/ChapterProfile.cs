using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ATG_Notifier.Data.Entities
{
    [Table("ChapterProfiles")]
    public class ChapterProfile : INotifyPropertyChanged
    {
        private bool isRead;

        public ChapterProfile() { }

        [Key]
        [DatabaseGenerat‌​ed(DatabaseGeneratedOption.None)]
        public long ChapterProfileId { get; set; }

        public int Number { get; set; }

        public string Title { get; set; }

        [Required]
        public string NumberAndTitleFallbackString { get; set; }

        public int WordCount { get; set; }

        [Required]
        public int ChapterSource { get; set; }

        [Required]
        public string Url { get; set; }

        public DateTime? ReleaseTime { get; set; }

        public DateTime AppArrivalTime { get; set; }

        public bool IsRead
        {
            get => this.isRead;
            set
            {
                if (value != this.isRead)
                {
                    this.isRead = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
