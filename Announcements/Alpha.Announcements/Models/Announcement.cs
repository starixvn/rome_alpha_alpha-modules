namespace Alpha.Announcements.Models
{
    using System;
    using System.Collections.Generic;
    using Specs;

    public enum AnnouncementType
    {
        Popup = 1,
        HeaderPopup = 2
    }

    public class Announcement
    {
        public AnnouncementType AnnouncementType { get; set; }

        public string Code { get; set; }

        public IEnumerable<MessageContent> Contents { get; set; }

        public int Id { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<ISpecification<IParam>> Specifications { get; set; }

        public string Title { get; set; }

        public IEnumerable<string> Environments { get; set; }

        public IEnumerable<string> Systems { get; set; }

        public int Ordinal { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}