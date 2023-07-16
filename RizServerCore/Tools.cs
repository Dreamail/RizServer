using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace RizServerCoreSharp
{
    public static class Tools
    {
        public class TokenGenerator
        {
            // 一个列表，用来存储信任的token
            private List<string> trustedTokens;

            // 一个构造函数，用来初始化列表
            public TokenGenerator()
            {
                trustedTokens = new List<string>();
            }

            // 一个方法，用来根据邮箱生成一个token，并把它加到列表里
            public string GenerateToken(string email)
            {
                // 创建一个随机数生成器
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    // 创建一个字节数组，用来存储token
                    byte[] tokenBytes = new byte[16];
                    // 用随机字节填充数组
                    rng.GetBytes(tokenBytes);
                    // 把数组转换成base64字符串
                    string token = Convert.ToBase64String(tokenBytes);
                    // 把邮箱和token加到列表里，用":"分隔
                    trustedTokens.Add(email + ":" + token);
                    // 返回token
                    return token;
                }
            }

            // 一个方法，用来检查一个token是否信任，并返回对应的邮箱
            public string CheckToken(string token)
            {
                // 遍历信任的token列表
                foreach (string item in trustedTokens)
                {
                    // 用":"分割item，得到邮箱和token
                    string[] parts = item.Split(":");
                    string email = parts[0];
                    string trustedToken = parts[1];
                    // 如果token和信任的token相同，返回邮箱
                    if (token == trustedToken)
                    {
                        return email;
                    }
                }
                // 如果没有找到匹配的，返回null
                return null;
            }
        }
    }
}
