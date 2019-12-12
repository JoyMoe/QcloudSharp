using System;
using QcloudSharp;
using Newtonsoft.Json;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            dynamic client = new QcloudClient
            {
                SecretId = "Your_Secret_Id",
                SecretKey = "Your_Secret_Key",
                Endpoint = Constants.Endpoint.Trade,
            };

            var resultString = client.DescribeUserInfo(Constants.Region.CAN);
            // e.g. {"code":0,"message": "","userInfo":{"name":"compName","isOwner":1,"mailStatus":1,"mail":"112233@qq.com","phone":"13811112222"}}

            var result = JsonConvert.DeserializeObject<ApiResult>(resultString);

            try
            {
                Console.WriteLine(result.Code);
                Console.WriteLine(result.Message);
                Console.WriteLine(result.userInfo.name);
                Console.WriteLine(result.notExist);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
