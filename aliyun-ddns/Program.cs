using Serilog;
using aliyun_ddns.Beans;
using System.Text.Json;
using AliyunSDK.DNS;

namespace aliyun_ddns
{
    internal class Program
    {
        private static readonly string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "config.json");
        private static Config? _config;
        static async Task Main(string[] args)
        {
            var logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
                .WriteTo.File(Path.Combine(logDir, "log.txt"),
                              rollingInterval: RollingInterval.Day,
                              rollOnFileSizeLimit: true)
                .CreateLogger();
            Log.Information($"aliyun dns start with args({string.Join(" ", args)})");
           
            if (!File.Exists(configPath))
            {
                Log.Information($"{configPath} not exists!");
                using var sw = new StreamWriter(configPath);
                _config = new Config();
                _config.Domains.Add(new Domain());
                _config.SubDomains.Add(new Domain());
                var configJson = JsonSerializer.Serialize(_config, ConfigSerializerContext.Default.Config);
                sw.WriteLine(configJson);
            }
            else
            {
                var content =File.ReadAllText(configPath);
                _config = JsonSerializer.Deserialize(content, ConfigSerializerContext.Default.Config);
                Log.Information($"{content}");
            }
            if (_config == null)
            {
                Log.Error($"config is null~");
                return;
            }
            if (string.IsNullOrWhiteSpace(_config.KeyId))
            {
                Log.Error($"KeyId is null~");
                return;
            }
            if (string.IsNullOrWhiteSpace(_config.KeySecret))
            {
                Log.Error($"KeySecret is null~");
                return;
            }
            AliyunDNS.Configure(new AliyunDns.Core.Option()
            {
                EndPoint = _config.EndPoint,
                KeyId = _config.KeyId,
                KeySecret = _config.KeySecret,
            });

            while (true)
            {
                var ipv4 = (await AddressUtil.GetIPv4AddressAsync(_config.V4Url))?.ToString();
                Log.Information($"query public ipv4:{ipv4}");
                var ipv6 = (await AddressUtil.GetIPv6AddressAsync(_config.V6Url))?.ToString();
                Log.Information($"query public ipv6:{ipv6}");

                try
                {
                    foreach (var domain in _config.Domains)
                    {
                        await UpdateDomain(domain, ipv4, ipv6);
                    }

                    foreach (var domain in _config.SubDomains)
                    {
                        await UpdateSubDomain(domain, ipv4, ipv6);
                    }
                }
                catch(Exception e)
                {
                    Log.Error(e, "UpdateDomain Exception");
                }
               
                await Task.Delay((int)(_config.Interval * 1000));
            }
        }

        private static async Task UpdateDomain(Domain domain, string? ipv4, string? ipv6)
        {
            if (_config == null) return;
            if (string.IsNullOrWhiteSpace(domain.DomainName)) return;
           
            var response = await AliyunDNS.DescribeDomainRecordsAsync(domain.DomainName);
            if (response != null && response.TotalCount > 0 && response.DomainRecords != null && response.DomainRecords.Record != null)
            {

                foreach (var record in response.DomainRecords.Record)
                {
                    if (string.IsNullOrWhiteSpace(record.RecordId) || string.IsNullOrWhiteSpace(record.Type) || string.IsNullOrWhiteSpace(record.RR))
                    {
                        Log.Information($"{record.DomainName} params is empty~");
                        continue;
                    }
                    if (record.Locked)
                    {
                        Log.Information($"{record.DomainName} {record.RR} is locked~");
                        continue;
                    }
                    if (record.Status?.ToLower() != "enable")
                    {
                        Log.Information($"{record.DomainName} {record.RR} is not enable~");
                        continue;
                    }

                    if (record.Type == "A" && domain.V4Enable)
                    {
                        if (!string.IsNullOrWhiteSpace(ipv4) && record.Value != ipv4)
                        {
                            Log.Information($"update {record.DomainName} {record.RR} {record.Value}->{ipv4}");

                         await AliyunDNS.UpdateDomainRecordAsync(record, ipv4);
                            
                        }
                        else
                        {
                            Log.Information($" {record.DomainName} {record.RR}  not changed");
                        }
                    }
                    else if (record.Type == "AAAA" && domain.V6Enable)
                    {
                        if (!string.IsNullOrWhiteSpace(ipv6) && record.Value != ipv6)
                        {
                            Log.Information($"update {record.DomainName} {record.RR} {record.Value}->{ipv6}");
                            await AliyunDNS.UpdateDomainRecordAsync(record, ipv6);
                        }
                        else
                        {
                            Log.Information($" {record.DomainName} {record.RR}  not changed");
                        }
                    }
                }
            }
            else
            {
                Log.Information($"query records with null or empty data");
            }
            return;
        }
        private static async Task UpdateSubDomain(Domain domain, string? ipv4, string? ipv6)
        {
            if (_config == null) return;
            if (string.IsNullOrWhiteSpace(domain.DomainName)) return;
            var response = await AliyunDNS.DescribeSubDomainRecordsAsync(domain.DomainName);
            if (response != null && response.TotalCount > 0 && response.DomainRecords != null && response.DomainRecords.Record != null)
            {
                foreach (var record in response.DomainRecords.Record)
                {
                    if (string.IsNullOrWhiteSpace(record.RecordId) || string.IsNullOrWhiteSpace(record.Type) || string.IsNullOrWhiteSpace(record.RR))
                    {
                        Log.Information($"{record.DomainName} params is empty~");
                        continue;
                    }
                    if (record.Locked)
                    {
                        Log.Information($"{record.DomainName} {record.RR} is locked~");
                        continue;
                    }
                    if (record.Status?.ToLower() != "enable")
                    {
                        Log.Information($"{record.DomainName} {record.RR} is not enable~");
                        continue;
                    }

                    if (record.Type == "A" && domain.V4Enable)
                    {
                        if (!string.IsNullOrWhiteSpace(ipv4) && record.Value != ipv4)
                        {
                            Log.Information($"update {record.DomainName} {record.RR} {record.Value}->{ipv4}");
                            var updateResponse = await AliyunDNS.UpdateDomainRecordAsync(record, ipv4);
                            Log.Information($"updateResponse {updateResponse.RecordId}");
                        }
                        else
                        {
                            Log.Information($" {record.DomainName} {record.RR}  not changed");
                        }
                    }
                    else if (record.Type == "AAAA" && domain.V6Enable)
                    {
                        if (!string.IsNullOrWhiteSpace(ipv6) && record.Value != ipv6)
                        {
                            Log.Information($"update {record.DomainName} {record.RR} {record.Value}->{ipv6}");
                            var updateResponse = await AliyunDNS.UpdateDomainRecordAsync(record, ipv6);
                            Log.Information($"updateResponse {updateResponse.RecordId}");
                        }
                        else
                        {
                            Log.Information($" {record.DomainName} {record.RR}  not changed");
                        }
                    }
                }
            }
            else
            {
                Log.Information($"query {domain.DomainName} records with null or empty data");
            }
            return;
        }
    }
}
