using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RizServerCoreSharp.Classes;

namespace RizServerCoreSharp.ReRizApi
{
    public static class check_buy_count
    {
        public static ReRizReturnEncryptResponseWithSign Check()
        {
            return Tools.ReRizTools.BuildEncryptMessage("{\"goodId\":30000,\"code\":0,\"message\":\"6666\"}");//某公司讨钱用的计数器
        }
    }
}
