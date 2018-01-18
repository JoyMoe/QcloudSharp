# QcloudSharp: Unoffical Qcloud.com API wrapper for .Net

[![AppVeyor](https://img.shields.io/appveyor/ci/JoyMoe/qcloudsharp.svg)](https://ci.appveyor.com/project/JoyMoe/qcloudsharp)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/JoyMoe/QcloudSmsSharp/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/QcloudSharp.svg)](https://www.nuget.org/packages/QcloudSharp)
[![NuGet](https://img.shields.io/nuget/vpre/QcloudSharp.svg)](https://www.nuget.org/packages/QcloudSharp/absoluteLatest)
![net451](https://img.shields.io/badge/.Net-4.5.1-brightgreen.svg)
![net462](https://img.shields.io/badge/.Net-4.6.2-brightgreen.svg)
![netstandard1.5](https://img.shields.io/badge/.Net-netstandard1.5-brightgreen.svg)
![netstandard2.0](https://img.shields.io/badge/.Net-netstandard2.0-brightgreen.svg)

[中文说明](README.zh.md)

## Installation

To install QcloudSharp, run the following command in the Package Manager Console

```powershell
PM> Install-Package QcloudSharp
```

## Example

```csharp
using QcloudSharp;
using Newtonsoft.Json;

dynamic client = new QcloudClient
{
    SecretId = "Your_Secret_Id",
    SecretKey = "Your_Secret_Key",
    Endpoint = Constants.Endpoint.Trade, // Endpoint can be used as the first argument of Dynamic Methods
    Region = Constants.Region.CAN, // Region can be used as the first or second argument of Dynamic Methods
};

var resultString = client.DescribeUserInfo();
// e.g. {"code":0,"message": "","userInfo":{"name":"compName","isOwner":1,"mailStatus":1,"mail":"112233@qq.com","phone":"13811112222"}}

dynamic result = JsonConvert.DeserializeObject<ApiResult>(resultString);

try
{
    Console.WriteLine(result.Code);
    Console.WriteLine(result.userInfo.name);
    Console.WriteLine(result.notExist);
}
catch(Exception ex)
{
    Console.WriteLine(ex.message);
}
```

Or you can have a look at [sample](sample).

## Constants

All Constants are provided by class `QcloudSharp.Constants`.

```csharp
public static class Endpoint // Abbreviation for endpoint domain
```

Members

* Account
* Batch
* Bgpip
* Bill
* Bm
* Bmeip
* Bmlb
* Bmvpc
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
public static class Region // IATA code for Region city
```

Members

* BJS `bj`
* CAN `gz`
* CAN1 `gzopen`
* CKG `cq`
* CTU `cd`
* FRA `de`
* HKG `hk`
* SEL `kr`
* SHA `sh`
* SHA2 `shjr`
* SIN `sg`
* SJC `usw`
* SZX2 `szjr`
* YTO `ca`

## Classes

### QcloudClient

```csharp
public class QcloudClient : DynamicObject
```

Constructors

* `QcloudClient()`
* `QcloudClient(SecretId, SecretKey)`

Properties

* string `SecretId`
* string `SecretKey`
* string `Region`
* string `Endpoint`

Methods

* `void AddParameter(KeyValuePair<string, string>)`
* `void AddParameter(IEnumerable<KeyValuePair<string, string>>)`
* `void ClearParameter()`
* `Submit(string, string, string)`
* `Submit(string, string)`
* `Submit(string)`

Dynamic Methods

* `{Action}([string][, string][, IEnumerable<KeyValuePair<string, string>>])`
* `{Action}([string][, string][, KeyValuePair<string, string> ...])`

### ApiResult

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

## License

The MIT License

More info see [LICENSE](LICENSE)
