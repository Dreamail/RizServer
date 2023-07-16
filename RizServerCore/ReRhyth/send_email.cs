using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RizServerCoreSharp.ReRhyth
{
    public static class SendEmail
    {
        public static string Send(string requestbody)
        {
            Classes.RhythAccountSendEmailRequest req = JsonConvert.DeserializeObject<Classes.RhythAccountSendEmailRequest>(requestbody);
            //TODO: Send email with SMTP
            return "";
        }
    }
}
