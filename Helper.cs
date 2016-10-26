using System.Net.Http;
using Newtonsoft.Json;

namespace QcloudSharp
{
    public class Helper
    {
        public static string GetHttpResult(HttpResponseMessage message)
        {
            return message.Content.ReadAsStringAsync().Result;
        }

        public static ApiResult ParseResult(string jsonText)
        {
            return JsonConvert.DeserializeObject<ApiResult>(jsonText);
        }
    }
}
