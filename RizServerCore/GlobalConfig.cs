using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RizServerCoreSharp
{
    public class GlobalConfig
    {
        public static void InitCore()
        {
            // Init for Osp-DB
            Classes.DBMain.Init();
            Classes.DBMain.InitTargetJsonFile(DBConfig.JsonName);
        }

        public class DBConfig
        {
            public static string JsonName = "RizServerCoreSharp";
        }
    }
}
