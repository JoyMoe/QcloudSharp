using System;
using System.Reflection;
// ReSharper disable InconsistentNaming

namespace QcloudSharp
{
    public class ContentText : Attribute
    {
        public string Text { get; set; }
        public ContentText(string text)
        {
            Text = text;
        }
    }

    public class Enum
    {
        public enum Endpoint
        {
            [ContentText("trade.api.qcloud.com")]
            Trade,
            [ContentText("cvm.api.qcloud.com")]
            Cvm
        };
        public enum Region // Defined as IATA code
        {
            [ContentText("bj")]
            BJS,
            [ContentText("gz")]
            CAN,
            [ContentText("sh")]
            SHA,
            [ContentText("hk")]
            HKG,
            [ContentText("ca")]
            YTO
        };

        public static string ToEndpoint(Endpoint en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(ContentText), false);

                if (attrs.Length > 0)
                    return ((ContentText)attrs[0]).Text;
            }

            return en.ToString();

        }

        public static string ToRegion(Region en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo.Length > 0)
            {

                object[] attrs = memInfo[0].GetCustomAttributes(typeof(ContentText), false);

                if (attrs.Length > 0)
                    return ((ContentText)attrs[0]).Text;
            }

            return en.ToString();

        }
    }
}
