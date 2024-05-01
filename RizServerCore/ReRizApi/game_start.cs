using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RizServerCoreSharp.ReRizApi
{
    public static class GameStart
    {
        public static Classes.ReRizReturnEncryptResponseWithSign Start(string verify)
        {
            return Tools.ReRizTools.BuildEncryptMessage("{\"code\":0,\"data\":\"3c89a1af974ee9d5a99321a903e0aad6\"}", verify);
        }
    }
}
