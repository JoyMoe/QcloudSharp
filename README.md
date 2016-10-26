QcloudSharp: Unoffical Qcloud.com API wrapper for .Net
===
[![Build status](https://ci.appveyor.com/api/projects/status/b96llok223xfhmx5?svg=true)](https://ci.appveyor.com/project/7IN0SAN9/qcloudsharp)
[![NuGet version](https://badge.fury.io/nu/QcloudSharp.svg)](https://www.nuget.org/packages/QcloudSharp)

### Installation
To install QcloudSharp, run the following command in the Package Manager Console
```
PM> Install-Package QcloudSharp
```

### Example
```csharp
using QcloudSharp;
using Enum = QcloudSharp.Enum;

dynamic client = new QcloudClient
{
    SecretId = "Your_Secret_Id",
    SecretKey = "Your_Secret_Key"
};

var resultString = client.DescribeUserInfo(Enum.Endpoint.Trade, Enum.Endpoint.Region.CAN);
dynamic result = JsonConvert.DeserializeObject<ApiResult>(resultString);
```

### Enums

All Enums are provided in class `QcloudSharp.Enum`.

```
public enum Endpoint // Abbreviation for endpoint domain
```

Members
* Trade
* Cvm

```
public enum Region // IATA code for Region city
```

Members
* BJS
* CAN
* SHA
* HKG
* YTO

### Classes

#### QcloudClient

```
public class QcloudClient : DynamicObject
```

Constructors
* `QcloudClient()`
* `QcloudClient(SecretId, SecretKey)`

Properties
* string `SecretId`
* string `SecretKey`
* Enum.Region `Region`
* Enum.Endpoint `Endpoint`

Methods
* `void AddParameter(KeyValuePair<string, string>)`
* `void AddParameter(IEnumerable<KeyValuePair<string, string>>)`
* `void ClearParameter()`
* `Submit(Enum.Endpoint, Enum.Region, string)`
* `Submit(Enum.Endpoint, string)`
* `Submit(string)`

Dynamic Methods
* `{Action}(Enum.Endpoint, Enum.Region)`
* `{Action}(Enum.Endpoint, Enum.Region, IEnumerable<KeyValuePair<string, string>>)`
* `{Action}(Enum.Endpoint endpoint, Enum.Region region,KeyValuePair<string, string>, ...)`

#### ApiResult

```
public class ApiResult : DynamicObject
```

Constructors
* `ApiResult()`

Properties
* int `Code`
* string `Message`

Dynamic Properties
* object Any { Get; Set; }