namespace WifiProfiles
{
    using NetSh;
    using System;
    using System.Linq;
    using Wifiadhoc;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //command line arguments officially got irritating at this point but I don't feel like bringing in a whole library.
                if (args[0].ToUpperInvariant() == Resources.stringResources.AutoDeletarParam 
                    && args.Length > 1 
                    && !string.IsNullOrEmpty(args[1]))
                {
                    Delete(args[1]);
                    return;
                }

                if (args[0].ToUpperInvariant() == Resources.stringResources.CreateHostedNetWork
                    && args.Length > 2 
                    && !string.IsNullOrEmpty(args[1]) 
                    && !string.IsNullOrEmpty(args[2]))
                {
                    CreateAdhoc(args[1], args[2]);
                    return;
                }

                if (args[0].ToUpperInvariant() == Resources.stringResources.DriversInformation)
                {
                    DriversInfo();
                    return;
                }

                List(args.Length == 1 && args[0].ToUpperInvariant() == Resources.stringResources.AutoDeletarParam);
            }
            catch
            {
                List(false);
            }

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
                var warning = NetShWrapper.IsOpenAndAutoWifiProfile(a) ? Resources.stringResources.WarningAutoConnect : String.Empty;
                Console.WriteLine("{0,-20} {1,10} {2,10} {3,30} ", a.Name, a.ConnectionMode, a.Authentication, warning);
                if (!String.IsNullOrWhiteSpace(warning)) badWifiNetworkFound = true;
            }

            if (badWifiNetworkFound)
            {
                if (!autoDelete) Console.WriteLine(Resources.stringResources.DeleteAutoConnect);
                if (autoDelete || Console.ReadLine().Trim().ToUpperInvariant().StartsWith(Resources.stringResources.AutoDeleteChar))
                {
                    foreach (var a in profiles.Where(NetShWrapper.IsOpenAndAutoWifiProfile))
                    {
                        Console.WriteLine(NetShWrapper.DeleteWifiProfile(a.Name));
                    }
                }
            }
            else
            {
                Console.WriteLine(Resources.stringResources.NoWifi);
            }
        }

        static void CreateAdhoc(string ssId, string password)
        {
            Console.WriteLine(Adhoc.CreateHostedNetWork(ssId, password));
        }

        static void DriversInfo()
        {
            string[] result = Adhoc.DriversInfo();
            foreach (string temp in result)
            {
                Console.WriteLine(temp);
            }
        }
    }
}
