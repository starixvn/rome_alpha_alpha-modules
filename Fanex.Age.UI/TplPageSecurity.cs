namespace Fanex.Age.UI
{
    using System.Web.UI;
    using Alpha.Web.Validators;

    /// <summary>
    /// Each .aspx file inherits from this class will be automatically add security information for checking later
    /// List security need be verified:
    /// + Cross Site Forgery Attack
    /// </summary>
    public abstract class TplPageSecurity : TplPage
    {
        protected override void RenderSecurityContent(HtmlTextWriter writer)
        {
            base.RenderSecurityContent(writer);

            writer.Write(AntiForgeryValidator.GenerateTokenInInputElement());
        }
    }
}