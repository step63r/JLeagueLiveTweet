//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MinatoProject.Apps.JLeagueLiveTweet.Core.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("11223b4a-239b-45ae-a2de-3bb8603154bf")]
        public global::System.Guid PrivateGuid {
            get {
                return ((global::System.Guid)(this["PrivateGuid"]));
            }
            set {
                this["PrivateGuid"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("rSlgVfctHz2wKr0nDn2jgYbbx")]
        public string TwitterApiKey {
            get {
                return ((string)(this["TwitterApiKey"]));
            }
            set {
                this["TwitterApiKey"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("6DEDeKLbpyuegSTRHQctVVA3KsLRCdFV1YBK2a6XWeteo58gKq")]
        public string TwitterApiSecret {
            get {
                return ((string)(this["TwitterApiSecret"]));
            }
            set {
                this["TwitterApiSecret"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("AAAAAAAAAAAAAAAAAAAAALh7TQEAAAAAc0qgl4pdwj3S3F7A9baf%2BwssLEs%3D8d3TL6JvK8BV48buQ" +
            "0REn4Ydt2LjefNRWNhSbW9vzyALfOs3vP")]
        public string TwitterBearerToken {
            get {
                return ((string)(this["TwitterBearerToken"]));
            }
            set {
                this["TwitterBearerToken"] = value;
            }
        }
    }
}
