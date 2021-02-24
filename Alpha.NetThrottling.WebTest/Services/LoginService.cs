namespace Alpha.NetThrottling.WebTest.Services
{
    using Alpha.NetThrottling.WebTest.Helpers;
    using Alpha.NetThrottling.WebTest.Models;
    using System.Diagnostics;

    public class LoginService
    {
        public User Login(LoginModel model)
        {
            var clientIp = IPHelper.ClientIP();
            Debug.WriteLine($"Login IP: {clientIp}");
            if (model.Password == "1234aaAA")
            {
                var userId = model.Username == "leo?" ? 2 : 1;
                var user = new User
                {
                    UserId = userId,
                    Username = model.Username
                };
                return user;
            }

            return null;
        }
    }
}