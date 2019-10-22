using System;

namespace ATG_Notifier.Model
{
    public class MenuNotification
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string Url { get; set; }

        public DateTime ArrivalTime { get; set; }

        public bool IsRead { get; set; }

        public bool IsDirty { get; set; }

        public MenuNotification() { }
    }
}
