using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RizServerCoreSharp
{
    public class Classes
    {
        public static Osp_DB.DBMain DBMain = new Osp_DB.DBMain();
        public static Tools.TokenGenerator TokenGenerator = new Tools.TokenGenerator();

        public class RhythAccountCheckEmailRequest
        {
            public string email;
        }

        public class RhythAccountCheckEmailResponse
        {
            public int code;
            public string msg;
        }

        public class RhythAccountLoginRequest
        {
            public string email;
            public string password;
        }

        public class RhythAccountRegisterRequest
        {
            public string email;
            public string password;
            public string code;
        }

        public class RhythAccountSendEmailRequest
        {
            public string email;
            public string transaction;
        }

        public class RhythAccountResponseWithToken
        {
            public string ret;
            public string header_set_token;
        }

        public class RhythAccountAvatar
        {
            public string name = "testAvatar";
            public string portrait = "";
        }

        public class RhythAccount
        {
            public string email;
            public string password;
            public RhythAccountAvatar avatar;
        }
    }
}
