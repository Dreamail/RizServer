using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RizServerCoreSharp.ReRhyth
{
    public static class RhythAccountLogin
    {
        public static Classes.RhythAccountResponseWithToken Login(string requestbody)
        {
            Classes.RhythAccountLoginRequest req = JsonConvert.DeserializeObject<Classes.RhythAccountLoginRequest>(requestbody);
            Osp_DB.SearchFilter SearchFilter = new Osp_DB.SearchFilter("RizServerCoreSharp_ReRhythUserAccountObject", null, null);
            var SearchResult = Classes.DBMain.SearchWithFilter(GlobalConfig.DBConfig.JsonName, SearchFilter);
            foreach(var item in SearchResult)
            {
                var itemobj = JsonConvert.DeserializeObject<Classes.RhythAccount>(item.obj.ToString());
                if(itemobj.email == req.email && itemobj.password == req.password)
                {
                    return new Classes.RhythAccountResponseWithToken { ret = ("{\"code\":0,\"msg\":\"{\\\"email\\\":\\\"" + req.email + "\\\",\\\"avatar:\\\":{\\\"name\\\":\\\"testAvatar\\\",\\\"portrait\\\":\\\"\\\"}\"}"), header_set_token = Classes.TokenGenerator.GenerateToken(req.email) };
                }
            }
            return new Classes.RhythAccountResponseWithToken { ret = "{\"code\":2,\"msg\":\"Password Error\"}",header_set_token = "error" };
        }
    }
}
