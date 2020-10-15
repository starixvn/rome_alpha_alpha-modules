namespace Alpha.DNAide.Web
{
    using DNAide.Core;
    using System.Web;

    public class TplPage
    {
        public static string CCulture
        {
            get
            {
                return CastAide.AsString(HttpContext.Current.Session["Tpl_Culture"], "en-US");
            }
            set
            {
                HttpContext.Current.Session["Tpl_Culture"] = value;
            }
        }
    }
}
