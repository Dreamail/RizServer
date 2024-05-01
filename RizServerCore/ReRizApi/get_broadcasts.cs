using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RizServerCoreSharp.ReRizApi
{
    public static class GetBroadCasts
    {
        public static Classes.ReRizReturnEncryptResponseWithSign GetNow(string verify)
        {
            return Tools.ReRizTools.BuildEncryptMessage("{\"code\":0,\"data\":{\"syncId\":\"114514\",\"broadcasts\":[\"broadcastId\": 0, \"endTime\": \"2024-05-09T05:41:54.735Z\", \"interval\": 10, \"isCountdown\": false, \"startTime\": 2024-05-01T05:41:54.735Z, \"message\": \"Welcome to RizServer\"]}}", verify);
        }
    }
}
