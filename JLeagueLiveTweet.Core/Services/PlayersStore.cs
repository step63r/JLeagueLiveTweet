using MinatoProject.Apps.JLeagueLiveTweet.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace MinatoProject.Apps.JLeagueLiveTweet.Core.Services
{
    /// <summary>
    /// 選手情報をストアするクラス
    /// </summary>
    public class PlayersStore
    {
        #region Singleton
        /// <summary>
        /// シングルトン インスタンス
        /// </summary>
        private static PlayersStore _instance = new PlayersStore();

        /// <summary>
        /// インスタンスを取得する
        /// </summary>
        /// <returns></returns>
        public static PlayersStore GetInstance()
        {
            return _instance;
        }
        #endregion

        #region メンバ変数
        /// <summary>
        /// 選手情報
        /// </summary>
        private IList<Player> _players = new List<Player>();
        /// <summary>
        /// 選手情報ファイルパス
        /// </summary>
        private static readonly string _playersFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\JLeagueLiveTweet\players.xml";
        #endregion

        #region コンストラクタ
        private PlayersStore()
        {
        }
        #endregion

        #region メソッド
        /// <summary>
        /// インスタンスを初期化する
        /// </summary>
        public void InitializeInstance()
        {
            // ファイルが存在しない場合、新規作成
            if (!File.Exists(_playersFilePath))
            {
                Serialize(_players as List<Player>, _playersFilePath);
            }
            _players = Deserialize<List<Player>>(_playersFilePath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<Player> GetPlayers()
        {
            return _players;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="club"></param>
        /// <returns></returns>
        public IList<Player> AddPlayer(Player player)
        {
            _players.Add(player);
            return _players;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="club"></param>
        /// <returns></returns>
        public IList<Player> RemovePlayer(Player player)
        {
            _players.Remove(player);
            return _players;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        private static void Serialize<T>(T obj, string filePath) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                serializer.Serialize(sw, obj);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static T Deserialize<T>(string filePath) where T : class
        {
            var ret = default(T);
            var serializer = new XmlSerializer(typeof(T));
            using (var sr = new StreamReader(filePath, Encoding.UTF8))
            {
                ret = serializer.Deserialize(sr) as T;
            }
            return ret;
        }
        #endregion
    }
}
