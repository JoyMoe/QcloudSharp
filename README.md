QcloudSharp: Unoffical Qcloud.com API wrapper for .Net
===
[![Build status](https://ci.appveyor.com/api/projects/status/my3yu20j7635osdj?svg=true)](https://ci.appveyor.com/project/7IN0SAN9/qcloudsharp)
[![NuGet version](https://badge.fury.io/nu/QcloudSharp.svg)](https://www.nuget.org/packages/QcloudSharp)

### Installation
To install QcloudSharp, run the following command in the Package Manager Console
```powershell
PM> Install-Package QcloudSharp
```

### Example
```csharp
using QcloudSharp;
using Newtonsoft.Json;
using QcloudSharp.Enums;

dynamic client = new QcloudClient
{
    SecretId = "Your_Secret_Id",
    SecretKey = "Your_Secret_Key"
};

var resultString = client.DescribeUserInfo(Enums.Endpoint.Trade, Enums.Endpoint.Region.CAN);
// e.g. {"code":0,"message": "","userInfo":{"name":"compName","isOwner":1,"mailStatus":1,"mail":"112233@qq.com","phone":"13811112222"}}

dynamic result = JsonConvert.DeserializeObject<ApiResult>(resultString);

try
{
    Console.WriteLine(result.Code);
    Console.WriteLine(result.userInfo.name);
    Console.WriteLine(result.notExist); // Will throw an ArgumentNullException
}
catch(Exception ex)
{
    Console.WriteLine(ex.message);
}
```

Or you can have a look at [QcloudCvmHelper](https://github.com/kinosang/QcloudCvmHelper)

### Enums

All Enums are provided by class `QcloudSharp.Enums`.

```csharp
public enum Endpoint // Abbreviation for endpoint domain
```

Members
* Account
* Bill
* Bm
* Cbs
* Cdb
* Cdn
* Cmem
* CmqQueue
* CmqTopic
* Cns
* Csec
* Cvm
* Dayu
* Dfw
* Eip
* Image
* Iot
* Kms
* Lb
* Live
* Market
* Mongodb
* Monitor
* Partners
* Redis
* Scaling
* Snapshot
* Sqlserver
* Tdsql
* Tmt
* Trade
* Vod
* Vod2
* Vpc
* Wenzhi
* Wss
* Yunsou

```csharp
public enum Region // IATA code for Region city
```

Members
* BJS `bj`
* CAN `gz`
* HKG `hk`
* SHA `sh`
* SIN `sg`
* SJC `usw`
* YTO `ca`
* CAN1 `gzopen`
* SHA2 `shjr`
* SZX2 `szjr`

### Classes

#### QcloudClient

```csharp
public class QcloudClient : DynamicObject
```

Constructors
* `QcloudClient()`
* `QcloudClient(SecretId, SecretKey)`

Properties
* string `SecretId`
* string `SecretKey`
* Enums.Region `Region`
* Enums.Endpoint `Endpoint`

Methods
* `void AddParameter(KeyValuePair<string, string>)`
* `void AddParameter(IEnumerable<KeyValuePair<string, string>>)`
* `void ClearParameter()`
* `Submit(Enums.Endpoint, EnumsRegion, string)`
* `Submit(Enums.Endpoint, string)`
* `Submit(string)`

Dynamic Methods
* `{Action}(Enums.Endpoint, Enums.Region)`
* `{Action}(Enums.Endpoint, Enums.Region, IEnumerable<KeyValuePair<string, string>>)`
* `{Action}(Enums.Endpoint, Enums.Region, KeyValuePair<string, string>, ...)`

#### ApiResult

```csharp
public class ApiResult : DynamicObject
```

Constructors
* `ApiResult()`

Properties
* int `Code`
* string `Message`

Dynamic Properties
* object Any { Get; Set; }

## Donate us

[Donate us](https://7in0.me/#donate)

## License

The MIT License

More info see [LICENSE](LICENSE)
