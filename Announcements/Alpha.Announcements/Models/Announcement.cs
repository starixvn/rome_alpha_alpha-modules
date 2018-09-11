namespace Alpha.Announcements.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Specs;

    public enum AnnouncementType
    {
        Popup = 1,

        [Description("Header Popup")]
        HeaderPopup = 2,

        [Description("Emergency Popup")]
        EmergencyPopup = 3,

        [Description("Normal Announcement")]
        Normal = 4,
    }

    public class Announcement
    {
        public bool IsPopup { get; set; }

        public string Code { get; set; }

        public IEnumerable<MessageContent> Contents { get; set; }

        public int Id { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<ISpecification<IParam>> Specifications { get; set; }

        public string Title { get; set; }

        public IEnumerable<string> Environments { get; set; }

        public IEnumerable<string> Systems { get; set; }        

        public DateTime ReleasedDate { get; set; }

        public DateTime ExpiredDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatorId { get; set; }

        public string CreatorName { get; set; }

        public DateTime? LastEditDate { get; set; }

        public int? LastEditorId { get; set; }

        public string LastEditorName { get; set; }
    }
}