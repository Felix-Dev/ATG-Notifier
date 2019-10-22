using System;

namespace ATG_Notifier.Data.Entities
{
    internal class MenuNotification
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string Url { get; set; }

        /// <summary>
        ///
        /// </summary>
        public DateTime ArrivalTime { get; set; }

        public bool IsRead { get; set; }

        public MenuNotification() { }
    }
}
