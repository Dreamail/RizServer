using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Utilities.Encoders;

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
                Random random = new Random();
                // 创建一个字节数组，用来存储token
                byte[] tokenBytes = new byte[16];
                // 用随机字节填充数组
                random.NextBytes(tokenBytes);
                // 把数组转换成base64字符串
                string token = Convert.ToBase64String(tokenBytes);
                // 把邮箱和token加到列表里，用":"分隔
                trustedTokens.Add(email + ":" + token);
                // 返回token
                return token;
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
                public static string AESEncrypt(string input)
                {
                    byte[] key = Encoding.UTF8.GetBytes(Classes.LoadedConfig.aes_key);
                    byte[] iv = Encoding.UTF8.GetBytes(Classes.LoadedConfig.aes_iv);

                    // 将输入字符串转换为字节数组
                    byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                    // 创建AesEngine对象
                    AesEngine engine = new AesEngine();

                    // 创建CbcBlockCipher对象（CBC模式）
                    CbcBlockCipher blockCipher = new CbcBlockCipher(engine);

                    // 创建PaddedBufferedBlockCipher对象（PKCS7填充）
                    PaddedBufferedBlockCipher cipher = new PaddedBufferedBlockCipher(blockCipher, new Pkcs7Padding());

                    // 创建KeyParameter对象（密钥）
                    KeyParameter keyParam = new KeyParameter(key);

                    // 创建ParametersWithIV对象（初始向量）
                    ParametersWithIV keyParamWithIv = new ParametersWithIV(keyParam, iv);

                    // 初始化cipher对象（true表示加密）
                    cipher.Init(true, keyParamWithIv);

                    // 创建输出字节数组
                    byte[] outputBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];

                    // 执行加密操作
                    int length = cipher.ProcessBytes(inputBytes, outputBytes, 0);
                    cipher.DoFinal(outputBytes, length);

                    // 将输出字节数组转换为Base64字符串
                    return Convert.ToBase64String(outputBytes);
                }

                // 解密方法
                public static string AESDecrypt(string input)
                {
                    byte[] key = Encoding.UTF8.GetBytes(Classes.LoadedConfig.aes_key);
                    byte[] iv = Encoding.UTF8.GetBytes(Classes.LoadedConfig.aes_iv);

                    // 将输入字符串转换为字节数组
                    byte[] inputBytes = Convert.FromBase64String(input);

                    // 创建AesEngine对象
                    AesEngine engine = new AesEngine();

                    // 创建CbcBlockCipher对象（CBC模式）
                    CbcBlockCipher blockCipher = new CbcBlockCipher(engine);

                    // 创建PaddedBufferedBlockCipher对象（PKCS7填充）
                    PaddedBufferedBlockCipher cipher = new PaddedBufferedBlockCipher(blockCipher, new Pkcs7Padding());

                    // 创建KeyParameter对象（密钥）
                    KeyParameter keyParam = new KeyParameter(key);

                    // 创建ParametersWithIV对象（初始向量）
                    ParametersWithIV keyParamWithIv = new ParametersWithIV(keyParam, iv);

                    // 初始化cipher对象（false表示解密）
                    cipher.Init(false, keyParamWithIv);

                    // 创建输出字节数组
                    byte[] outputBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];

                    // 执行解密操作
                    int length = cipher.ProcessBytes(inputBytes, outputBytes, 0);
                    cipher.DoFinal(outputBytes, length);

                    // 将输出字节数组转换为UTF8字符串
                    return Encoding.UTF8.GetString(outputBytes);
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
                // MD5哈希函数
                public static string GetMD5Hash(string input)
                {
                    // 创建MD5Digest对象
                    MD5Digest digest = new MD5Digest();

                    // 将输入字符串转换为字节数组
                    byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                    // 更新digest对象
                    digest.BlockUpdate(inputBytes, 0, inputBytes.Length);

                    // 创建输出字节数组
                    byte[] outputBytes = new byte[digest.GetDigestSize()];

                    // 完成digest操作
                    digest.DoFinal(outputBytes, 0);

                    // 使用Hex对象将输出字节数组转换为十六进制字符串
                    return Hex.ToHexString(outputBytes);
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
