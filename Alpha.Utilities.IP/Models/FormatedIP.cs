namespace Alpha.Utilities.IP.Model
{
    public class FormatedIP
    {
        public FormatedIP(string ip, bool allowOpenInfoPopup)
        {
            IP = ip;
            AllowOpenInfoPopup = allowOpenInfoPopup;
        }

        public string IP { get; }

        public bool AllowOpenInfoPopup { get; }
    }
}