using RizServerCoreSharp;
using Newtonsoft.Json;

namespace RizServerCoreSharp.ReRhyth
{
    public static class CheckEmail
    {
        public static string Check(String requestbody)
        {
            Classes.RhythAccountCheckEmailRequest req = JsonConvert.DeserializeObject<Classes.RhythAccountCheckEmailRequest>(requestbody);
            Osp_DB.SearchFilter SearchFilter = new Osp_DB.SearchFilter("RizServerCoreSharp_ReRhythUserAccountObject", null, null); //Get all accounts
            var SearchResult = Classes.DBMain.SearchWithFilter(GlobalConfig.DBConfig.JsonName, SearchFilter);
            foreach (var item in SearchResult)
            {
                var itemobj = JsonConvert.DeserializeObject<Classes.RhythAccount>(item.obj.ToString());
                if (itemobj.email == req.email)
                {
                    return JsonConvert.SerializeObject(new Classes.RhythAccountCheckEmailResponse { code = 0 }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }
            }
            return JsonConvert.SerializeObject(new Classes.RhythAccountCheckEmailResponse { code = 2, msg = "ÕË»§Î´×¢²á" });
        }
    }
}