﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LittleBrother.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Menu : global::System.Configuration.ApplicationSettingsBase {
        
        private static Menu defaultInstance = ((Menu)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Menu())));
        
        public static Menu Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Add project...")]
        public string NewProjectWatermarkText {
            get {
                return ((string)(this["NewProjectWatermarkText"]));
            }
            set {
                this["NewProjectWatermarkText"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\"How long?\"")]
        public string NewIntervalWatermarkText {
            get {
                return ((string)(this["NewIntervalWatermarkText"]));
            }
            set {
                this["NewIntervalWatermarkText"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("You asked to be reminded of something.")]
        public string ReminderTextWatermarkText {
            get {
                return ((string)(this["ReminderTextWatermarkText"]));
            }
            set {
                this["ReminderTextWatermarkText"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string ClearText {
            get {
                return ((string)(this["ClearText"]));
            }
            set {
                this["ClearText"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("200,200,200")]
        public string WatermarkTextColor {
            get {
                return ((string)(this["WatermarkTextColor"]));
            }
            set {
                this["WatermarkTextColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0,0,0")]
        public string NewItemTextColor {
            get {
                return ((string)(this["NewItemTextColor"]));
            }
            set {
                this["NewItemTextColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("50,0,200,200")]
        public string ActiveTaskColor {
            get {
                return ((string)(this["ActiveTaskColor"]));
            }
            set {
                this["ActiveTaskColor"] = value;
            }
        }
    }
}
