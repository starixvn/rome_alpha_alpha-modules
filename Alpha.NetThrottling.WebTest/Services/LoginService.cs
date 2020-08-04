namespace Alpha.NetThrottling.WebTest.Services
{
    using Alpha.NetThrottling.WebTest.Models;

    public class LoginService
    {
        public User Login(LoginModel model)
        {
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