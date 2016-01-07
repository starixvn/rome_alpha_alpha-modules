namespace Alpha.Auth.Services
{
    public interface IStatisticService
    {
        void UpdatePageVisit_Old(string url);

        void UpdateFunctionUsage(string url, string function, string subFunction, string site);

        void UpdateFunctionUsage(string function, string subFunction);
    }
}