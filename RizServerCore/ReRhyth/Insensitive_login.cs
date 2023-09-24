using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;

namespace RizServerCoreSharp.ReRhyth
{
    public static class Insensitive_login
    {
        public static string InsensitiveLogin(string requestbody)
        {
            Classes.RhythAccountInsensitiveLoginRequest req = JsonConvert.DeserializeObject<Classes.RhythAccountInsensitiveLoginRequest>(requestbody);

            if(Classes.TokenGenerator.CheckToken(req.token) != null)
            {
                return "{\"code\":0,\"msg\":\"{\\\"email\\\":\\\"" + req.email + "\\\",\\\"avatar\\\":{\\\"name\\\":\\\"testAvatar\\\",\\\"portrait\\\":\\\"\\\"}}\"}";
            }

            return "token invalid";
        }
    }
}
