﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WifiProfiles.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class stringResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal stringResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WifiProfiles.Resources.stringResources", typeof(stringResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /DELETEAUTOOPEN.
        /// </summary>
        internal static string AutoDeletarParam {
            get {
                return ResourceManager.GetString("AutoDeletarParam", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Y.
        /// </summary>
        internal static string AutoDeleteChar {
            get {
                return ResourceManager.GetString("AutoDeleteChar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /ADHOC.
        /// </summary>
        internal static string CreateHostedNetWork {
            get {
                return ResourceManager.GetString("CreateHostedNetWork", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE.
        /// </summary>
        internal static string Delete {
            get {
                return ResourceManager.GetString("Delete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///Delete WiFi profiles that are OPEN *and* AUTO connect? [y/n].
        /// </summary>
        internal static string DeleteAutoConnect {
            get {
                return ResourceManager.GetString("DeleteAutoConnect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /DRIVERSINFO.
        /// </summary>
        internal static string DriversInformation {
            get {
                return ResourceManager.GetString("DriversInformation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hostednetwork Not Supported!!.
        /// </summary>
        internal static string NotSupported {
            get {
                return ResourceManager.GetString("NotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No WiFi profiles set to OPEN and AUTO connect were found.
        ///Option: 
        ///Run with /deleteautoopen to auto delete.
        ///Run with /DRIVERSINFO to Get Driver Info.
        ///Run with /ADHOC ssid password to Create HostedNetwork..
        /// </summary>
        internal static string NoWifi {
            get {
                return ResourceManager.GetString("NoWifi", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Warning: AUTO connect to OPEN WiFi.
        /// </summary>
        internal static string WarningAutoConnect {
            get {
                return ResourceManager.GetString("WarningAutoConnect", resourceCulture);
            }
        }
    }
}
