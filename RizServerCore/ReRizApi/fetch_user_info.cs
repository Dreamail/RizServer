using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RizServerCoreSharp.Classes;

namespace RizServerCoreSharp.ReRizApi
{
    public static class FetchUserInfo
    {
        public static ReRizReturnEncryptResponseWithSign FetchUserInfoMain(string header_token, string requestbody)
        {
            string token_email = Classes.TokenGenerator.CheckToken(header_token);
            if (token_email == null)
            {
                return Tools.ReRizTools.BuildEncryptMessage("token_error");
            }

            Osp_DB.SearchFilter SearchFilter = new Osp_DB.SearchFilter("RizServerCoreSharp_RizUserAccountObject", null, null);
            var SearchResult = Classes.DBMain.SearchWithFilter(GlobalConfig.DBConfig.JsonName, SearchFilter);
            foreach (var item in SearchResult)
            {
                var itemobj = JsonConvert.DeserializeObject<Classes.RizAccount>(item.obj.ToString());
                if (itemobj.username == token_email)
                {
                    return Tools.ReRizTools.BuildEncryptMessage("{\"code\":0,\"data\":{\"username\":\"\",\"dot\":" + itemobj.dot + ",\"coin\":" + itemobj.coin + "}}");
                }
            }
            return Tools.ReRizTools.BuildEncryptMessage("token_error");
        }
    }
}
