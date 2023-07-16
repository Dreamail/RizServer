using RizServerCoreSharp;
using Newtonsoft.Json;

namespace RizServerCoreSharp
{
    public static class CheckEmail
    {
        public static void Check(String requestbody)
        {
            Classes.CheckEmailRequest req = JsonConvert.DeserializeObject<Classes.CheckEmailRequest>(requestbody);
        }
    }
}