using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RizServerCoreSharp.ReRizApi
{
    public static class GetUserShop
    {
        public static Classes.ReRizReturnEncryptResponseWithSign GetNow(string header_token, string requestBody, string verify)
        {
            return Tools.ReRizTools.BuildEncryptMessage("{\"code\":0,\"data\":{\"shop\":[{\"id\":\"bio.exbio00057;bio.exbio00055\",\"price\":1145,\"leftPrice\":14,\"rightPrice\":250,\"isCoinPrice\":false},{\"id\":\"bio.exbio00019;bio.exbio00033\",\"price\":500,\"leftPrice\":250,\"rightPrice\":250,\"isCoinPrice\":false},{\"id\":\"layout.00005\",\"price\":500,\"isCoinPrice\":false},{\"id\":\"illustration.PowerAttack.EBIMAYO.0\",\"price\":500,\"isCoinPrice\":false}],\"refreshTime\":\"2024-05-06T00:00:00.000Z\",\"refreshed\":false}}", verify);
        }
    }
}
