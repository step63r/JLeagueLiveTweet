using MinatoProject.Apps.JLeagueLiveTweet.Core.Models;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MinatoProject.Apps.JLeagueLiveTweet.Core.Services
{
    /// <summary>
    /// ユーザー設定情報をストアするクラス
    /// </summary>
    public sealed class ConfigStore : ServiceStoreBase
    {
        #region Singleton
        /// <summary>
        /// シングルトン インスタンス
        /// </summary>
        private static readonly ConfigStore _instance = new ConfigStore();

        /// <summary>
        /// インスタンスを取得する
        /// </summary>
        /// <returns></returns>
        public static ConfigStore GetInstance()
        {
            return _instance;
        }
        #endregion

        #region メンバ変数
        /// <summary>
        /// ユーザー設定情報
        /// </summary>
        private Config _config = new Config();
        /// <summary>
        /// クラブ情報ファイルパス
        /// </summary>
        private static readonly string _configFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\JLeagueLiveTweet\config.xml";
        /// <summary>
        /// Guid
        /// </summary>
        private static readonly Guid _privateGuid = Properties.Settings.Default.PrivateGuid;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private ConfigStore()
        {
        }
        #endregion

        #region メソッド
        /// <summary>
        /// インスタンスを初期化する
        /// </summary>
        public override void InitializeInstance()
        {
            _logger.Info("start");
            // ファイルが存在しない場合、新規作成
            if (!File.Exists(_configFilePath))
            {
                Serialize(_config, _configFilePath);
            }
            _config = Deserialize<Config>(_configFilePath);
            _logger.Info("end");
        }

        /// <summary>
        /// ユーザー設定情報を取得する
        /// </summary>
        /// <returns>ユーザー設定情報</returns>
        public Config GetConfig()
        {
            _logger.Info("start");
            _logger.Info("end");
            return _config;
        }

        /// <summary>
        /// ユーザー設定情報を更新する
        /// </summary>
        /// <param name="config">ユーザー設定情報</param>
        /// <returns>更新後のユーザー設定情報</returns>
        public Config SetConfig(Config config)
        {
            _logger.Info("start");
            _config = config;
            Serialize(_config, _configFilePath);
            _logger.Info("end");
            return _config;
        }

        /// <summary>
        /// マイクラブを更新する
        /// </summary>
        /// <param name="club">クラブ</param>
        /// <returns>更新後のユーザー設定情報</returns>
        public Config SetMyClub(Club club)
        {
            _logger.Info("start");
            _config.MyClub = club;
            Serialize(_config, _configFilePath);
            _logger.Info("end");
            return _config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetTwitterApiKey()
        {
            _logger.Info("start");
            _logger.Info("end");
            return Properties.Settings.Default.TwitterApiKey;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetTwitterApiSecret()
        {
            _logger.Info("start");
            _logger.Info("end");
            return Properties.Settings.Default.TwitterApiSecret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetTwitterBearerToken()
        {
            _logger.Info("start");
            _logger.Info("end");
            return Properties.Settings.Default.TwitterBearerToken;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetTwitterAccessToken()
        {
            _logger.Info("start");
            string ret = string.Empty;
            if (!string.IsNullOrEmpty(_config.TwitterAccessToken))
            {
                try
                {
                    ret = Unprotect(_config.TwitterAccessToken);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            }
            _logger.Info("end");
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetTwitterAccessToken(string value)
        {
            _logger.Info("start");
            string src = value;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    src = Protect(value);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            }
            _config.TwitterAccessToken = src;
            Serialize(_config, _configFilePath);
            _logger.Info("end");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetTwitterAccessTokenSecret()
        {
            _logger.Info("start");
            string ret = string.Empty;
            if (!string.IsNullOrEmpty(_config.TwitterAccessTokenSecret))
            {
                try
                {
                    ret = Unprotect(_config.TwitterAccessTokenSecret);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            }
            _logger.Info("end");
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetTwitterAccessTokenSecret(string value)
        {
            _logger.Info("start");
            string src = value;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    src = Protect(value);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            }
            _config.TwitterAccessTokenSecret = src;
            Serialize(_config, _configFilePath);
            _logger.Info("end");
        }

        /// <summary>
        /// 文字列を暗号化する
        /// </summary>
        /// <param name="str">文字列</param>
        /// <returns>暗号化された文字列</returns>
        private static string Protect(string str) => Convert.ToBase64String(
            ProtectedData.Protect(
                Encoding.ASCII.GetBytes(str),
                Encoding.ASCII.GetBytes(_privateGuid.ToString()),
                DataProtectionScope.CurrentUser));

        /// <summary>
        /// 文字列を復号する
        /// </summary>
        /// <param name="str">暗号化された文字列</param>
        /// <returns>復号後の文字列</returns>
        private static string Unprotect(string str) => Encoding.ASCII.GetString(
            ProtectedData.Unprotect(
                Convert.FromBase64String(str),
                Encoding.ASCII.GetBytes(_privateGuid.ToString()),
                DataProtectionScope.CurrentUser));
        #endregion
    }
}
