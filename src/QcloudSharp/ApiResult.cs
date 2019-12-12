using System;
using System.Collections.Generic;
using System.Dynamic;

namespace QcloudSharp
{
    public class ApiResult : DynamicObject
    {
        private readonly Dictionary<string, object> _attr = new Dictionary<string, object>();
        public int Code { get; set; }
        public string Message { get; set; }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!_attr.ContainsKey(binder.Name))
            {
                throw new MemberAccessException(binder.Name);
            }

            result = _attr[binder.Name];
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _attr[binder.Name] = value;
            return true;
        }
    }
}
