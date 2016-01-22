namespace Fanex.Age.UI
{
    /// <summary>
    /// Each .ashx file inherits from this class will be automatically checked security 
    /// before doing more action with database
    /// List security need be verified:
    /// + Cross Site Forgery Attack
    /// </summary>
    public abstract class TplHandlerSecurity : TplHandler
    {
        public override object ValidateSecurityActions()
        {
            if (!AntiForgeryValidator.ValidateRequest())
            {
                return new Result(ErrorType.AccessDenied, "Validation of Anti-XSRF token failed. Please refresh your page. Thanks.");
            }

            return null;
        }
    }
}