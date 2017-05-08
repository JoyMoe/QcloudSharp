QcloudSharp: 腾讯云 API 的非官方 .Net 封装
===
[![AppVeyor](https://img.shields.io/appveyor/ci/7IN0SAN9/qcloudsharp.svg)](https://ci.appveyor.com/project/7IN0SAN9/qcloudsharp)
[![NuGet](https://img.shields.io/nuget/v/QcloudSharp.svg)](https://www.nuget.org/packages/QcloudSharp)

### 安装
在 Package Manager Console 中运行如下指令或使用 Nuget.
```powershell
PM> Install-Package QcloudSharp
```

### 范例
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
// 预期返回值 {"code":0,"message": "","userInfo":{"name":"compName","isOwner":1,"mailStatus":1,"mail":"112233@qq.com","phone":"13811112222"}}

dynamic result = JsonConvert.DeserializeObject<ApiResult>(resultString);

try
{
    Console.WriteLine(result.Code);
    Console.WriteLine(result.userInfo.name);
    Console.WriteLine(result.notExist); // 将抛出 ArgumentNullException 异常
}
catch(Exception ex)
{
    Console.WriteLine(ex.message);
}
```

更多细节可查看 [QcloudCvmHelper](https://github.com/kinosang/QcloudCvmHelper) 项目.

### 枚举

所有枚举由 `QcloudSharp.Enums` 类提供.

```csharp
public enum Endpoint // API 端点
```

成员
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
public enum Region // 区域（采用三位 IATA 城市编码）
```

成员
* BJS `北京`
* CAN `广州`
* HKG `香港`
* SHA `上海`
* SIN `新加坡`
* SJC `美国西部`
* YTO `加南大`
* CAN1 `广州开放`
* SHA2 `上海金融`
* SZX2 `深圳金融`

### 类

#### QcloudClient

```csharp
public class QcloudClient : DynamicObject
```

初始化
* `QcloudClient()`
* `QcloudClient(SecretId, SecretKey)`

属性
* string `SecretId`
* string `SecretKey`
* Enums.Region `Region`
* Enums.Endpoint `Endpoint`

方法
* `void AddParameter(KeyValuePair<string, string>)`
* `void AddParameter(IEnumerable<KeyValuePair<string, string>>)`
* `void ClearParameter()`
* `Submit(Enums.Endpoint, EnumsRegion, string)`
* `Submit(Enums.Endpoint, string)`
* `Submit(string)`

动态方法
* `{Action}(Enums.Endpoint, Enums.Region)`
* `{Action}(Enums.Endpoint, Enums.Region, IEnumerable<KeyValuePair<string, string>>)`
* `{Action}(Enums.Endpoint, Enums.Region, KeyValuePair<string, string>, ...)`

#### ApiResult

```csharp
public class ApiResult : DynamicObject
```

初始化
* `ApiResult()`

属性
* int `Code`
* string `Message`

动态属性
* object Any { Get; Set; }

## 捐赠

[Donate us](https://7in0.me/#donate)

## 授权

The MIT License

详见 [LICENSE](LICENSE).
