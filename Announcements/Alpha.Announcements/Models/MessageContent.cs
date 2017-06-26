namespace Alpha.Announcements.Models
{
    public class MessageContent
    {
        public string LanguageKey { get; set; }

        public string Text { get; set; }

        public bool IsDisplayingByUrl { get; set; }

        public string Url { get; set; }
    }
}