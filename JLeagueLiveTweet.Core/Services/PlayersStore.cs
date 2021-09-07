using MinatoProject.Apps.JLeagueLiveTweet.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace MinatoProject.Apps.JLeagueLiveTweet.Core.Services
{
    /// <summary>
    /// 選手情報をストアするクラス
    /// </summary>
    public class PlayersStore : ServiceStoreBase
    {
        #region Singleton
        /// <summary>
        /// シングルトン インスタンス
        /// </summary>
        private static readonly PlayersStore _instance = new PlayersStore();

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
        /// クラブ情報をストアするインスタンス
        /// </summary>
        private readonly ClubsStore _clubsStore = ClubsStore.GetInstance();
        /// <summary>
        /// 選手情報
        /// </summary>
        private readonly IDictionary<string, IList<Player>> _players = new Dictionary<string, IList<Player>>();
        /// <summary>
        /// 選手情報ファイルパス
        /// </summary>
        private static readonly string _playersFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\JLeagueLiveTweet";
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
        public override void InitializeInstance()
        {
            foreach (var club in _clubsStore.GetClubs())
            {
                string filePath = GetClubFileName(club);

                // ファイルが存在しない場合、新規作成
                if (!File.Exists(filePath))
                {
                    Serialize(new List<Player>(), filePath);
                }
                _players[club.Name] = Deserialize<List<Player>>(filePath);
            }
        }

        /// <summary>
        /// 指定したクラブの選手情報一覧を取得する
        /// </summary>
        /// <param name="club">クラブ</param>
        /// <returns>選手情報</returns>
        public IList<Player> GetPlayers(Club club)
        {
            return _players[club.Name];
        }

        /// <summary>
        /// 指定したクラブの選手情報一覧を更新する
        /// </summary>
        /// <param name="club">クラブ</param>
        /// <param name="players">選手情報一覧</param>
        /// <returns>更新後の選手情報一覧</returns>
        public IList<Player> SetPlayers(Club club, IList<Player> players)
        {
            _players[club.Name] = players;
            Serialize(_players[club.Name] as List<Player>, GetClubFileName(club));
            return _players[club.Name];
        }

        /// <summary>
        /// 指定したクラブに選手情報を追加する
        /// </summary>
        /// <param name="club">クラブ</param>
        /// <param name="player">選手情報</param>
        /// <returns>追加後の選手情報一覧</returns>
        public IList<Player> AddPlayer(Club club, Player player)
        {
            _players[club.Name].Add(player);
            Serialize(_players[club.Name] as List<Player>, GetClubFileName(club));
            return _players[club.Name];
        }

        /// <summary>
        /// 指定したクラブから選手情報を削除する
        /// </summary>
        /// <param name="club">クラブ</param>
        /// <param name="player">選手情報</param>
        /// <returns>削除後の選手情報一覧</returns>
        public IList<Player> RemovePlayer(Club club, Player player)
        {
            _ = _players[club.Name].Remove(player);
            Serialize(_players[club.Name] as List<Player>, GetClubFileName(club));
            return _players[club.Name];
        }

        /// <summary>
        /// 指定したクラブの選手情報一覧ファイル名を取得する
        /// </summary>
        /// <param name="club">クラブ</param>
        /// <returns>ファイル名</returns>
        private string GetClubFileName(Club club)
        {
            return Path.Combine(_playersFilePath, $"{club.Name}.xml");
        }
        #endregion
    }
}
