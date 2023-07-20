using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RizServerCoreSharp.Classes;

namespace RizServerCoreSharp.ReRizApi
{
    public static class AfterPlay
    {
        public static bool AfterPlayIsFullCombo(double completeRate)
        {
            if (completeRate < 120) {
                return false;
            }
            return true;
        }

        public static bool AfterPlayIsClear(double completeRate)
        {
            if(completeRate < 100)
            {
                return false;
            }
            return true;
        }

        public static ReRizReturnEncryptResponseWithSign AfterPlayMain(string header_token,string requestbody)
        {
            ReRizAfterPlayRequest req = JsonConvert.DeserializeObject<ReRizAfterPlayRequest>(requestbody);

            string token_email = Classes.TokenGenerator.CheckToken(header_token);
            if(token_email == null)
            {
                return Tools.ReRizTools.BuildEncryptMessage("token_error");
            }

            var NewBestSave = new RizAccountMyBest
            {
                trackAssetId = req.TrackAssetId,
                difficultyClassName = req.DifficultyClassName,
                score = req.Score,
                completeRate = req.CompleteRate,
                isFullCombo = AfterPlayIsFullCombo(req.CompleteRate),
                isClear = AfterPlayIsClear(req.CompleteRate)
            };

            Osp_DB.SearchFilter SearchFilter = new Osp_DB.SearchFilter("RizServerCoreSharp_RizUserAccountObject", null, null);
            var SearchResult = Classes.DBMain.SearchWithFilter(GlobalConfig.DBConfig.JsonName, SearchFilter);
            foreach (var item in SearchResult)
            {
                var itemobj = JsonConvert.DeserializeObject<Classes.RizAccount>(item.obj.ToString());
                if (itemobj.username == token_email)
                {
                    var NewObjectToModify = itemobj;
                    var dotadd = Classes.RandomObject.Next(10, 50);
                    NewObjectToModify.myBest.Add(NewBestSave);
                    NewObjectToModify.dot = NewObjectToModify.dot + dotadd;

                    Osp_DB.SearchFilter ModifySearchFilter = new Osp_DB.SearchFilter("RizServerCoreSharp_RizUserAccountObject", item.label, null);
                    Classes.DBMain.ModifyObject(GlobalConfig.DBConfig.JsonName, ModifySearchFilter, NewObjectToModify);

                    return Tools.ReRizTools.BuildEncryptMessage("{\"newDot\":" + itemobj.dot + ",\"deltaDot\":" + dotadd + ",\"dropedItems\":[],\"dropedLevels\":[]}");
                }
            }
            return Tools.ReRizTools.BuildEncryptMessage("token_error");
        }
    }
}
