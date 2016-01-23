namespace Fanex.Age.UI
{
    using Fanex.Age.UI.Validators;

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
            var antiXSRFResult = AntiForgeryValidator.ValidateRequest();

            return antiXSRFResult.ErrCode == (int)ErrorType.Success ? null : antiXSRFResult;
        }
    }
}