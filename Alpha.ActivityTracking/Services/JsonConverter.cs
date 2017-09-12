using Fanex.ActivityTracking.Services;

namespace Alpha.ActivityTracking.Services
{
    public class JsonConverter : IJsonConverter
    {
        public string SerializeObject(object value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }
    }
}
