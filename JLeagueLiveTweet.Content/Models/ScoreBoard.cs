using MinatoProject.Apps.JLeagueLiveTweet.Core.Models;
using Prism.Mvvm;

namespace MinatoProject.Apps.JLeagueLiveTweet.Content.Models
{
    /// <summary>
    /// スコアボード
    /// </summary>
    public class ScoreBoard : BindableBase
    {
        #region プロパティ
        private Club _homeClub;
        /// <summary>
        /// ホームクラブ
        /// </summary>
        public Club HomeClub
        {
            get => _homeClub;
            set => _ = SetProperty(ref _homeClub, value);
        }

        private Club _awayClub;
        /// <summary>
        /// アウェイクラブ
        /// </summary>
        public Club AwayClub
        {
            get => _awayClub;
            set => _ = SetProperty(ref _awayClub, value);
        }

        private int _home1stScore = 0;
        /// <summary>
        /// ホーム前半得点
        /// </summary>
        public int Home1stScore
        {
            get => _home1stScore;
            set
            {
                _ = SetProperty(ref _home1stScore, value);
                RaisePropertyChanged(nameof(HomeTotalScore));
            }
        }

        private int _home2ndScore = 0;
        /// <summary>
        /// ホーム後半得点
        /// </summary>
        public int Home2ndScore
        {
            get => _home2ndScore;
            set
            {
                _ = SetProperty(ref _home2ndScore, value);
                RaisePropertyChanged(nameof(HomeTotalScore));
            }
        }

        /// <summary>
        /// ホーム得点
        /// </summary>
        public int HomeTotalScore => Home1stScore + Home2ndScore;

        private int _away1stScore = 0;
        /// <summary>
        /// アウェイ前半得点
        /// </summary>
        public int Away1stScore
        {
            get => _away1stScore;
            set
            {
                _ = SetProperty(ref _away1stScore, value);
                RaisePropertyChanged(nameof(AwayTotalScore));
            }
        }

        private int _away2ndScore = 0;
        /// <summary>
        /// アウェイ後半得点
        /// </summary>
        public int Away2ndScore
        {
            get => _away2ndScore;
            set
            {
                _ = SetProperty(ref _away2ndScore, value);
                RaisePropertyChanged(nameof(AwayTotalScore));
            }
        }

        /// <summary>
        /// アウェイ得点
        /// </summary>
        public int AwayTotalScore => Away1stScore + Away2ndScore;
        #endregion

        #region コンストラクタ
        public ScoreBoard()
        {

        }
        #endregion

        #region メソッド
        /// <summary>
        /// ホームとアウェイを入れ替える
        /// </summary>
        public void ExchangeHomeAndAway()
        {
            var tempHomeClue = HomeClub;
            int tempHome1stScore = Home1stScore;
            int tempHome2ndScore = Home2ndScore;
            HomeClub = AwayClub;
            Home1stScore = Away1stScore;
            Home2ndScore = Away2ndScore;
            AwayClub = tempHomeClue;
            Away1stScore = tempHome1stScore;
            Away2ndScore = tempHome2ndScore;
        }
        #endregion
    }
}
