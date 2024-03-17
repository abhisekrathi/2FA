using _2FA.Models;
namespace _2FA.Extensions
{
    public static class CodeExtensions
    {
        public static bool SendCodeBySMS(this Code c)
        {
            Console.WriteLine(c.CodeValue + " Sent By SMS");
            return true;
        }
        public static bool SendCodeByRCS(this Code c) //Additional Options Added For Example. Not Being Used
        {
            Console.WriteLine(c.CodeValue + " Sent By RCS");
            return true;
        }
        public static bool SendCodeByWhatsApp(this Code c) //Additional Options Added For Example. Not Being Used
        {
            Console.WriteLine(c.CodeValue + " Sent By WhatsApp");
            return true;
        }
    }
}