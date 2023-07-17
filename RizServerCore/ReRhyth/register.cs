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
        public static List<String> NewPlayerUnlockedLevels = new List<String>() {
            "track.PastelLines.RekuMochizuki.0",
            "track.Gleam.Uske.0",
            "track.PowerAttack.EBIMAYO.0"
        };

        public static Classes.RhythAccountResponseWithToken Reg(string requestbody)
        {
            string reg_timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString() + ">" + Classes.RandomObject.Next(1, 114514).ToString();
            Classes.RizAccount NewPlayerTemplate = JsonConvert.DeserializeObject<Classes.RizAccount>(File.ReadAllText(Classes.LoadedConfig.resources_path + "/RizAccountBase.json"));

            Classes.RhythAccountRegisterRequest req = JsonConvert.DeserializeObject<Classes.RhythAccountRegisterRequest>(requestbody);
            //TODO: Do code verification after completing the SMTP function of send_email (optional in the configuration file)

            Classes.RhythAccount NewAccount = new Classes.RhythAccount
            {
                email = req.email,
                password = req.password,
                avatar = new Classes.RhythAccountAvatar()
            };
            //Create a RhythAccount
            Classes.DBMain.AddContent(GlobalConfig.DBConfig.JsonName, "RizServerCoreSharp_ReRhythUserAccountObject", reg_timestamp, NewAccount);

            //Create a RizAccount
            var RizNewAccount = new Classes.RizAccount
            {
                _id = "RegTimestamp=" + reg_timestamp,
                username = req.email,
                unlockedLevels = NewPlayerUnlockedLevels,
                appearLevels = NewPlayerTemplate.appearLevels,
                getProducts = NewPlayerTemplate.getProducts,
                getItems = NewPlayerTemplate.getItems
            };
            Classes.DBMain.AddContent(GlobalConfig.DBConfig.JsonName, "RizServerCoreSharp_RizUserAccountObject", reg_timestamp, RizNewAccount);

            return new Classes.RhythAccountResponseWithToken { ret = ("{\"code\":0,\"msg\":\"{\\\"email\\\":\\\"" + req.email + "\\\",\\\"avatar\\\":{\\\"name\\\":\\\"testAvatar\\\",\\\"portrait\\\":\\\"\\\"}}\"}"), header_set_token = Classes.TokenGenerator.GenerateToken(req.email) };
        }
    }
}
