using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Reflection.Metadata;
using System.Buffers.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.OpenSsl;

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

        public static class Security
        {
            public static class AES
            {
                // 定义AESEncrypt函数，用于用AES加密文本并输出为base64格式
                public static string AESEncrypt(string plainText)
                {
                    // 创建AES对象
                    using (Aes aes = Aes.Create())
                    {
                        // 设置密钥长度为256位
                        aes.KeySize = 256;
                        // 设置加密模式为CBC
                        aes.Mode = CipherMode.CBC;
                        // 设置填充模式为PKCS7
                        aes.Padding = PaddingMode.PKCS7;
                        // 创建加密器
                        ICryptoTransform encryptor = aes.CreateEncryptor(Encoding.UTF8.GetBytes(Classes.LoadedConfig.aes_key), Encoding.UTF8.GetBytes(Classes.LoadedConfig.aes_iv));
                        // 创建内存流
                        using (MemoryStream ms = new MemoryStream())
                        {
                            // 创建加密流
                            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                            {
                                // 将文本转换为字节数组
                                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                                // 将字节数组写入加密流
                                cs.Write(plainBytes, 0, plainBytes.Length);
                                // 刷新加密流
                                cs.FlushFinalBlock();
                                // 获取加密后的字节数组
                                byte[] cipherBytes = ms.ToArray();
                                // 将字节数组转换为base64字符串
                                string cipherText = Convert.ToBase64String(cipherBytes);
                                // 返回加密后的字符串
                                return cipherText;
                            }
                        }
                    }
                }

                // 定义AESDecrypt函数，用于用AES解密base64格式的字符串并输出为文本
                public static string AESDecrypt(string cipherText)
                {
                    // 创建AES对象
                    using (Aes aes = Aes.Create())
                    {
                        // 设置密钥长度为256位
                        aes.KeySize = 256;
                        // 设置加密模式为CBC
                        aes.Mode = CipherMode.CBC;
                        // 设置填充模式为PKCS7
                        aes.Padding = PaddingMode.PKCS7;
                        // 创建解密器
                        ICryptoTransform decryptor = aes.CreateDecryptor(Encoding.UTF8.GetBytes(Classes.LoadedConfig.aes_key), Encoding.UTF8.GetBytes(Classes.LoadedConfig.aes_iv));
                        // 创建内存流
                        using (MemoryStream ms = new MemoryStream())
                        {
                            // 创建解密流
                            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                            {
                                // 将base64字符串转换为字节数组
                                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                                // 将字节数组写入解密流
                                cs.Write(cipherBytes, 0, cipherBytes.Length);
                                // 刷新解密流
                                cs.FlushFinalBlock();
                                // 获取解密后的字节数组
                                byte[] plainBytes = ms.ToArray();
                                // 将字节数组转换为文本
                                string plainText = Encoding.UTF8.GetString(plainBytes);
                                // 返回解密后的文本
                                return plainText;
                            }
                        }
                    }
                }
            }

            public static class RSA
            {
                // 使用PKCS8格式的RSA私钥加密字符串
                public static string EncryptStringWithPrivateKey(string plainText, string privateKey)
                {
                    // 将私钥转换为AsymmetricKeyParameter对象
                    AsymmetricKeyParameter key = GetPrivateKeyFromString(privateKey);

                    // 创建RSA引擎并初始化
                    var rsaEngine = new RsaEngine();
                    rsaEngine.Init(true, key);

                    // 创建PKCS1编码器并初始化
                    var pkcs1Encoding = new Pkcs1Encoding(rsaEngine);
                    pkcs1Encoding.Init(true, key);

                    // 将明文转换为字节数组
                    byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                    // 使用PKCS1编码器加密字节数组
                    byte[] encryptedBytes = pkcs1Encoding.ProcessBlock(plainTextBytes, 0, plainTextBytes.Length);

                    // 将加密后的字节数组转换为Base64编码的字符串
                    string encryptedString = Convert.ToBase64String(encryptedBytes);

                    return encryptedString;
                }

                // 从字符串中获取AsymmetricKeyParameter对象
                private static AsymmetricKeyParameter GetPrivateKeyFromString(string privateKey)
                {
                    using (var txtreader = GetStreamReader(privateKey))
                    {
                        var keyPair = (AsymmetricKeyParameter)new PemReader(txtreader).ReadObject();
                        return keyPair;
                    }
                }

                private static StreamReader GetStreamReader(string content)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(content);
                    var memory = new MemoryStream(bytes);
                    var reader = new StreamReader(memory);

                    return reader;
                }

                public static string GenerateSignature(string md5)
                {
                    var private_key = File.ReadAllText(Classes.LoadedConfig.resources_path + "/RSAPrivateKey.pem");
                    var encrypted = EncryptStringWithPrivateKey(md5, private_key);
                    return encrypted;
                }
            }

            public static class MD5
            {
                public static string GetMD5Hash(string input)
                {
                    // 创建MD5实例
                    using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                    {
                        // 将输入字符串转换为字节数组
                        byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                        // 计算输入字节数组的MD5哈希值
                        byte[] hashBytes = md5.ComputeHash(inputBytes);

                        // 将字节数组转换为十六进制字符串
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < hashBytes.Length; i++)
                        {
                            sb.Append(hashBytes[i].ToString("x2"));
                        }
                        return sb.ToString();
                    }
                }

                public static byte[] GetByteHashFromString(string str)
                {
                    System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
                    // 将字符串转换成字节数组
                    byte[] byteOld = Encoding.UTF8.GetBytes(str);
                    // 调用加密方法
                    byte[] byteNew = md5.ComputeHash(byteOld);

                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in byteNew)
                    {
                        // 将字节转换成16进制表示的字符串，
                        sb.Append(b.ToString("x2"));
                    }
                    Console.WriteLine("Debug>MD5 Hash=" + sb);

                    return byteNew;
                }
            }
        }
        public static class ReRizTools
        {
            public static Classes.ReRizReturnEncryptResponseWithSign BuildEncryptMessage(string responsebody)
            {
                string aes_encrypted = Tools.Security.AES.AESEncrypt(responsebody);
                string header_sign = Tools.Security.RSA.GenerateSignature(Security.MD5.GetMD5Hash(responsebody));
                return new Classes.ReRizReturnEncryptResponseWithSign
                {
                    ResponseBody = aes_encrypted,
                    ResponseHeaderSign = header_sign
                };
            }
        }
    }
}
