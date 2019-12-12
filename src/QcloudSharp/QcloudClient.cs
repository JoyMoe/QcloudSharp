using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QcloudSharp
{
    /// <summary>
    ///
    /// </summary>
    public class QcloudClient : DynamicObject
    {
        private const string Uri = "/v2/index.php";
        private readonly HttpMessageHandler _handler;

        /// <summary>
        /// Gets or sets the SecretId.
        /// </summary>
        public string SecretId { get; set; }

        /// <summary>
        /// Gets or sets the SecretKey.
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Gets or sets the Region.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the Endpoint.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QcloudClient"/> class.
        /// </summary>
        /// <param name="secretId">The SecretId.</param>
        /// <param name="secretKey">The SecretKey.</param>
        public QcloudClient(string secretId = null, string secretKey = null, HttpMessageHandler handler = null)
        {
            _handler = handler;

            SecretId = secretId;
            SecretKey = secretKey;
        }

        private static int CompareKeyValuePair(KeyValuePair<string, string> a, KeyValuePair<string, string> b)
        {
            return string.Compare(a.Key, b.Key, StringComparison.Ordinal);
        }

        private async Task<string> GetSignatureAsync(string endpoint, List<KeyValuePair<string, string>> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            data.Sort(CompareKeyValuePair);

            using var content = new FormUrlEncodedContent(data);

            var queryString = WebUtility.UrlDecode(await content.ReadAsStringAsync());
            var plainString = $"GET{endpoint}{Uri}?{queryString}";

            using var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(SecretKey));

            return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(plainString)));
        }

        private async Task<string> SendAsync(string endpoint, List<KeyValuePair<string, string>> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            data.Add(new KeyValuePair<string, string>("Signature", await GetSignatureAsync(endpoint, data)));

            using var content = new FormUrlEncodedContent(data);

            var client = new HttpClient(_handler);

            var message = await client.GetAsync($"https://{endpoint}{Uri}?{await content.ReadAsStringAsync()}");

            return await message.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Submit the request.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <return>The result.</return>
        public string Submit(string action, IEnumerable<KeyValuePair<string, string>> parameters = null)
        {
            return SubmitAsync(action, parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Submit the request.
        /// </summary>
        /// <param name="endpoint">The Endpoint, <see cref="Constants.Endpoint" />.</param>
        /// <param name="action">The action.</param>
        /// <return>The result.</return>
        public string Submit(string endpoint, string action, IEnumerable<KeyValuePair<string, string>> parameters = null)
        {
            return SubmitAsync(endpoint, action, parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Submit the request.
        /// </summary>
        /// <param name="endpoint">The Endpoint, <see cref="Constants.Endpoint" />.</param>
        /// <param name="region">The Region, <see cref="Constants.Region" />.</param>
        /// <param name="action">The action.</param>
        /// <param name="parameters">The parameters.</param>
        /// <return>The result.</return>
        public string Submit(string endpoint, string region, string action, IEnumerable<KeyValuePair<string, string>> parameters = null)
        {
            return SubmitAsync(endpoint, region, action, parameters).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Submit the request.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="parameters">The parameters.</param>
        /// <return>The result.</return>
        public async Task<string> SubmitAsync(string action, IEnumerable<KeyValuePair<string, string>> parameters = null)
        {
            return await SubmitAsync(Endpoint, Region, action, parameters);
        }

        /// <summary>
        /// Submit the request.
        /// </summary>
        /// <param name="endpoint">The Endpoint, <see cref="Constants.Endpoint" />.</param>
        /// <param name="action">The action.</param>
        /// <param name="parameters">The parameters.</param>
        /// <return>The result.</return>
        public async Task<string> SubmitAsync(string endpoint, string action, IEnumerable<KeyValuePair<string, string>> parameters = null)
        {
            return await SubmitAsync(endpoint, Region, action, parameters);
        }

        /// <summary>
        /// Submit the request.
        /// </summary>
        /// <param name="endpoint">The Endpoint, <see cref="Constants.Endpoint" />.</param>
        /// <param name="region">The Region, <see cref="Constants.Region" />.</param>
        /// <param name="action">The action.</param>
        /// <param name="parameters">The parameters.</param>
        /// <return>The result.</return>
        public async Task<string> SubmitAsync(string endpoint, string region, string action, IEnumerable<KeyValuePair<string, string>> parameters = null)
        {
            var data = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Action", action),
                new KeyValuePair<string, string>("Region", region),
                new KeyValuePair<string, string>("Timestamp", DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
                new KeyValuePair<string, string>("Nonce", new Random().Next().ToString()),
                new KeyValuePair<string, string>("SecretId", SecretId)
            };

            if (parameters != null) data.AddRange(parameters);

            var endpointUrl = string.Format(endpoint, region);

            return await SendAsync(endpointUrl, data);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var arguments = args.ToList();

            if (arguments.Count >= 1)
            {
                var endpoints = typeof(Constants.Endpoint).GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Select(x => x.GetRawConstantValue().ToString());

                if (arguments.First() is string && endpoints.Contains(arguments.First()))
                {
                    Endpoint = (string) arguments.First();

                    arguments.RemoveAt(0);
                }
            }

            if (arguments.Count >= 1)
            {
                var regions = typeof(Constants.Region).GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Select(x => x.GetRawConstantValue().ToString());

                if (arguments.First() is string && regions.Contains(arguments.First()))
                {
                    Region = (string) arguments.First();

                    arguments.RemoveAt(0);
                }
            }

            if (string.IsNullOrEmpty(Endpoint))
                throw new ArgumentNullException(nameof(Endpoint));

            if (string.IsNullOrEmpty(Region))
                throw new ArgumentNullException(nameof(Region));

            if (!arguments.Any())
                result = Submit(binder.Name);
            else if (arguments.First() is IEnumerable<KeyValuePair<string, string>> parameters)
                result = Submit(binder.Name, parameters);
            else
                result = Submit(binder.Name, arguments.OfType<KeyValuePair<string, string>>());

            return true;
        }
    }
}
