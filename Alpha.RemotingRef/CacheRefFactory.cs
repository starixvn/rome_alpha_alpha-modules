namespace Alpha.RemotingRef
{
    public class CacheRefFactory
    {
        private static CacheRefService _cacheRefService = new CacheRefServiceImpl();

        public static CacheRefService CacheRef
        {
            get { return _cacheRefService; }
            set { _cacheRefService = value; }
        }
    }
}