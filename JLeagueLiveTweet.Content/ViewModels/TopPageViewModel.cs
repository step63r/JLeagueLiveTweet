using LinqToTwitter;
using log4net;
using MaterialDesignColors;
using MinatoProject.Apps.JLeagueLiveTweet.Content.Extensions;
using MinatoProject.Apps.JLeagueLiveTweet.Content.Models;
using MinatoProject.Apps.JLeagueLiveTweet.Core.Models;
using MinatoProject.Apps.JLeagueLiveTweet.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MinatoProject.Apps.JLeagueLiveTweet.Content.ViewModels
{
    /// <summary>
    /// TopPage.xamlのViewModelクラス
    /// </summary>
    public class TopPageViewModel : BindableBase
    {
        /// <summary>
        /// ツイート種別
        /// </summary>
        private enum TweetType
        {
            /// <summary>
            /// 状況
            /// </summary>
            Status,
            /// <summary>
            /// 得点
            /// </summary>
            GetScore,
            /// <summary>
            /// 失点
            /// </summary>
            LostScore
        };

        #region プロパティ
        private Club _myClub;
        /// <summary>
        /// 自クラブ
        /// </summary>
        public Club MyClub
        {
            get => _myClub;
            set
            {
                _ = SetProperty(ref _myClub, value);
                RaisePropertyChanged(nameof(ScoreBoard));
            }
        }

        private ObservableCollection<Club> _allClubs = new ObservableCollection<Club>();
        /// <summary>
        /// 全クラブ
        /// </summary>
        public ObservableCollection<Club> AllClubs
        {
            get => _allClubs;
            set => _ = SetProperty(ref _allClubs, value);
        }

        private Club _selectedClub;
        /// <summary>
        /// 選択されたクラブ
        /// </summary>
        public Club SelectedClub
        {
            get => _selectedClub;
            set
            {
                _ = SetProperty(ref _selectedClub, value);
                if (IsMyClubAway)
                {
                    ScoreBoard.HomeClub = SelectedClub;
                }
                else
                {
                    ScoreBoard.AwayClub = SelectedClub;
                }
            }
        }

        private bool _isMyClubAway = false;
        /// <summary>
        /// 自クラブがアウェイか
        /// </summary>
        public bool IsMyClubAway
        {
            get => _isMyClubAway;
            set
            {
                _ = SetProperty(ref _isMyClubAway, value);
                ScoreBoard.ExchangeHomeAndAway();
            }
        }

        private ObservableCollection<Player> _allPlayers = new ObservableCollection<Player>();
        /// <summary>
        /// 自クラブの選手一覧
        /// </summary>
        public ObservableCollection<Player> AllPlayers
        {
            get => _allPlayers;
            set => _ = SetProperty(ref _allPlayers, value);
        }

        private Player _selectedPlayer;
        /// <summary>
        /// 選択された選手
        /// </summary>
        public Player SelectedPlayer
        {
            get => _selectedPlayer;
            set => _ = SetProperty(ref _selectedPlayer, value);
        }

        private ScoreBoard _scoreBoard = new ScoreBoard();
        /// <summary>
        /// スコアボード
        /// </summary>
        public ScoreBoard ScoreBoard
        {
            get => _scoreBoard;
            set => _ = SetProperty(ref _scoreBoard, value);
        }

        private DateTime _currentDateTime = DateTime.Now;
        /// <summary>
        /// 現在時刻
        /// </summary>
        public DateTime CurrentDateTime
        {
            get => _currentDateTime;
            set => _ = SetProperty(ref _currentDateTime, value);
        }

        private TimeSpan _quarterTime = new TimeSpan();
        /// <summary>
        /// 45分計
        /// </summary>
        public TimeSpan QuarterTime
        {
            get => _quarterTime;
            set => _ = SetProperty(ref _quarterTime, value);
        }

        /// <summary>
        /// 試合開始/前半終了/後半開始/試合終了コマンドの表示文字列
        /// </summary>
        public string QuarterTimerCommandContent
        {
            get
            {
                switch (GameProgress)
                {
                    case GameProgress.Before:
                        return "試合開始";
                    case GameProgress.FirstHalf:
                        return "前半終了";
                    case GameProgress.HalfTime:
                        return "後半開始";
                    case GameProgress.SecondHalf:
                        return "試合終了";
                    case GameProgress.After:
                        return "リセット";
                    default:
                        throw new Exception($"The value of {nameof(GameProgress)} is invalid");
                }
            }
        }

        private GameProgress _gameProgress = GameProgress.Before;
        /// <summary>
        /// 試合経過
        /// </summary>
        public GameProgress GameProgress
        {
            get => _gameProgress;
            set
            {
                _ = SetProperty(ref _gameProgress, value);
                RaisePropertyChanged(nameof(InGameProgress));
                RaisePropertyChanged(nameof(SecondHalfVisibility));
                RaisePropertyChanged(nameof(QuarterTimerCommandContent));
            }
        }

        /// <summary>
        /// 試合中フラグ
        /// </summary>
        public bool InGameProgress => GameProgress != GameProgress.Before &&
            GameProgress != GameProgress.HalfTime &&
            GameProgress != GameProgress.After;

        /// <summary>
        /// 後半のスコアを表示するか
        /// </summary>
        public bool SecondHalfVisibility => GameProgress == GameProgress.SecondHalf || GameProgress == GameProgress.After;

        private string _tweetContent = string.Empty;
        /// <summary>
        /// ツイート内容
        /// </summary>
        public string TweetContent
        {
            get => _tweetContent;
            set => _ = SetProperty(ref _tweetContent, value);
        }
        #endregion

        #region コマンド
        /// <summary>
        /// 試合開始/前半終了/後半開始/試合終了コマンド
        /// </summary>
        public DelegateCommand QuarterTimerCommand { get; private set; }
        /// <summary>
        /// 得点コマンド
        /// </summary>
        public DelegateCommand GetScoreCommand { get; private set; }
        /// <summary>
        /// 失点コマンド
        /// </summary>
        public DelegateCommand LostScoreCommand { get; private set; }
        /// <summary>
        /// ツイートコマンド
        /// </summary>
        public DelegateCommand TweetCommand { get; private set; }
        #endregion


        #region インターフェイス
        /// <summary>
        /// IDialogService
        /// </summary>
        private readonly IDialogService _dialogService = null;
        #endregion

        #region メンバ変数
        /// <summary>
        /// ロガー
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// クラブ情報をストアするインスタンス
        /// </summary>
        private ClubsStore _clubsStore = null;
        /// <summary>
        /// 現在時刻用のディスパッチャタイマー
        /// </summary>
        private readonly DispatcherTimer _currentDispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
        /// <summary>
        /// 45分計用のディスパッチャタイマー
        /// </summary>
        private readonly DispatcherTimer _quarterDispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
        /// <summary>
        /// 45分計の開始時刻
        /// </summary>
        private DateTime _quarterDispatcherTimerStartedAt;
        /// <summary>
        /// PinAuthorizer
        /// </summary>
        private PinAuthorizer _pinAuthorizer;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService"></param>
        public TopPageViewModel(IDialogService dialogService)
        {
            // インターフェイスを取得
            _dialogService = dialogService;

            // 現在時刻用のディスパッチャタイマーを開始
            _currentDispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _currentDispatcherTimer.Tick += OnCurrentDispatcherTimerTicked;
            _currentDispatcherTimer.Start();

            _clubsStore = ClubsStore.GetInstance();

            // 全クラブを取得
            AllClubs = new ObservableCollection<Club>(_clubsStore.GetClubs());

            // TODO: 自クラブを取得
            MyClub = AllClubs.First(p => p.Name.Equals("アルビレックス新潟"));
            ScoreBoard.HomeClub = MyClub;

            // 全クラブから自クラブを除外
            _ = AllClubs.Remove(MyClub);

            // 全クラブから自クラブと同一ディビジョンのクラブを抽出
            AllClubs = new ObservableCollection<Club>(AllClubs.Where(p => p.Division == MyClub.Division));

            // TODO: 自クラブの選手一覧を取得
            AllPlayers = new ObservableCollection<Player>()
            {
                new Player() { Club = MyClub, Number = 1, Name = "小島 亨介", Position = Position.GK },
                new Player() { Club = MyClub, Number = 21, Name = "阿部 航斗", Position = Position.GK },
                new Player() { Club = MyClub, Number = 22, Name = "瀬口 拓弥", Position = Position.GK },
                new Player() { Club = MyClub, Number = 41, Name = "藤田 和輝", Position = Position.GK },
                new Player() { Club = MyClub, Number = 5, Name = "舞行龍ジェームズ", Position = Position.DF },
                new Player() { Club = MyClub, Number = 19, Name = "星 雄次", Position = Position.DF },
                new Player() { Club = MyClub, Number = 26, Name = "遠藤 凌", Position = Position.DF },
                new Player() { Club = MyClub, Number = 28, Name = "早川 史哉", Position = Position.DF },
                new Player() { Club = MyClub, Number = 30, Name = "丸山 嵩大", Position = Position.DF },
                new Player() { Club = MyClub, Number = 31, Name = "堀米 悠斗", Position = Position.DF },
                new Player() { Club = MyClub, Number = 32, Name = "長谷川 巧", Position = Position.DF },
                new Player() { Club = MyClub, Number = 35, Name = "千葉 和彦", Position = Position.DF },
                new Player() { Club = MyClub, Number = 50, Name = "田上 大地", Position = Position.DF },
                new Player() { Club = MyClub, Number = 8, Name = "高 宇洋", Position = Position.MF },
                new Player() { Club = MyClub, Number = 10, Name = "本間 至恩", Position = Position.MF },
                new Player() { Club = MyClub, Number = 16, Name = "ゴンサロ ゴンザレス", Position = Position.MF },
                new Player() { Club = MyClub, Number = 17, Name = "福田 晃斗", Position = Position.MF },
                new Player() { Club = MyClub, Number = 20, Name = "島田 譲", Position = Position.MF },
                new Player() { Club = MyClub, Number = 24, Name = "ロメロ フランク", Position = Position.MF },
                new Player() { Club = MyClub, Number = 25, Name = "藤原 奏哉", Position = Position.MF },
                new Player() { Club = MyClub, Number = 27, Name = "大本 祐槻", Position = Position.MF },
                new Player() { Club = MyClub, Number = 33, Name = "高木 善朗", Position = Position.MF },
                new Player() { Club = MyClub, Number = 37, Name = "三戸 舜介", Position = Position.MF },
                new Player() { Club = MyClub, Number = 40, Name = "シマブク カズヨシ", Position = Position.MF },
                new Player() { Club = MyClub, Number = 7, Name = "谷口 海斗", Position = Position.FW },
                new Player() { Club = MyClub, Number = 9, Name = "鈴木 孝司", Position = Position.FW },
                new Player() { Club = MyClub, Number = 11, Name = "髙澤 優也", Position = Position.FW },
                new Player() { Club = MyClub, Number = 14, Name = "田中 達也", Position = Position.FW },
                new Player() { Club = MyClub, Number = 23, Name = "小見 洋太", Position = Position.FW },
                new Player() { Club = MyClub, Number = 39, Name = "矢村 健", Position = Position.FW },
            };

            // オウンゴールの人を作っておく
            AllPlayers.Add(new Player { Club = null, Number = -1, Name = "オウンゴール", Position = default });

            // コマンドの登録
            QuarterTimerCommand = new DelegateCommand(ExecuteQuarterTimerCommand, CanExecuteQuarterTimerCommand)
                .ObservesProperty(() => InGameProgress)
                .ObservesProperty(() => SelectedClub);
            GetScoreCommand = new DelegateCommand(ExecuteGetScoreCommand, CanExecuteGetScoreCommand)
                .ObservesProperty(() => SelectedPlayer)
                .ObservesProperty(() => InGameProgress);
            LostScoreCommand = new DelegateCommand(ExecuteLostScoreCommand, CanExecuteLostScoreCommand)
                .ObservesProperty(() => InGameProgress);
            TweetCommand = new DelegateCommand(ExecuteTweetCommand, CanExecuteTweetCommand)
                .ObservesProperty(() => TweetContent);
        }

        /// <summary>
        /// ファイナライザ
        /// </summary>
        ~TopPageViewModel()
        {
            if (_currentDispatcherTimer.IsEnabled)
            {
                _currentDispatcherTimer.Stop();
            }
            _currentDispatcherTimer.Tick -= OnCurrentDispatcherTimerTicked;
        }

        /// <summary>
        /// 試合開始/前半終了/後半開始/試合終了コマンドを実行する
        /// </summary>
        private void ExecuteQuarterTimerCommand()
        {
            switch (GameProgress)
            {
                case GameProgress.Before:
                    _quarterDispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                    _quarterDispatcherTimer.Tick += OnQuarterDispatcherTimerTicked;
                    _quarterDispatcherTimerStartedAt = DateTime.Now;
                    _quarterDispatcherTimer.Start();
                    TweetContent = CreateTweetString(TweetType.Status);
                    break;

                case GameProgress.FirstHalf:
                    _quarterDispatcherTimer.Stop();
                    QuarterTime = new TimeSpan(0, 45, 0);
                    TweetContent = CreateTweetString(TweetType.Status);
                    break;

                case GameProgress.HalfTime:
                    _quarterDispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                    _quarterDispatcherTimer.Tick += OnQuarterDispatcherTimerTicked;
                    _quarterDispatcherTimerStartedAt = DateTime.Now;
                    _quarterDispatcherTimer.Start();
                    TweetContent = CreateTweetString(TweetType.Status);
                    break;

                case GameProgress.SecondHalf:
                    _quarterDispatcherTimer.Stop();
                    QuarterTime = new TimeSpan(0, 0, 0);
                    TweetContent = CreateTweetString(TweetType.Status);
                    break;

                case GameProgress.After:
                    SelectedClub = null;
                    IsMyClubAway = false;
                    ScoreBoard = new ScoreBoard()
                    {
                        HomeClub = MyClub
                    };
                    TweetContent = string.Empty;
                    break;

                default:
                    throw new Exception($"The value of {nameof(GameProgress)} is invalid");
            }
            GameProgress = GameProgress.Next();
        }
        /// <summary>
        /// 試合開始/前半終了/後半開始/試合終了コマンドが実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteQuarterTimerCommand()
        {
            switch (GameProgress)
            {
                case GameProgress.Before:
                    return !InGameProgress && SelectedClub != null;

                case GameProgress.FirstHalf:
                    return InGameProgress;

                case GameProgress.HalfTime:
                    return !InGameProgress && SelectedClub != null;

                case GameProgress.SecondHalf:
                    return InGameProgress;

                case GameProgress.After:
                    return !InGameProgress && SelectedClub != null;

                default:
                    throw new Exception($"The value of {nameof(GameProgress)} is invalid");
            }
        }

        /// <summary>
        /// 得点コマンドを実行する
        /// </summary>
        private void ExecuteGetScoreCommand()
        {
            // ホーム or アウェイ
            if (IsMyClubAway)
            {
                // 前半 or 後半
                if (GameProgress == GameProgress.FirstHalf)
                {
                    ScoreBoard.Away1stScore++;
                }
                else if (GameProgress == GameProgress.SecondHalf)
                {
                    ScoreBoard.Away2ndScore++;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                RaisePropertyChanged(nameof(ScoreBoard.AwayTotalScore));
            }
            else
            {
                // 前半 or 後半
                if (GameProgress == GameProgress.FirstHalf)
                {
                    ScoreBoard.Home1stScore++;
                }
                else if (GameProgress == GameProgress.SecondHalf)
                {
                    ScoreBoard.Home2ndScore++;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                RaisePropertyChanged(nameof(ScoreBoard.HomeTotalScore));
            }

            TweetContent = CreateTweetString(TweetType.GetScore);
        }
        /// <summary>
        /// 得点コマンドが実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteGetScoreCommand()
        {
            return InGameProgress && SelectedPlayer != null;
        }

        /// <summary>
        /// 失点コマンドを実行する
        /// </summary>
        private void ExecuteLostScoreCommand()
        {
            // ホーム or アウェイ
            if (IsMyClubAway)
            {
                // 前半 or 後半
                if (GameProgress == GameProgress.FirstHalf)
                {
                    ScoreBoard.Home1stScore++;
                }
                else if (GameProgress == GameProgress.SecondHalf)
                {
                    ScoreBoard.Home2ndScore++;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                RaisePropertyChanged(nameof(ScoreBoard.HomeTotalScore));
            }
            else
            {
                // 前半 or 後半
                if (GameProgress == GameProgress.FirstHalf)
                {
                    ScoreBoard.Away1stScore++;
                }
                else if (GameProgress == GameProgress.SecondHalf)
                {
                    ScoreBoard.Away2ndScore++;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                RaisePropertyChanged(nameof(ScoreBoard.AwayTotalScore));
            }

            TweetContent = CreateTweetString(TweetType.LostScore);
        }
        /// <summary>
        /// 失点コマンドが実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteLostScoreCommand()
        {
            return InGameProgress;
        }

        /// <summary>
        /// ツイートコマンドを実行する
        /// </summary>
        private async void ExecuteTweetCommand()
        {
            if (_pinAuthorizer == null)
            {
                _pinAuthorizer = await AuthorizeTwitter();
            }

            if (_pinAuthorizer == null)
            {
                return;
            }

            var context = new TwitterContext(_pinAuthorizer);
            _ = await context.TweetAsync(TweetContent);

            TweetContent = string.Empty;
        }
        /// <summary>
        /// ツイートコマンドが実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteTweetCommand()
        {
            return !string.IsNullOrEmpty(TweetContent);
        }

        /// <summary>
        /// 現在時刻用のディスパッチャタイマーのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCurrentDispatcherTimerTicked(object sender, EventArgs e)
        {
            CurrentDateTime = DateTime.Now;
        }

        /// <summary>
        /// 45分計用のディスパッチャタイマーのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnQuarterDispatcherTimerTicked(object sender, EventArgs e)
        {
            var addMinutes = GameProgress == GameProgress.SecondHalf ? new TimeSpan(0, 45, 0) : new TimeSpan();
            QuarterTime = DateTime.Now - _quarterDispatcherTimerStartedAt + addMinutes;
        }

        /// <summary>
        /// ツイート内容を生成する
        /// </summary>
        /// <returns></returns>
        private string CreateTweetString(TweetType tweetType)
        {
            string ret;
            switch (tweetType)
            {
                case TweetType.Status:
                    ret = $"{ScoreBoard.HomeClub.Abbreviation} {ScoreBoard.HomeTotalScore} {QuarterTimerCommandContent} {ScoreBoard.AwayTotalScore} {ScoreBoard.AwayClub.Abbreviation} {MyClub.HashTag}";
                    break;

                case TweetType.GetScore:
                    ret = $"{ScoreBoard.HomeClub.Abbreviation} {ScoreBoard.HomeTotalScore} - {ScoreBoard.AwayTotalScore} {ScoreBoard.AwayClub.Abbreviation}" + Environment.NewLine +
                        $"{QuarterTime.Minutes + 1}分 {SelectedPlayer.Name} {MyClub.HashTag}";
                    break;

                case TweetType.LostScore:
                    ret = $"{ScoreBoard.HomeClub.Abbreviation} {ScoreBoard.HomeTotalScore} - {ScoreBoard.AwayTotalScore} {ScoreBoard.AwayClub.Abbreviation}";
                    break;
                default:
                    throw new Exception($"The value of {nameof(tweetType)} is invalid");
            }
            return ret;
        }

        /// <summary>
        /// Twitterのアカウント認証をする
        /// </summary>
        /// <returns></returns>
        private async Task<PinAuthorizer> AuthorizeTwitter()
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            var auth = new PinAuthorizer()
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = Properties.Settings.Default.TwitterApiKey,
                    ConsumerSecret = Properties.Settings.Default.TwitterApiSecret
                },
                GoToTwitterAuthorization = pageLink => Process.Start(pageLink),
                GetPin = () =>
                {
                    var ret = new DialogResult();
                    dispatcher.Invoke(() =>
                    {
                        _dialogService?.ShowDialog("InputPinCodeDialogView", null, r => ret = r as DialogResult);
                    });
                    return ret.Result == ButtonResult.OK ? ret.Parameters.GetValue<string>("PinCode") : string.Empty;
                }
            };

            await auth.AuthorizeAsync();

            var context = new TwitterContext(auth);
            return context == null ? default : context.Authorizer as PinAuthorizer;
        }
    }
}
