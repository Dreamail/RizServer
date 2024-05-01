using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RizServerCoreSharp.ReRizApi
{
    public static class ChangeUserName
    {
        public static Classes.ReRizReturnEncryptResponseWithSign ChangeNow(string header_token, string requestBody, string verify)
        {
            string token_email = Classes.TokenGenerator.CheckToken(header_token);
            if (token_email == null)
            {
                return Tools.ReRizTools.BuildEncryptMessage("token_error", verify);
            }

            var body = JsonConvert.DeserializeObject<Classes.RizApiChangeUsernameRequest>(requestBody);

            Osp_DB.SearchFilter SearchFilter = new Osp_DB.SearchFilter("RizServerCoreSharp_RizUserAccountObject", null, null);
            var SearchResult = Classes.DBMain.SearchWithFilter(GlobalConfig.DBConfig.JsonName, SearchFilter);
            foreach (var item in SearchResult)
            {
                var NewObjectToModify = JsonConvert.DeserializeObject<Classes.RizAccount>(item.obj.ToString());
                if (NewObjectToModify._id.Split(">")[2] == token_email)
                {
                    NewObjectToModify.username = body.username;
                    Osp_DB.SearchFilter ModifySearchFilter = new Osp_DB.SearchFilter("RizServerCoreSharp_RizUserAccountObject", item.label, null);
                    Classes.DBMain.ModifyObject(GlobalConfig.DBConfig.JsonName, ModifySearchFilter, NewObjectToModify);
                    return Tools.ReRizTools.BuildEncryptMessage("{\"code\": 0, \"data\": {}}", verify);
                }
            }
            return Tools.ReRizTools.BuildEncryptMessage("token_error", verify);
        }
    }
}
