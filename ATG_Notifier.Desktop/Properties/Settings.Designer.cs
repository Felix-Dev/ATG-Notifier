﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ATG_Notifier.Desktop.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.4.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool DoNotDisturb {
            get {
                return ((bool)(this["DoNotDisturb"]));
            }
            set {
                this["DoNotDisturb"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool PlayPopupSound {
            get {
                return ((bool)(this["PlayPopupSound"]));
            }
            set {
                this["PlayPopupSound"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool DisableOnFullscreen {
            get {
                return ((bool)(this["DisableOnFullscreen"]));
            }
            set {
                this["DisableOnFullscreen"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string CurrentChapterId {
            get {
                return ((string)(this["CurrentChapterId"]));
            }
            set {
                this["CurrentChapterId"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("TopRight")]
        public global::ATG_Notifier.Desktop.Views.ToastNotification.DisplayPosition NotificationDisplayPosition {
            get {
                return ((global::ATG_Notifier.Desktop.Views.ToastNotification.DisplayPosition)(this["NotificationDisplayPosition"]));
            }
            set {
                this["NotificationDisplayPosition"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string WindowSetting {
            get {
                return ((string)(this["WindowSetting"]));
            }
            set {
                this["WindowSetting"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool WasUpdateServiceRunning {
            get {
                return ((bool)(this["WasUpdateServiceRunning"]));
            }
            set {
                this["WasUpdateServiceRunning"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string MostRecentChapterInfo {
            get {
                return ((string)(this["MostRecentChapterInfo"]));
            }
            set {
                this["MostRecentChapterInfo"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool KeepRunningOnClose {
            get {
                return ((bool)(this["KeepRunningOnClose"]));
            }
            set {
                this["KeepRunningOnClose"] = value;
            }
        }
    }
}
