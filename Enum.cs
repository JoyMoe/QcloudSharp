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
            [ContentText("account.api.qcloud.com")]
            Account,
            [ContentText("bill.api.qcloud.com")]
            Bill,
            [ContentText("cbs.api.qcloud.com")]
            Cbs,
            [ContentText("cdb.api.qcloud.com")]
            Cdb,
            [ContentText("cdn.api.qcloud.com")]
            Cdn,
            [ContentText("cmem.api.qcloud.com")]
            Cmem,
            [ContentText("csec.api.qcloud.com")]
            Csec,
            [ContentText("cvm.api.qcloud.com")]
            Cvm,
            [ContentText("dayu.api.qcloud.com")]
            Dayu,
            [ContentText("eip.api.qcloud.com")]
            Eip,
            [ContentText("image.api.qcloud.com")]
            Image,
            [ContentText("lb.api.qcloud.com")]
            Lb,
            [ContentText("live.api.qcloud.com")]
            Live,
            [ContentText("market.api.qcloud.com")]
            Market,
            [ContentText("monitor.api.qcloud.com")]
            Monitor,
            [ContentText("redis.api.qcloud.com")]
            Redis,
            [ContentText("scaling.api.qcloud.com")]
            Scaling,
            [ContentText("snapshot.api.qcloud.com")]
            Snapshot,
            [ContentText("sqlserver.api.qcloud.com")]
            Sqlserver,
            [ContentText("tdsql.api.qcloud.com")]
            Tdsql,
            [ContentText("trade.api.qcloud.com")]
            Trade,
            [ContentText("vod.api.qcloud.com")]
            Vod,
            [ContentText("vpc.api.qcloud.com")]
            Vpc,
            [ContentText("wenzhi.api.qcloud.com")]
            Wenzhi,
            [ContentText("yunsou.api.qcloud.com")]
            Yunsou
        }
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
        }

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
