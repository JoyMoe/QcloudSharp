using System.Collections.Generic;
using System.Dynamic;

namespace QcloudSharp
{
    public class ApiResult : DynamicObject
    {
        private Dictionary<string, object> _attr = new Dictionary<string, object>();
        public int Code { get; set; }
        public string Message { get; set; }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _attr.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _attr[binder.Name] = value;
            return true;
        }
    }
}
