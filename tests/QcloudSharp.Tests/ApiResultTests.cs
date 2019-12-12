using Newtonsoft.Json;
using System;
using System.Dynamic;
using Xunit;

namespace QcloudSharp.Tests
{
    public class ApiResultTests
    {
        [Fact]
        public void ShouldCreateNewDynamicObject()
        {
            dynamic result = new ApiResult();
            Assert.IsType<ApiResult>(result);
            Assert.IsAssignableFrom<DynamicObject>(result);
        }

        [Fact]
        public void ShouldSetGetProperties()
        {
            dynamic result = new ApiResult { 
                Code = 200,
                Message = "OK"
            };

            Assert.Equal(200, result.Code);
            Assert.Equal("OK", result.Message);
        }

        [Fact]
        public void ShouldSetGetDynamicMembers()
        {
            dynamic result = new ApiResult();

            result.Foo = "Bar";

            Assert.Equal("Bar", result.Foo);
        }

        [Fact]
        public void ShouldThrowMemberAccessException()
        {
            dynamic result = new ApiResult();

            Assert.Throws<MemberAccessException>(() => result.Foo);
        }

        [Fact]
        public void ShouldUnserializeFromJson()
        {
            var json = @"{""code"":200,""message"": ""OK"",""userInfo"":{""name"":""compName"",""isOwner"":1,""mailStatus"":1,""mail"":""112233@qq.com"",""phone"":""13811112222""}}";

            dynamic result = JsonConvert.DeserializeObject<ApiResult>(json);

            Assert.Equal(200, result.Code);
            Assert.Equal("OK", result.Message);
            Assert.Equal("compName", result.userInfo.name.ToString());
            Assert.Throws<MemberAccessException>(() => result.notExist);
        }
    }
}
