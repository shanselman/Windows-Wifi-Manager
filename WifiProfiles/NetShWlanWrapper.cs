﻿using System.Text.RegularExpressions;

namespace NetSh
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Lots of questions about what this should look like. I'm making temp files, calling shell stuff
    /// and generally being evil. Should this be IDispoable? Is it better to call something in WMI?
    /// Your thoughts are appreciate. My goal is "it works" and "fewer lines of code." 
    /// Not to mention this was hacked together in 20 minutes. - Scott Hanselman
    /// </summary>
    public class NetShWrapper : ExecuteCommand.Execute
    {
        static XNamespace ns = "http://www.microsoft.com/networking/WLAN/profile/v1";

        public static List<WifiProfile> GetWifiProfiles()
        {
            ExportAllWifiProfiles();

            var profiles = new List<WifiProfile>();
            foreach (string file in Directory.EnumerateFiles(Environment.CurrentDirectory, "*.xml"))
            {
                var x = XElement.Load(file);
                if (x.Name.Namespace == ns)
                {
                    profiles.Add(new WifiProfile() { 
                        Name = x.Descendants(ns + "name").First().Value,
                        ConnectionMode = x.Descendants(ns + "connectionMode").First().Value,
                        Authentication = x.Descendants(ns + "authentication").First().Value 
                    });
                }
            }
            DeleteExportedWifiProfiles();
            return profiles;
        }

        public static string DeleteWifiProfile(string profileName)
        {
            string result = ExecuteNetSh(String.Format("wlan delete profile name=\"{0}\"", profileName));
            return result;

        }

        public static bool IsOpenAndAutoWifiProfile(WifiProfile profile)
        {
            return profile.Authentication == "open" && profile.ConnectionMode == "auto";
        }

        private static void ExportAllWifiProfiles()
        {
            //string result = "\r\nProfiles on interface Wi-Fi:\r\n\r\nGroup policy profiles (read only)\r\n---------------------------------\r\n    A-MSFTWLAN (WPA2)\r\n    A-MSFTWLAN (WPA)\r\n    MSFTWLAN (WEP)\r\n\r\nUser profiles\r\n-------------\r\n    All User Profile     : Wayport_Access\r\n    All User Profile     : nalen dressingroom\r\n    All User Profile     : Scott's iPhone 4s\r\n    All User Profile     : HANSELMAN\r\n    All User Profile     : HANSELMAN-N\r\n    All User Profile     : HanselSpot\r\n    All User Profile     : SFO-WiFi\r\n    All User Profile     : AP-guest\r\n    All User Profile     : EliteWifi\r\n    All User Profile     : Qdoba Free Wifi\r\n    All User Profile     : Sunrise_Bagels\r\n\r\n";
            string result = ExecuteNetSh("wlan show profiles");

            var listOfProfiles = Regex.Matches(result, "(?<=: )(.*?)(?=\\r)", RegexOptions.IgnoreCase);

            foreach (var match in listOfProfiles)
            {
                ExecuteNetSh(String.Format("wlan export profile \"{0}\" folder=\"{1}\"", match.ToString(), Environment.CurrentDirectory));

            }
        }

        public static void DeleteExportedWifiProfiles()
        {
            //Delete the exported profiles we made, making sure they are what we think they are!
            foreach (string file in Directory.EnumerateFiles(Environment.CurrentDirectory, "*.xml"))
                if (XElement.Load(file).Name.Namespace == ns)
                    File.Delete(file);
        }

    }
}

