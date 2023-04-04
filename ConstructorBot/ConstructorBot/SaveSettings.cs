using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot
{
    public static class SaveSettings
    {
        public static void SetKeyTelegram(string token)
        {
            Preferences.Set("telegram", token);
        }

        public static string GetKeyTelegram()
        {
            return Preferences.Get("telegram", null);
        }
    }
}
