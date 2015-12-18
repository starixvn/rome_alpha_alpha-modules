namespace Fanex.Age.UI
{
    using System.Text.RegularExpressions;

    public static class Validation
    {
        private const int SecurityCodeLength = 6;
        private const string SecurityCodeRulePattern = @"^([0-9])(?!\1+$)[0-9]+$";

        private static readonly Regex PasswordRule1 = new Regex(@"^(?=.*\d).{8,100}$");
        private static readonly Regex PasswordRule2 = new Regex(@"^(?=.*[a-z]).{8,100}$");
        private static readonly Regex PasswordRule3 = new Regex(@"^(?=.*[A-Z]).{8,100}$");
        private static readonly Regex PasswordRule4 = new Regex(@"^(?=.*[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]).{8,100}$");
        private static readonly Regex MemberPasswordRule = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,10}$");
        private static readonly Regex NoWhiteSpace = new Regex(@"^\S*$");

        /// <summary>
        /// Password must be at least 8 characters without white space and contain at least 3 of the following:
        ///  • Uppercase letter [A-Z]  
        ///  • Lowercase leader [a-z]  
        ///  • Numeric [0-9]  
        ///  • Special character (!,@,#,etc..)
        /// For example: 59D7!4$h
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool CheckValidPassword(string password)
        {
            var r1 = PasswordRule1.Match(password).Success ? 1 : 0;
            var r2 = PasswordRule2.Match(password).Success ? 1 : 0;
            var r3 = PasswordRule3.Match(password).Success ? 1 : 0;
            var r4 = PasswordRule4.Match(password).Success ? 1 : 0;

            return (r1 + r2 + r3 + r4 >= 3) && NoWhiteSpace.Match(password).Success;
        }

        public static bool CheckValidMemberPassword(string password)
        {
            return MemberPasswordRule.Match(password).Success && NoWhiteSpace.Match(password).Success;
        }

        /// <summary>
        /// Check is valid language string.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static bool CheckValidLanguage(string culture)
        {
            return Sunplus.Agent.Model.Common.Language.IsValidLanguage(culture);
        }

        public static bool CheckValidSecurityCode(string securityCode)
        {
            int secCodeLength = securityCode.Length;
            if (secCodeLength != SecurityCodeLength)
            { // check length
                return false;
            }

            Regex regex = new Regex(SecurityCodeRulePattern);
            bool result = regex.IsMatch(securityCode); // check digits and all of them are not the same
            if (result)
            { // check 'Forward Run' and 'Backward Run'
                bool isForwardRun = true, isBackwardRun = true;
                int backward;
                var forward = backward = int.Parse(securityCode[0].ToString());
                for (int i = 1; i < secCodeLength; i++)
                {
                    int current = int.Parse(securityCode[i].ToString());
                    if (++forward != current)
                    {
                        isForwardRun = false;
                    }

                    if (--backward != current)
                    {
                        isBackwardRun = false;
                    }
                }

                result = !(isForwardRun || isBackwardRun);
            }

            return result;
        }
    }
}
