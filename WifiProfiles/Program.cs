using NetSh;
using System;
using System.Linq;

namespace WifiProfiles
{
    class Program
    {
        static void Main(string[] args)
        { 
            //command line arguments officially got irritating at this point but I don't feel like bringing in a whole library.
            if (args.Length > 1 && args[0].ToUpperInvariant() == "DELETE" && !string.IsNullOrEmpty(args[1]))
                Delete(args[1]);
            else
                List(args.Length == 1 && args[0].ToUpperInvariant() == "/DELETEAUTOOPEN");
        }

        static void Delete(string profileName)
        {
            Console.WriteLine(NetShWrapper.DeleteWifiProfile(profileName));
        }

        static void List(bool autoDelete)
        {
            var profiles = NetShWrapper.GetWifiProfiles();
            var badWifiNetworkFound = false;

            foreach (var a in profiles)
            {
                var warning = NetShWrapper.IsOpenAndAutoWifiProfile(a) ? "Warning: AUTO connect to OPEN WiFi" : String.Empty;
                Console.WriteLine("{0,-20} {1,10} {2,10} {3,30} ", a.Name, a.ConnectionMode, a.Authentication, warning);
                if (!String.IsNullOrWhiteSpace(warning)) badWifiNetworkFound = true;
            }

            if (badWifiNetworkFound)
            {
                if(!autoDelete) Console.WriteLine("\r\nDelete WiFi profiles that are OPEN *and* AUTO connect? [y/n]");
                if (autoDelete || Console.ReadLine().Trim().ToUpperInvariant().StartsWith("Y"))
                {
                    foreach (var a in profiles.Where(NetShWrapper.IsOpenAndAutoWifiProfile))
                    {
                        Console.WriteLine(NetShWrapper.DeleteWifiProfile(a.Name));
                    }
                }
            }
            else
            {
                Console.WriteLine("\r\nNo WiFi profiles set to OPEN and AUTO connect were found. \r\nOption: Run with /deleteautoopen to auto delete.");
            }
        }
    }
}
