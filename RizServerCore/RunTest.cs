﻿namespace RizServerCoreSharp
{
    public class RizServerCoreSharpTest
    {
        /*
         Warning: If you want to use RizServerCoreSharp, 
         you should create a separate C# project and reference this namespace instead of directly modifying it. 
         This Main function is just for running tests.
        */

        static void Main(string[] args)
        {
            GlobalConfig.InitCore();
            Console.WriteLine(ReRhyth.CheckEmail.Check("{\"email\" : \"searchstars@1.com\"}"));
            Console.WriteLine(ReRhyth.Register.Reg("{\"email\":\"searchstars@11.com\",\"password\":\"MADFDF0\",\"code\":\"123456\"}").ret);
            Console.WriteLine(ReRhyth.CheckEmail.Check("{\"email\" : \"searchstars@11.com\"}"));
            Console.WriteLine(ReRhyth.RhythAccountLogin.Login("{\"email\":\"searchstars@aa.com\",\"password\":\"MADFDF0\"}").ret);
            Console.WriteLine(ReRhyth.RhythAccountLogin.Login("{\"email\":\"searchstars@11.com\",\"password\":\"MADFDF0\"}").ret);
            Console.ReadLine();
        }
    }
}