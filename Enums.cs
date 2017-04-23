using System;
using System.ComponentModel;
using System.Reflection;

namespace QcloudSharp
{
    public static class Enums
    {
        public enum Endpoint
        {
            [Description("account.api.qcloud.com")]
            Account,
            [Description("bill.api.qcloud.com")]
            Bill,
            [Description("bm.api.qcloud.com")]
            Bm,
            [Description("cbs.api.qcloud.com")]
            Cbs,
            [Description("cdb.api.qcloud.com")]
            Cdb,
            [Description("cdn.api.qcloud.com")]
            Cdn,
            [Description("cmem.api.qcloud.com")]
            Cmem,
            [Description("cmq-queue-{0}.api.qcloud.com")]
            CmqQueue,
            [Description("cmq-topic-{0}.api.qcloud.com")]
            CmqTopic,
            [Description("cns.api.qcloud.com")]
            Cns,
            [Description("csec.api.qcloud.com")]
            Csec,
            [Description("cvm.api.qcloud.com")]
            Cvm,
            [Description("dayu.api.qcloud.com")]
            Dayu,
            [Description("dfw.api.qcloud.com")]
            Dfw,
            [Description("eip.api.qcloud.com")]
            Eip,
            [Description("image.api.qcloud.com")]
            Image,
            [Description("iot.api.qcloud.com")]
            Iot,
            [Description("kms-{0}.api.qcloud.com")]
            Kms,
            [Description("lb.api.qcloud.com")]
            Lb,
            [Description("live.api.qcloud.com")]
            Live,
            [Description("market.api.qcloud.com")]
            Market,
            [Description("mongodb.api.qcloud.com")]
            Mongodb,
            [Description("monitor.api.qcloud.com")]
            Monitor,
            [Description("partners.api.qcloud.com")]
            Partners,
            [Description("redis.api.qcloud.com")]
            Redis,
            [Description("scaling.api.qcloud.com")]
            Scaling,
            [Description("snapshot.api.qcloud.com")]
            Snapshot,
            [Description("sqlserver.api.qcloud.com")]
            Sqlserver,
            [Description("tdsql.api.qcloud.com")]
            Tdsql,
            [Description("tmt.api.qcloud.com")]
            Tmt,
            [Description("trade.api.qcloud.com")]
            Trade,
            [Description("vod.api.qcloud.com")]
            Vod,
            [Description("vod2.api.qcloud.com")]
            Vod2,
            [Description("vpc.api.qcloud.com")]
            Vpc,
            [Description("wenzhi.api.qcloud.com")]
            Wenzhi,
            [Description("wss.api.qcloud.com")]
            Wss,
            [Description("yunsou.api.qcloud.com")]
            Yunsou
        }
        public enum Region // Defined as IATA code
        {
            [Description("bj")]
            BJS,
            [Description("gz")]
            CAN,
            [Description("hk")]
            HKG,
            [Description("sh")]
            SHA,
            [Description("sg")]
            SIN,
            [Description("usw")]
            SJC,
            [Description("ca")]
            YTO,
            [Description("gzopen")]
            CAN1,
            [Description("shjr")]
            SHA2,
            [Description("szjr")]
            SZX2,
        }

        public static string ToDescription<T>(this T enumerationValue)
            where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }
    }
}
