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

        public static HtmlString RenderGtagScript(string gtagId)
        {
            if (string.IsNullOrEmpty(gtagId))
            {
                return EmptyHtml;
            }

            return new HtmlString(@"<script>(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':
                                    new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],
                                    j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src=
                                    'https://www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);
                                    })(window,document,'script','dataLayer','" + gtagId + "');</script>");
        }

        public static HtmlString RenderGtagNoScript(string gtagId)
        {
            if (string.IsNullOrEmpty(gtagId))
            {
                return EmptyHtml;
            }

            return new HtmlString(@"<noscript><iframe src='https://www.googletagmanager.com/ns.html?id=" + gtagId + 
                                    " height='0' width='0' style='display:none;visibility:hidden'></iframe></noscript>");
        }
    }
}