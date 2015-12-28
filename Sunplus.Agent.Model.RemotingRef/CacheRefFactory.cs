namespace Sunplus.Agent.Model.RemotingRef
{
    public class CacheRefFactory
    {
        private static ICacheRefService _cacheRefService = new CacheRefService();

        public static ICacheRefService CacheRef
        {
            get { return _cacheRefService; }
            set { _cacheRefService = value; }
        }
    }
}