using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RizServerCoreSharp.ReRizApi
{
    public static class SpecialEvent
    {
        public static Classes.ReRizReturnEncryptResponseWithSign GetNow(string verify)
        {
            return Tools.ReRizTools.BuildEncryptMessage("{\"code\":0,\"data\":{\"allStar\": 99, \"maxStar\": 100}}", verify);
        }
    }
}
