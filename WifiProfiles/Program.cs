using NetSh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WifiProfiles
{
    class Program
    {
        static void Main(string[] args)
        {
            var profiles = NetShWrapper.GetWifiProfiles();
            bool sawBadWifi = false;
            foreach (var a in profiles)
            {
                string warning = NetShWrapper.IsOpenAndAutoWifiProfile(a) ? "Warning: AUTO connect to OPEN WiFi" : String.Empty;
                Console.WriteLine(String.Format("{0,-20} {1,10} {2,10} {3,30} ", a.Name, a.ConnectionMode, a.Authentication, warning));
                if (!String.IsNullOrWhiteSpace(warning)) sawBadWifi = true;
            }
            if (sawBadWifi)
            {
                Console.WriteLine("\r\nDelete WiFi profiles that are OPEN *and* AUTO connect? [y/n]");
                if (args[0].ToUpperInvariant() == "/DELETEAUTOOPEN" || Console.ReadLine().Trim().ToUpperInvariant()[0] == 'Y')
                {
                    Console.WriteLine("in here");
                    foreach (var a in profiles.Where(a => NetShWrapper.IsOpenAndAutoWifiProfile(a)))
                    {
                        Console.WriteLine(NetShWrapper.DeleteWifiProfile(a));
                    }
                }
            }
            else
            {
                Console.WriteLine("\r\nNo WiFi profiles set to OPEN and AUTO connect were found. \r\nOption: Run with /deleteautoopen to auto delete.");
            }
            //Console.ReadKey();
        }
    }
}
