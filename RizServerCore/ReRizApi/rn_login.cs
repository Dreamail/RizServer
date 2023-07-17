using Newtonsoft.Json;
using System.Security.Cryptography;

namespace RizServerCoreSharp.ReRizApi
{
    public static class RizLogin
    {
        public static (string,string) Login(string header_token)
        {
            string token_email = Classes.TokenGenerator.CheckToken(header_token);
            if (token_email == null) {
                return (" ","token_error");
            }
            else
            {
                Osp_DB.SearchFilter SearchFilter = new Osp_DB.SearchFilter("RizServerCoreSharp_RizUserAccountObject", null, null);
                var SearchResult = Classes.DBMain.SearchWithFilter(GlobalConfig.DBConfig.JsonName, SearchFilter);
                foreach (var item in SearchResult)
                {
                    var itemobj = JsonConvert.DeserializeObject<Classes.RizAccount>(item.obj.ToString());
                    if (itemobj.username == token_email)
                    {
                        string ret_str = JsonConvert.SerializeObject(itemobj);
                        string aes_encrypted = Tools.Security.AES.AESEncrypt(ret_str);
                        string header_sign = Tools.Security.RSA.GenerateSignature(Tools.Security.MD5.GetHashFromString(ret_str));
                        return (header_sign,header_token);
                    }
                }
            }
        }
    }
}