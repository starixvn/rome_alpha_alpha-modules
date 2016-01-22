namespace Fanex.Age.UI
{
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using Fanex.Security.SpamFilter;

    public abstract class TplPageSpamFilter : TplPageSecurity
    {
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            var spamFiler = (from blocker in
                                 HttpContext.Current.GetSpamFilterBlockers()
                             select blocker.Name).ToList();

            if (spamFiler.Count <= 0)
            {
                return;
            }

            var blockFilter = (from filter in spamFiler
                              where filter.ToLower().Contains(Security.Captcha.Entities.Page.Status.Blocked.ToString().ToLower())
                              select filter).FirstOrDefault();
            if (!string.IsNullOrEmpty(blockFilter))
            {
                ShowBlock(writer);
                return;
            }

            var captchaFilter = (from filter in spamFiler
                          where filter.ToLower().Contains(Security.Captcha.Entities.Page.Status.Captcha.ToString().ToLower())
                          select filter).FirstOrDefault();

            if (!string.IsNullOrEmpty(captchaFilter))
            {
                ShowCaptcha(writer, captchaFilter);
            }
        }

        private static void ShowBlock(HtmlTextWriter writer)
        {
            writer.Write(
                string.Format(
                "<script type=\"text/javascript\" src=\"{0}\"></script>",
                HttpContext.Current.Response.ApplyAppPathModifier("~/SpamFilter/Captcha/RenderCaptchaScript?mode=1")));
        }

        private static void ShowCaptcha(HtmlTextWriter writer, string rule)
        {
            rule = HttpUtility.HtmlEncode(rule);

            writer.Write(
                string.Format(
                "<script type=\"text/javascript\" src=\"{0}\"></script>",
                HttpContext.Current.Response.ApplyAppPathModifier("~/SpamFilter/Captcha/RenderCaptchaScript?mode=2&" + "rule=" + rule)));
        }
    }
}