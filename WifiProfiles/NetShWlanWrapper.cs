using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NetSh
{
    public class WifiProfile
    {
        public string Name { get; set; }
        public string Authentication { get; set; }
        public string ConnectionMode { get; set; }
    }

    /// <summary>
    /// Lots of questions about what this should look like. I'm making temp files, calling shell stuff
    /// and generally being evil. Should this be IDisposible? Is it better to call something in WMI?
    /// Your thoughts are appreciated. My goal is "it works" and "fewer lines of code." 
    /// Not to mention this was hacked together in 20 minutes. - Scott Hanselman
    /// </summary>
    public class NetShWrapper
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
                    //Yeah, I know.
                    string name = x.Descendants(ns + "name").First().Value;
                    string mode = x.Descendants(ns + "connectionMode").First().Value;
                    string auth = x.Descendants(ns + "authentication").First().Value;

                    profiles.Add(new WifiProfile() { Name = name, ConnectionMode = mode, Authentication = auth });
                }
            }
            DeleteExportedWifiProfiles();
            return profiles;
        }

        public static string DeleteWifiProfile(string profileName)
        {
            return ExecuteNetSh("wlan delete profile name=\"" + profileName + "\"");
        }

        public static bool IsOpenAndAutoWifiProfile(WifiProfile profile)
        {
            return String.Equals(profile.Authentication, "open", StringComparison.CurrentCultureIgnoreCase)
                && String.Equals(profile.ConnectionMode, "auto", StringComparison.CurrentCultureIgnoreCase);
        }

        private static void ExportAllWifiProfiles()
        {
            string result = ExecuteNetSh("wlan show profiles");
            var listOfProfiles = from line in result.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                                 where line.Contains(":")
                                 let l = line
                                 where l.Last() != ':'
                                 select l.Split(':')[1].Trim();

            foreach (string profile in listOfProfiles)
                ExecuteNetSh(String.Format("wlan export profile \"{0}\" folder=\"{1}\"", profile, Environment.CurrentDirectory));
        }

        public static void DeleteExportedWifiProfiles()
        {
            //Delete the exported profiles we made, making sure they are what we think they are! 
            foreach (string file in Directory.EnumerateFiles(Environment.CurrentDirectory, "*.xml"))
                if (XElement.Load(file).Name.Namespace == ns)
                    File.Delete(file);
        }

        private static string ExecuteNetSh(string arguments = null)
        {
            Process p = new Process();
            p.StartInfo.FileName = "netsh.exe";
            p.StartInfo.Arguments = arguments ?? String.Empty;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();

            return p.StandardOutput.ReadToEnd();
        }
    }
}

