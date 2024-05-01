using Newtonsoft.Json;

namespace RizServerCoreSharp.ReRizApi
{
    public static class RizLogin
    {
        public static Classes.ReRizReturnEncryptResponseWithSign Login(string header_token, string verify)
        {
            string token_email = Classes.TokenGenerator.CheckToken(header_token);
            if (token_email == null) {
                return Tools.ReRizTools.BuildEncryptMessage("token_error", verify);
            }
            else
            {
                Osp_DB.SearchFilter SearchFilter = new Osp_DB.SearchFilter("RizServerCoreSharp_RizUserAccountObject", null, null);
                var SearchResult = Classes.DBMain.SearchWithFilter(GlobalConfig.DBConfig.JsonName, SearchFilter);
                foreach (var item in SearchResult)
                {
                    var itemobj = JsonConvert.DeserializeObject<Classes.RizAccount>(item.obj.ToString());
                    if (itemobj._id.Split(">")[2] == token_email)
                    {
                        string ret_str = JsonConvert.SerializeObject(itemobj);
                        Console.WriteLine(ret_str);
                        return Tools.ReRizTools.BuildEncryptMessage(ret_str, verify);
                    }
                }
                return Tools.ReRizTools.BuildEncryptMessage("token_error", verify);
            }
        }
    }
}