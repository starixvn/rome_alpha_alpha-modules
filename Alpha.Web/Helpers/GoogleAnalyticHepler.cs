namespace Alpha.Web.Helpers
{
    using System.Web;
    using Configurations;

    public static class GoogleAnalyticHepler
    {
        private static readonly HtmlString EmptyHtml = new HtmlString(string.Empty);

        public static HtmlString Render()
        {
            return Render(AppSettings.GoogleAnalyticId);
        }

        public static HtmlString Render(string googleAnalyticId)
        {
            if (string.IsNullOrEmpty(googleAnalyticId))
            {
                return EmptyHtml;
            }

            return new HtmlString(@"<script>
                                      setTimeout(function(){
                                          (function(i,s,o,g,r,a,m){{i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){{
                                          (i[r].q=i[r].q||[]).push(arguments)}},i[r].l=1*new Date();a=s.createElement(o),
                                          m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
                                          }})(window,document,'script','" + AppSettings.GoogleAnalyticUrl + @"','ga');

                                          ga('create', '" + googleAnalyticId + @"', 'auto');
                                          ga('send', 'pageview');
                                      },200);
                                    </script>");
        }
    }
}