using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetSh
{
    /// <summary>
    /// Lots of questions about what this should look like. I'm making temp files, calling shell stuff
    /// and generally being evil. Should this be IDispoable? Is it better to call something in WMI?
    /// Your thoughts are appreciate. My goal is "it works" and "fewer lines of code." 
    /// Not to mention this was hacked together in 20 minutes. - Scott Hanselman
    /// </summary>
    public class NetShWrapper
    {
        static XNamespace ns = "http://www.microsoft.com/networking/WLAN/profile/v1";

        public static async Task<List<WifiProfile>> GetWifiProfilesAsync()
        {
            await ExportAllWifiProfilesAsync();

            var profiles = new List<WifiProfile>();
            foreach (string file in Directory.EnumerateFiles(Environment.CurrentDirectory, "*.xml"))
            {
                var x = XElement.Load(file);
                if (x.Name.Namespace == ns)
                {
                    profiles.Add(new WifiProfile()
                    {
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

        private static async Task ExportAllWifiProfilesAsync()
        {
            await Task.Run(() =>
            {
                string result = ExecuteNetSh("wlan show profiles");

                var listOfProfiles = Regex.Matches(result, "(?<=: )(.*?)(?=\\r)", RegexOptions.IgnoreCase);

                foreach (var match in listOfProfiles)
                {
                    ExecuteNetSh(String.Format("wlan export profile \"{0}\" folder=\"{1}\"", match.ToString(), Environment.CurrentDirectory));
                }
            });
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
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = arguments ?? String.Empty;
            p.StartInfo.UseShellExecute = false;
            //p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();

            string output = p.StandardOutput.ReadToEnd();
            return output;
        }
    }
}

