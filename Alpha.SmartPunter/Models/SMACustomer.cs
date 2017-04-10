namespace Alpha.SmartPunter.Models
{
    public class SMACustomer
    {
        public SMACustomer()
            : this(false, false)
        {
        }

        public SMACustomer(bool isSmartPunter, bool hasDownlineSmartPunter)
        {
            IsSmartPunter = isSmartPunter;
            HasDownlineSmartPunter = hasDownlineSmartPunter;
        }

        public bool IsSmartPunter { get; }

        public bool HasDownlineSmartPunter { get; }
    }
}
