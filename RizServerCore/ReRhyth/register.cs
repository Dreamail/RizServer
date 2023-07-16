using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RizServerCoreSharp.ReRhyth
{
    public static class Register
    {
        public static Classes.RhythAccountResponseWithToken Reg(string requestbody)
        {
            Classes.RhythAccountRegisterRequest req = JsonConvert.DeserializeObject<Classes.RhythAccountRegisterRequest>(requestbody);
            //TODO: Do code verification after completing the SMTP function of send_email (optional in the configuration file)

            Classes.RhythAccount NewAccount = new Classes.RhythAccount
            {
                email = req.email,
                password = req.password,
                avatar = new Classes.RhythAccountAvatar()
            };
            Classes.DBMain.AddContent(GlobalConfig.DBConfig.JsonName, "RizServerCoreSharp_ReRhythUserAccountObject", new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(),NewAccount);
            return new Classes.RhythAccountResponseWithToken { ret = ("{\"code\":0,\"msg\":\"{\\\"email\\\":\\\"" + req.email + "\\\",\\\"avatar:\\\":{\\\"name\\\":\\\"testAvatar\\\",\\\"portrait\\\":\\\"\\\"}\"}"),header_set_token = Classes.TokenGenerator.GenerateToken(req.email) };
        }
    }
}
