using System;

namespace Wifiadhoc
{
    public class Adhoc : ExecuteCommand.Execute
    {
        public static string[] DriversInfo()
        {
            string result = ExecuteNetSh("wlan show drivers");
            return System.Text.RegularExpressions.Regex.Split(result, "\r\n");
        }

        public static string CreateHostedNetWork(string ssid, string password)
        {
            if (!IsSupported(DriversInfo()))
                return "Hostednetwork Not Supported!!";

            string result = ExecuteNetSh(String.Format("wlan set hostednetwork mode=allow ssid={0} key={1}", ssid, password));

            result += Environment.NewLine;
            result += ExecuteNetSh(String.Format("wlan start hostednetwork"));
            return result;
        }

        private static bool IsSupported(string[] driveInfo)
        {
            foreach (string temp in driveInfo)
            {
                if (temp.Contains("Hosted network supported"))
                {
                    if (temp.Split(':')[1].Trim().ToLower() == "yes")
                        return true;
                }
            }
            return false;
        }
    }
}
