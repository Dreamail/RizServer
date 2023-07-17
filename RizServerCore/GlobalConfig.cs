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

        public class DynamicConfigTemplate
        {
            public string aes_key = "Sv@H,+SV-U*VEjCW,n7WA-@n}j3;U;XF";
            public string aes_iv = "1%[OB.<YSw?)o:rQ";

            public string resources_path = "./resources";
        }//DynamicConfig不能直接读GlobalConfig.DynamicConfigTemplate，应该读Classes.LoadedConfig
    }
}
