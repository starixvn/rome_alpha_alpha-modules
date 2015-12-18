namespace Fanex.Razorbill.Security.Controllers
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.Razorbill.Security.Encryption;
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Resources;
    using Fanex.Razorbill.Security.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SecurityCodeController : BasedController
    {
        private readonly ISecurityCodeService _securityCodeService;

        public SecurityCodeController()
            : this(null)
        {
        }

        public SecurityCodeController(ISecurityCodeService securityCodeService)
        {
            _securityCodeService = securityCodeService ?? new SecurityCodeService();
        }

        private string PH
        {
            get
            {
                return System.Web.HttpContext.Current.Session["ph"] + string.Empty;
            }
        }

        private string ClientIP
        {
            get
            {
                string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                    ?? System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                string[] ips = string.IsNullOrEmpty(ip) ? new string[] { string.Empty } : ip.Split(',');

                return ips[0];
            }
        }

        /// <summary>
        /// Forces the change security code.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult ForceChangeSecurityCode()
        {
            string securityCodeToken = _securityCodeService.GenerateSecurityToken();
            this.SecurityCodeSession.SecurityCodeToken = securityCodeToken;
            ViewBag.SecurityCodeToken = securityCodeToken;
            ViewBag.SecuritySalt = this.SecurityCodeSession.SubAccId > 0
                ? _securityCodeService.MD5Salt(this.SecurityCodeSession.SubAccId)
                : _securityCodeService.MD5Salt(this.SecurityCodeSession.CustId);

            return View("~/Areas/Security/Views/SecurityCode/ForceChangeSecurityCode.cshtml");
        }

        /// <summary>
        /// Forces the change security code without validation.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult ForceChangeSecurityCodeWithoutValidation()
        {
            return View("~/Areas/Security/Views/SecurityCode/ForceChangeSecurityCodeWithoutValidation.cshtml");
        }

        /// <summary>
        /// Verifies the security code.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult VerifySecurityCode()
        {
            string securityCodeToken = _securityCodeService.GenerateSecurityToken();
            this.SecurityCodeSession.SecurityCodeToken = securityCodeToken;
            ViewBag.SecurityCodeToken = securityCodeToken;
            ViewBag.SecuritySalt = this.SecurityCodeSession.SubAccId > 0
                ? _securityCodeService.MD5Salt(this.SecurityCodeSession.SubAccId)
                : _securityCodeService.MD5Salt(this.SecurityCodeSession.CustId);

            return View("~/Areas/Security/Views/SecurityCode/VerifySecurityCode.cshtml");
        }

        /// <summary>
        /// Changes the security code.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult ChangeSecurityCode()
        {
            string redirectURL = SecurityCodeChecker.GetRedirectUrl();
            if (redirectURL != null)
            {
                return Redirect(redirectURL);
            }

            string securityCodeToken = _securityCodeService.GenerateSecurityToken();
            this.SecurityCodeSession.SecurityCodeToken = securityCodeToken;
            ViewBag.SecurityCodeToken = securityCodeToken;
            ViewBag.SecuritySalt = this.SecurityCodeSession.SubAccId > 0
                ? _securityCodeService.MD5Salt(this.SecurityCodeSession.SubAccId)
                : _securityCodeService.MD5Salt(this.SecurityCodeSession.CustId);
            ViewBag.Language = this.SecurityCodeSession.Language;
            ViewBag.VirtualDirectory = ConfigurationManager.AppSettings["VirtualDirectory"];

            return View("~/Areas/Security/Views/SecurityCode/ChangeSecurityCode.cshtml");
        }

        /// <summary>
        /// Resets the security code.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult ResetSecurityCode()
        {
            ViewBag.Language = this.SecurityCodeSession.Language;
            ViewBag.VirtualDirectory = ConfigurationManager.AppSettings["VirtualDirectory"];
            return View("~/Areas/Security/Views/SecurityCode/ResetSecurityCode.cshtml");
        }

        [HttpGet]
        public ActionResult HowToUse()
        {
            string language = this.SecurityCodeSession.Language;
            return View("~/Areas/Security/Views/SecurityCode/HowToUse/SecurityCode_" + language + ".cshtml");
        }

        /// <summary>
        /// Forces the change security code.
        /// </summary>
        /// <param name="oldSecurityCode">The old security code.</param>
        /// <param name="securityCode">The security code.</param>
        /// <param name="confirmSecurityCode">The confirm security code.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult ForceChangeSecurityCode(string oldSecurityCode, string securityCode, string confirmSecurityCode)
        {
            JsonRequestBehavior jsonBehavior = JsonRequestBehavior.DenyGet;
            if (this.SecurityCodeSession.IsInternal)
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = Lang.NotAuthorized }, jsonBehavior);
            }

            securityCode = GetRC4Decryption(securityCode);
            confirmSecurityCode = GetRC4Decryption(confirmSecurityCode);

            //// validate request
            string message = string.Empty;
            if (string.IsNullOrEmpty(oldSecurityCode))
            {
                message = Lang.alertOldSecCodeEmpty;
            }
            else if (string.IsNullOrEmpty(securityCode))
            {
                message = Lang.alertSecCodeEmpty;
            }
            else if (!_securityCodeService.ValidateSecurityCode(securityCode))
            {
                message = Lang.lblSecCodeWrongRule;
            }
            else if (securityCode != confirmSecurityCode)
            {
                message = Lang.alertErrorSecCodeConfirm;
            }

            if (message.Length != 0)
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = message }, jsonBehavior);
            }

            Result result = _securityCodeService.ChangeSecurityCode(
                this.SecurityCodeSession.CustId,
                this.SecurityCodeSession.UserId,
                oldSecurityCode,
                securityCode,
                this.SecurityCodeSession.SecurityCodeToken,
                this.SecurityCodeSession.Language,
                ClientIP,
                this.SecurityCodeSession.SubAccId);
            if (result.ErrorCode == 0)
            {
                // reset random key
                this.SecurityCodeSession.SecurityCodeToken = string.Empty;
                this.SecurityCodeSession.SecurityCodeResult = (int)SecurityCodeStatus.PROVIDED_SC_SUCCESS;
            }

            return Json(result, jsonBehavior);
        }

        /// <summary>
        /// Forces the change security code without validation.
        /// </summary>
        /// <param name="securityCode">The security code.</param>
        /// <param name="confirmSecurityCode">The confirm security code.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        public ActionResult ForceChangeSecurityCodeWithoutValidation(string securityCode, string confirmSecurityCode)
        {
            JsonRequestBehavior jsonBehavior = JsonRequestBehavior.DenyGet;
            if (this.SecurityCodeSession.IsInternal)
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = Lang.NotAuthorized }, jsonBehavior);
            }

            securityCode = GetRC4Decryption(securityCode);
            confirmSecurityCode = GetRC4Decryption(confirmSecurityCode);

            //// validate request
            string message = string.Empty;
            if (string.IsNullOrEmpty(securityCode))
            {
                message = Lang.alertSecCodeEmpty;
            }
            else if (!_securityCodeService.ValidateSecurityCode(securityCode))
            {
                message = Lang.lblSecCodeWrongRule;
            }
            else if (securityCode != confirmSecurityCode)
            {
                message = Lang.alertErrorSecCodeConfirm;
            }

            if (message.Length != 0)
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = message }, jsonBehavior);
            }

            Result result = _securityCodeService.ChangeSecurityCodeWithoutValidation(
                this.SecurityCodeSession.CustId,
                this.SecurityCodeSession.UserId,
                securityCode,
                this.SecurityCodeSession.Language,
                ClientIP,
                this.SecurityCodeSession.SubAccId);
            if (result.ErrorCode == 0)
            {
                this.SecurityCodeSession.SecurityCodeResult = (int)SecurityCodeStatus.PROVIDED_SC_SUCCESS;
            }

            return Json(result, jsonBehavior);
        }

        /// <summary>
        /// Verifies the security code.
        /// </summary>
        /// <param name="securityCode">The security code.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        public ActionResult VerifySecurityCode(string securityCode)
        {
            JsonRequestBehavior jsonBehavior = JsonRequestBehavior.DenyGet;
            if (this.SecurityCodeSession.IsInternal)
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = Lang.NotAuthorized }, jsonBehavior);
            }

            if (string.IsNullOrEmpty(securityCode))
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = Lang.informSecCode }, jsonBehavior);
            }

            Result result = _securityCodeService.CheckSecurityCode(
                this.SecurityCodeSession.CustId,
                securityCode,
                this.SecurityCodeSession.SecurityCodeToken,
                this.SecurityCodeSession.Language,
                ClientIP,
                this.SecurityCodeSession.SubAccId);
            if (result.ErrorCode == 0)
            {
                // reset random key
                this.SecurityCodeSession.SecurityCodeToken = string.Empty;
                this.SecurityCodeSession.SecurityCodeResult = (int)SecurityCodeStatus.PROVIDED_SC_SUCCESS;
            }

            return Json(result, jsonBehavior);
        }

        /// <summary>
        /// Changes the security code.
        /// </summary>
        /// <param name="oldSecurityCode">The old security code.</param>
        /// <param name="securityCode">The security code.</param>
        /// <param name="confirmSecurityCode">The confirm security code.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        public ActionResult ChangeSecurityCode(string oldSecurityCode, string securityCode, string confirmSecurityCode)
        {
            JsonRequestBehavior jsonBehavior = JsonRequestBehavior.DenyGet;
            if (this.SecurityCodeSession.IsInternal)
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = Lang.NotAuthorized }, jsonBehavior); 
            }

            securityCode = GetRC4Decryption(securityCode);
            confirmSecurityCode = GetRC4Decryption(confirmSecurityCode);

            //// validate request
            string message = string.Empty;
            if (string.IsNullOrEmpty(oldSecurityCode))
            {
                message = Lang.alertOldSecCodeEmpty;
            }
            else if (string.IsNullOrEmpty(securityCode))
            {
                message = Lang.alertSecCodeEmpty;
            }
            else if (!_securityCodeService.ValidateSecurityCode(securityCode))
            {
                message = Lang.lblSecCodeWrongRule;
            }
            else if (securityCode != confirmSecurityCode)
            {
                message = Lang.alertErrorSecCodeConfirm;
            }

            if (message.Length != 0)
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = message }, jsonBehavior);
            }

            Result result = _securityCodeService.ChangeSecurityCode(
                this.SecurityCodeSession.CustId,
                this.SecurityCodeSession.UserId,
                oldSecurityCode,
                securityCode,
                this.SecurityCodeSession.SecurityCodeToken,
                this.SecurityCodeSession.Language,
                ClientIP,
                this.SecurityCodeSession.SubAccId);
            if (result.ErrorCode == 0)
            {
                this.SecurityCodeSession.SecurityCodeToken = string.Empty;
            }

            return Json(result, jsonBehavior);
        }

        /// <summary>
        /// Resets the security code.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="subAccId">The sub account id.</param>
        /// <param name="securityCode">The security code.</param>
        /// <param name="confirmSecurityCode">The confirm security code.</param>
        /// <returns>The JSON result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        public ActionResult ResetSecurityCode(int? custId, int? subAccId, string securityCode, string confirmSecurityCode)
        {
            JsonRequestBehavior jsonBehavior = JsonRequestBehavior.DenyGet;
            bool isResetSubAccount = subAccId.HasValue && subAccId.Value > 0;
            if (this.SecurityCodeSession.IsInternal && !isResetSubAccount)
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = Lang.NotAuthorized }, jsonBehavior);
            }

            securityCode = GetRC4Decryption(securityCode);
            confirmSecurityCode = GetRC4Decryption(confirmSecurityCode);
            
            //// validate request
            string message = string.Empty;
            if (string.IsNullOrEmpty(securityCode))
            {
                message = Lang.alertSecCodeEmpty;
            }
            else if (!_securityCodeService.ValidateSecurityCode(securityCode))
            {
                message = Lang.lblSecCodeWrongRule;
            }
            else if (securityCode != confirmSecurityCode)
            {
                message = Lang.alertErrorSecCodeConfirm;
            }

            if (message.Length != 0)
            {
                return Json(new Result { ErrorCode = -1, ErrorMessage = message }, jsonBehavior);
            }

            Result result = null;
            if (custId.HasValue && custId.Value > 0)
            {
                result = _securityCodeService.ResetSecurityCode(
                    custId.Value,
                    this.SecurityCodeSession.UserId,
                    this.SecurityCodeSession.SubAccId,
                    securityCode,
                    this.SecurityCodeSession.Language,
                    ClientIP,
                    this.SecurityCodeSession.RoleId == (byte)UserRole.Agent);
            }
            else if (isResetSubAccount)
            {
                result = _securityCodeService.ResetSecurityCodeOfSubAccount(
                    this.SecurityCodeSession.CustId,
                    this.SecurityCodeSession.UserId,
                    subAccId.Value,
                    securityCode,
                    this.SecurityCodeSession.Language,
                    ClientIP);
            }

            return Json(result, jsonBehavior);
        }

        #region helper methods

        /// <summary>
        /// Gets the R c4 decryption.
        /// </summary>
        /// <param name="encryption">The encryption.</param>
        /// <returns>The encryption string.</returns>
        private string GetRC4Decryption(string encryption)
        {
            if (string.IsNullOrEmpty(encryption))
            {
                return string.Empty;
            }

            return Encryptor.RC4(this.PH, Encryptor.HexDecode(encryption));
        }

        #endregion
    }
}