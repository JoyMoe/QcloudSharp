using Microsoft.AspNetCore.WebUtilities;
using Moq;
using Moq.Protected;
using System;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace QcloudSharp.Tests
{
    public class QcloudClientTests
    {
        private const string SecretId = "Your_Secret_Id";
        private const string SecretKey = "Your_Secret_Key";

        [Fact]
        public void ShouldCreateNewDynamicObject()
        {
            dynamic client = new QcloudClient();
            Assert.IsType<QcloudClient>(client);
            Assert.IsAssignableFrom<DynamicObject>(client);
        }

        [Fact]
        public void ShouldSendCorrectlyRequest()
        {
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns((HttpRequestMessage req, CancellationToken ct) => GetMockResponse(req, ct))
                .Callback<HttpRequestMessage, CancellationToken>((req, ct) =>
                {
                    var query = QueryHelpers.ParseQuery(req.RequestUri.Query);
                    Assert.Equal("DescribeUserInfo", query["Action"]);
                    Assert.Equal(Constants.Region.CAN, query["Region"]);
                    Assert.Equal(SecretId, query["SecretId"]);

                    var plaintext = $"GET{Constants.Endpoint.Trade}/v2/index.php?Action={query["Action"]}&Nonce={query["Nonce"]}&Region={query["Region"]}&SecretId={query["SecretId"]}&Timestamp={query["Timestamp"]}";

                    using var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(SecretKey));

                    Assert.Equal(Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(plaintext))), query["Signature"]);
                })
                .Verifiable();

            var disposable = mock.As<IDisposable>();
            disposable.Setup(d => d.Dispose());

            dynamic client = new QcloudClient(SecretId, SecretKey, mock.Object)
            {
                Endpoint = Constants.Endpoint.Trade,
                Region = Constants.Region.CAN,
            };

            var resultString = client.DescribeUserInfo();

            mock.Protected().Verify(
                "SendAsync",
                Times.Once(), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get &&
                  req.RequestUri.Host == Constants.Endpoint.Trade &&
                  req.RequestUri.Query.Contains("Action") &&
                  req.RequestUri.Query.Contains("Region") &&
                  req.RequestUri.Query.Contains("Timestamp") &&
                  req.RequestUri.Query.Contains("Nonce") &&
                  req.RequestUri.Query.Contains("SecretId") &&
                  req.RequestUri.Query.Contains("Signature")
               ),
               ItExpr.IsAny<CancellationToken>()
            );
        }

        private Task<HttpResponseMessage> GetMockResponse(HttpRequestMessage request, CancellationToken ct)
        {
            return Task.FromResult(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{""code"":200,""message"": ""OK"",""userInfo"":{""name"":""compName"",""isOwner"":1,""mailStatus"":1,""mail"":""112233@qq.com"",""phone"":""13811112222""}}"),
            });
        }
    }
}
