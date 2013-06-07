using NetSh;
using System;
using System.Diagnostics;
using System.Linq;

namespace WifiProfiles
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Any() && String.Equals(args.First(), "DELETE", StringComparison.CurrentCultureIgnoreCase)) // You're using LINQ anyway?
                Delete(args[1]);
            else
                List(args.Length == 1 && String.Equals(args.First(), "/DELETEAUTOOPEN", StringComparison.CurrentCultureIgnoreCase));
        }

        static void Delete(string profileName)
        {
            Console.WriteLine(NetShWrapper.DeleteWifiProfile(profileName));
        }

        static void Pause()
        {
            Console.WriteLine();
            var pauseProc = Process.Start(new ProcessStartInfo()
                {
                    FileName = "cmd",
                    Arguments = "/C pause",
                    UseShellExecute = false
                });
            pauseProc.WaitForExit();
        }

        static void List(bool autoDelete)
        {
            var profiles = NetShWrapper.GetWifiProfiles();
            bool sawBadWifi = false;
            foreach (var a in profiles)
            {
                string warning = NetShWrapper.IsOpenAndAutoWifiProfile(a) ? "Warning: AUTO connect to OPEN WiFi" : String.Empty;
                // Console.WriteLine has a "formatting" overload
                Console.WriteLine("{0,-20} {1,10} {2,10} {3,30} ", a.Name, a.ConnectionMode, a.Authentication, warning);
                if (!String.IsNullOrWhiteSpace(warning)) sawBadWifi = true;
            }
            if (sawBadWifi)
            {
                if (!autoDelete) Console.WriteLine("{0}Delete WiFi profiles that are OPEN *and* AUTO connect? [y/n]", Environment.NewLine);
                if (autoDelete || Console.ReadLine().Trim().ToUpperInvariant().StartsWith("Y")) // Another String.Equals CurrentCultureIgnoreCase?
                    foreach (var a in profiles.Where(a => NetShWrapper.IsOpenAndAutoWifiProfile(a)))
                        Console.WriteLine(NetShWrapper.DeleteWifiProfile(a.Name));
            }
            else
            {
                Console.WriteLine("{0}No WiFi profiles set to OPEN and AUTO connect were found." +
                                  "{0}Option: Run with /deleteautoopen to auto delete.", Environment.NewLine);
            }

            if (Environment.UserInteractive) Pause();
        }
    }
}
