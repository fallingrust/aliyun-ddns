using System.Text.Json.Serialization;

namespace aliyun_ddns.Beans
{
    public class Config
    {
        /// <summary>
        /// 获取IPV4地址
        /// </summary>
        public string V4Url { get; set; } = "https://ipv4.ip.mir6.com";
        /// <summary>
        /// 获取IPV6地址
        /// </summary>
        public string V6Url { get; set; } = "https://ipv6.ip.mir6.com";
        /// <summary>
        /// 服务地址，阿里云:https://api.aliyun.com/product/Alidns
        /// </summary>
        public string EndPoint { get; set; } = "https://alidns.cn-hangzhou.aliyuncs.com";
        /// <summary>
        /// 更新间隔，单位：秒
        /// </summary>
        public long Interval { get; set; } = 600;
        /// <summary>
        /// 阿里云RAM Access Key Id
        /// </summary>
        public string KeyId { get; set; } = string.Empty;
        /// <summary>
        /// 阿里云RAM Access Key Secret
        /// </summary>
        public string KeySecret { get; set; } = string.Empty;
        /// <summary>
        /// 域名
        /// </summary>
        public List<Domain> Domains { get; set; } = [];
        /// <summary>
        /// 子域名
        /// </summary>
        public List<Domain> SubDomains { get; set; } = [];
    }

    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(Config))]
    [JsonSerializable(typeof(IEnumerable<Config>))]
    public partial class ConfigSerializerContext : JsonSerializerContext
    {
    }


    public class Domain
    {
        /// <summary>
        /// 域名
        /// </summary>
        public string DomainName { get; set; } = string.Empty;

        /// <summary>
        /// IPV4 Enable
        /// </summary>
        public bool V4Enable { get; set; } = true;

        /// <summary>
        ///  IPV6 Enable
        /// </summary>
        public bool V6Enable { get; set; } = true;

        public Domain()
        {

        }
        public Domain(string domainName, bool v4Enable, bool v6Enable)
        {
            DomainName = domainName;
            V4Enable = v4Enable;
            V6Enable = v6Enable;
        }
    }


    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(Domain))]
    [JsonSerializable(typeof(IEnumerable<Domain>))]
    public partial class DomainSerializerContext : JsonSerializerContext
    {
    }
}
