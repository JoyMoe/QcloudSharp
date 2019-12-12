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
        private List<KeyValuePair<string, string>> _patameters;
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
        /// Add a parameter to the request.
        /// </summary>
        /// <param name="parameter">The <see cref="KeyValuePair{TKey, TValue}" />.</param>
        public void AddParameter(KeyValuePair<string, string> parameter)
        {
            if (_patameters == null) _patameters = new List<KeyValuePair<string, string>>();
            _patameters.Add(parameter);
        }

        /// <summary>
        /// Add parameters to the request.
        /// </summary>
        /// <param name="parameters">The <see cref="IEnumerable{T}" /> of <see cref="KeyValuePair{TKey, TValue}" />.</param>
        public void AddParameter(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (_patameters == null) _patameters = new List<KeyValuePair<string, string>>();
            _patameters.AddRange(parameters.ToList());
        }

        /// <summary>
        /// Remove all parameters from the request.
        /// </summary>
        public void ClearParameter()
        {
            _patameters = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Submit the request.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <return>The result.</return>
        public string Submit(string action)
        {
            return SubmitAsync(action).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Submit the request.
        /// </summary>
        /// <param name="endpoint">The Endpoint, <see cref="Constants.Endpoint" />.</param>
        /// <param name="action">The action.</param>
        /// <return>The result.</return>
        public string Submit(string endpoint, string action)
        {
            return SubmitAsync(endpoint, action).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Submit the request.
        /// </summary>
        /// <param name="endpoint">The Endpoint, <see cref="Constants.Endpoint" />.</param>
        /// <param name="region">The Region, <see cref="Constants.Region" />.</param>
        /// <param name="action">The action.</param>
        /// <return>The result.</return>
        public string Submit(string endpoint, string region, string action)
        {
            return SubmitAsync(endpoint, region, action).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Submit the request.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <return>The result.</return>
        public async Task<string> SubmitAsync(string action)
        {
            return await SubmitAsync(Endpoint, Region, action);
        }

        /// <summary>
        /// Submit the request.
        /// </summary>
        /// <param name="endpoint">The Endpoint, <see cref="Constants.Endpoint" />.</param>
        /// <param name="action">The action.</param>
        /// <return>The result.</return>
        public async Task<string> SubmitAsync(string endpoint, string action)
        {
            return await SubmitAsync(endpoint, Region, action);
        }

        /// <summary>
        /// Submit the request.
        /// </summary>
        /// <param name="endpoint">The Endpoint, <see cref="Constants.Endpoint" />.</param>
        /// <param name="region">The Region, <see cref="Constants.Region" />.</param>
        /// <param name="action">The action.</param>
        /// <return>The result.</return>
        public async Task<string> SubmitAsync(string endpoint, string region, string action)
        {
            var patameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Action", action),
                new KeyValuePair<string, string>("Region", region),
                new KeyValuePair<string, string>("Timestamp", DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
                new KeyValuePair<string, string>("Nonce", new Random().Next().ToString()),
                new KeyValuePair<string, string>("SecretId", SecretId)
            };

            if (_patameters != null) patameters.AddRange(_patameters);

            string endpointUrl = String.Format(endpoint, region);

            return await SendAsync(endpointUrl, patameters);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            ClearParameter();

            var argsStack = new Stack<object>(args.Reverse());

            if (argsStack.Count >= 1)
            {
                var endpoints = typeof(Constants.Endpoint).GetFields(BindingFlags.Public | BindingFlags.Static).
                                                Select(x => x.GetRawConstantValue().ToString());

                if (argsStack.First() is string && endpoints.Contains(argsStack.First()))
                {
                    Endpoint = (string)argsStack.Pop();
                }
            }

            if (argsStack.Count >= 1)
            {
                var regions = typeof(Constants.Region).GetFields(BindingFlags.Public | BindingFlags.Static).
                                                          Select(x => x.GetRawConstantValue().ToString());

                if (argsStack.First() is string && regions.Contains(argsStack.First()))
                {
                    Region = (string)argsStack.Pop();
                }
            }

            if (argsStack.Count >= 1)
            {
                if (argsStack.First() is IEnumerable<KeyValuePair<string, string>>)
                {
                    AddParameter((IEnumerable<KeyValuePair<string, string>>)argsStack.Pop());
                }
                else
                {
                    while (argsStack.Count > 0)
                    {
                        var arg = argsStack.Pop();

                        if (!(arg is KeyValuePair<string, string>)) continue;

                        AddParameter((KeyValuePair<string, string>)arg);
                    }
                }
            }

            if (string.IsNullOrEmpty(Endpoint))
                throw new ArgumentNullException(nameof(Endpoint));

            if (string.IsNullOrEmpty(Region))
                throw new ArgumentNullException(nameof(Region));

            result = Submit(binder.Name);

            return true;
        }
    }
}
