using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace QcloudSharp
{
    public class QcloudClient : DynamicObject
    {
        private string _uri = "/v2/index.php";
        private List<KeyValuePair<string, string>> _patameters;

        public string SecretId { get; set; }
        public string SecretKey { get; set; }
        public Enum.Region Region { get; set; }
        public Enum.Endpoint Endpoint { get; set; }

        public QcloudClient()
        {
        }
        public QcloudClient(string secretId, string secretKey)
        {
            SecretId = secretId;
            SecretKey = secretKey;
        }

        private static int CompareKeyValuePair(KeyValuePair<string, string> a, KeyValuePair<string, string> b)
        {
            return string.Compare(a.Key, b.Key, StringComparison.Ordinal);
        }
        private string GetSignature(string endpoint, List<KeyValuePair<string, string>> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            data.Sort(CompareKeyValuePair);
            using (var content = new FormUrlEncodedContent(data))
            {
                var queryString = WebUtility.UrlDecode(content.ReadAsStringAsync().Result);
                var plainString = $"GET{endpoint}{_uri}?{queryString}";

                using (HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(SecretKey)))
                {
                    return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(plainString)));
                }
            }
        }
        private string Send(string endpoint, List<KeyValuePair<string, string>> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            data.Add(new KeyValuePair<string, string>("Signature", GetSignature(endpoint, data)));
            
            using (var content = new FormUrlEncodedContent(data))
            {
                using (var client = new HttpClient())
                {
                    var message = client.GetAsync($"https://{endpoint}{_uri}?{content.ReadAsStringAsync().Result}").Result;
                    return message.Content.ReadAsStringAsync().Result;
                }
            }
        }

        public void AddParameter(KeyValuePair<string, string> parameter)
        {
            if (_patameters == null) _patameters = new List<KeyValuePair<string, string>>();
            _patameters.Add(parameter);
        }
        public void AddParameter(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (_patameters == null) _patameters = new List<KeyValuePair<string, string>>();
            _patameters.AddRange(parameters.ToList());
        }
        public void ClearParameter()
        {
            _patameters = new List<KeyValuePair<string, string>>();
        }

        public string Submit(Enum.Endpoint endpoint, Enum.Region region, string action)
        {
            Endpoint = endpoint;
            Region = region;
            return Submit(action);
        }
        public string Submit(Enum.Endpoint endpoint, string action)
        {
            Endpoint = endpoint;
            return Submit(action);
        }
        public string Submit(string action)
        {
            var patameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Action", action),
                new KeyValuePair<string, string>("Region", Enum.ToRegion(Region)),
                new KeyValuePair<string, string>("Timestamp", DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
                new KeyValuePair<string, string>("Nonce", new Random().Next().ToString()),
                new KeyValuePair<string, string>("SecretId", SecretId)
            };

            if (_patameters != null) patameters.AddRange(_patameters);

            return Send(Enum.ToEndpoint(Endpoint), patameters);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (args.Length < 2)
                throw new ArgumentException("Endpoint and Region must be specified.");

            if (!(args[0] is Enum.Endpoint))
                // ReSharper disable once NotResolvedInText
                throw new ArgumentNullException("Endpoint");

            Endpoint = (Enum.Endpoint)args[0];

            if (!(args[1] is Enum.Region))
                // ReSharper disable once NotResolvedInText
                throw new ArgumentNullException("Region");
            
            Region = (Enum.Region)args[0];

            if (args.Length >= 3)
            {
                ClearParameter();

                if (args[2] is IEnumerable<KeyValuePair<string, string>>)
                {
                    AddParameter((IEnumerable<KeyValuePair<string, string>>)args[2]);
                }
                else
                {
                    args = args.Skip(2).ToArray();
                    foreach (var arg in args)
                    {
                        if (!(arg is KeyValuePair<string, string>))
                            throw new ArgumentException("Parameters required to be KeyValuePair<string, string>");

                        AddParameter((KeyValuePair<string, string>)arg);
                    }
                }
                
            }

            result = Submit(binder.Name);

            return true;
        }
    }
}
