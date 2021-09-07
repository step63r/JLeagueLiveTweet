using MinatoProject.Apps.JLeagueLiveTweet.Core.Models;
using System;
using System.IO;

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
            // ファイルが存在しない場合、新規作成
            if (!File.Exists(_configFilePath))
            {
                Serialize(_config, _configFilePath);
            }
            _config = Deserialize<Config>(_configFilePath);
        }

        /// <summary>
        /// ユーザー設定情報を取得する
        /// </summary>
        /// <returns>ユーザー設定情報</returns>
        public Config GetConfig()
        {
            return _config;
        }

        /// <summary>
        /// ユーザー設定情報を更新する
        /// </summary>
        /// <param name="config">ユーザー設定情報</param>
        /// <returns>更新後のユーザー設定情報</returns>
        public Config SetConfig(Config config)
        {
            _config = config;
            Serialize(_config, _configFilePath);
            return _config;
        }

        /// <summary>
        /// マイクラブを更新する
        /// </summary>
        /// <param name="club">クラブ</param>
        /// <returns>更新後のユーザー設定情報</returns>
        public Config SetMyClub(Club club)
        {
            _config.MyClub = club;
            Serialize(_config, _configFilePath);
            return _config;
        }
        #endregion
    }
}
