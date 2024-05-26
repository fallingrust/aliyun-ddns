

# AliyunDNS

<!-- PROJECT SHIELDS -->

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]

### 配置
``` sh
 docker pull registry.cn-hangzhou.aliyuncs.com/xuejianchen/aliyun-ddns:lastest
 docker run -d --name aliyun-ddns 
```
根目录下生成config.json文件，或直接运行一次：
```json
{
  "V4Url": "https://ipv4.ip.mir6.com",
  "V6Url": "https://ipv6.ip.mir6.com",
  "EndPoint": "https://alidns.cn-hangzhou.aliyuncs.com",
  "Interval": 600,
  "KeyId": "",
  "KeySecret": "",
  "Domains": [
    {
      "DomainName": "",
      "V4Enable": true,
      "V6Enable": true
    }
  ],
  "SubDomains": [
    {
      "DomainName": "",
      "V4Enable": true,
      "V6Enable": true
    }
  ]
}
```

<!-- links -->

[your-project-path]:fallingrust/aliyun-ddns

[contributors-shield]: https://img.shields.io/github/contributors/fallingrust/aliyun-ddns.svg?style=flat-square

[contributors-url]: https://github.com/fallingrust/aliyun-ddns/graphs/contributors

[forks-shield]: https://img.shields.io/github/forks/fallingrust/aliyun-ddns.svg?style=flat-square

[forks-url]: https://github.com/fallingrust/aliyun-ddns/network/members

[stars-shield]: https://img.shields.io/github/stars/fallingrust/aliyun-ddns.svg?style=flat-square

[stars-url]: https://github.com/fallingrust/aliyun-ddns/stargazers

[issues-shield]: https://img.shields.io/github/issues/fallingrust/aliyun-ddns.svg?style=flat-square

[issues-url]: https://img.shields.io/github/issues/fallingrust/aliyun-ddns.svg

[license-shield]: https://img.shields.io/github/license/fallingrust/aliyun-ddns.svg?style=flat-square

[license-url]: https://github.com/fallingrust/aliyun-ddns/blob/master/LICENSE.txt