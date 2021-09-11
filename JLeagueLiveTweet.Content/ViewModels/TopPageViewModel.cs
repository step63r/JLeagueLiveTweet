using LinqToTwitter;
using log4net;
using MinatoProject.Apps.JLeagueLiveTweet.Content.Extensions;
using MinatoProject.Apps.JLeagueLiveTweet.Content.Models;
using MinatoProject.Apps.JLeagueLiveTweet.Core.Models;
using MinatoProject.Apps.JLeagueLiveTweet.Core.Services;
using Prism.Commands;
using Prism.Events;
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
        /// <summary>
        /// IEventAggregator
        /// </summary>
        private readonly IEventAggregator _eventAggregator = null;
        #endregion

        #region メンバ変数
        /// <summary>
        /// ロガー
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// クラブ情報をストアするインスタンス
        /// </summary>
        private readonly ClubsStore _clubsStore = ClubsStore.GetInstance();
        /// <summary>
        /// 選手情報をストアするインスタンス
        /// </summary>
        private readonly PlayersStore _playersStore = PlayersStore.GetInstance();
        /// <summary>
        /// ユーザー設定情報をストアするインスタンス
        /// </summary>
        private readonly ConfigStore _configStore = ConfigStore.GetInstance();
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
        /// IAuthorizer
        /// </summary>
        private IAuthorizer _authorizer;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService">IDialogService</param>
        /// <param name="eventAggregator">IEventAggregator</param>
        public TopPageViewModel(IDialogService dialogService, IEventAggregator eventAggregator)
        {
            _logger.Info("start");
            // インターフェイスを取得
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;

            // 現在時刻用のディスパッチャタイマーを開始
            _currentDispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _currentDispatcherTimer.Tick += OnCurrentDispatcherTimerTicked;
            _currentDispatcherTimer.Start();

            // 全クラブを取得
            AllClubs = new ObservableCollection<Club>(_clubsStore.GetClubs());

            // 自クラブの情報を取得してUIを一括更新
            UpdateMyClub(MyClub);

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

            // イベントの登録
            _ = _eventAggregator.GetEvent<PubSubEvent<Club>>().Subscribe(UpdateMyClub);
            _ = _eventAggregator.GetEvent<PubSubEvent<IList<Player>>>().Subscribe(UpdateAllPlayers);
            _logger.Info("end");
        }

        /// <summary>
        /// ファイナライザ
        /// </summary>
        ~TopPageViewModel()
        {
            _logger.Info("start");
            if (_currentDispatcherTimer.IsEnabled)
            {
                _currentDispatcherTimer.Stop();
            }
            _currentDispatcherTimer.Tick -= OnCurrentDispatcherTimerTicked;
            _logger.Info("end");
        }

        /// <summary>
        /// 試合開始/前半終了/後半開始/試合終了コマンドを実行する
        /// </summary>
        private void ExecuteQuarterTimerCommand()
        {
            _logger.Info("start");
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
            _logger.Info("end");
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
                    _logger.Error($"The value of {nameof(GameProgress)} is invalid");
                    return false;
            }
        }

        /// <summary>
        /// 得点コマンドを実行する
        /// </summary>
        private void ExecuteGetScoreCommand()
        {
            _logger.Info("start");
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
                    _logger.Error($"The value of {nameof(GameProgress)} is invalid");
                    return;
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
                    _logger.Error($"The value of {nameof(GameProgress)} is invalid");
                    return;
                }

                RaisePropertyChanged(nameof(ScoreBoard.HomeTotalScore));
            }

            TweetContent = CreateTweetString(TweetType.GetScore);
            _logger.Info("end");
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
            _logger.Info("start");
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
                    _logger.Error($"The value of {nameof(GameProgress)} is invalid");
                    return;
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
                    _logger.Error($"The value of {nameof(GameProgress)} is invalid");
                    return;
                }

                RaisePropertyChanged(nameof(ScoreBoard.AwayTotalScore));
            }

            TweetContent = CreateTweetString(TweetType.LostScore);
            _logger.Info("end");
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
            _logger.Info("start");
            _authorizer = await AuthorizeTwitter();

            if (_authorizer == null)
            {
                _logger.Error("Failed to authorize Twitter account");
                _logger.Info("  ==> Clear current access token and secret. Try again.");
                _configStore.SetTwitterAccessToken(string.Empty);
                _configStore.SetTwitterAccessTokenSecret(string.Empty);

                _eventAggregator.GetEvent<PubSubEvent<string>>().Publish("アカウント認証に失敗しました");
                return;
            }

            if (_authorizer is PinAuthorizer pinAuth)
            {
                _logger.Info("Success to pin authorize Twitter account");
                _logger.Info(" ==> Save access token and secret.");
                _configStore.SetTwitterAccessToken(pinAuth.CredentialStore.OAuthToken);
                _configStore.SetTwitterAccessTokenSecret(pinAuth.CredentialStore.OAuthTokenSecret);
            }

            var context = new TwitterContext(_authorizer);
            var response = await context.TweetAsync(TweetContent);
            if (response == null)
            {
                _logger.Error("Failed to tweet");
                _eventAggregator.GetEvent<PubSubEvent<string>>().Publish("ツイートに失敗しました");
                return;
            }

            TweetContent = string.Empty;
            _eventAggregator.GetEvent<PubSubEvent<string>>().Publish("ツイートしました");
            _logger.Info("end");
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
            _logger.Info("start");
            string ret = string.Empty;
            switch (tweetType)
            {
                case TweetType.Status:
                    ret = $"{ScoreBoard.HomeClub.Abbreviation} {ScoreBoard.HomeTotalScore} {QuarterTimerCommandContent} {ScoreBoard.AwayTotalScore} {ScoreBoard.AwayClub.Abbreviation} {MyClub.HashTag}";
                    break;

                case TweetType.GetScore:
                    ret = $"{ScoreBoard.HomeClub.Abbreviation} {ScoreBoard.HomeTotalScore} - {ScoreBoard.AwayTotalScore} {ScoreBoard.AwayClub.Abbreviation}" + Environment.NewLine +
                        $"{(int)QuarterTime.TotalMinutes + 1}分 {SelectedPlayer.Name} {MyClub.HashTag}";
                    break;

                case TweetType.LostScore:
                    ret = $"{ScoreBoard.HomeClub.Abbreviation} {ScoreBoard.HomeTotalScore} - {ScoreBoard.AwayTotalScore} {ScoreBoard.AwayClub.Abbreviation} {MyClub.HashTag}";
                    break;
                default:
                    _logger.Error($"The value of {nameof(tweetType)} is invalid");
                    break;
            }
            _logger.Info($"end [{ret}]");
            return ret;
        }

        /// <summary>
        /// Twitterのアカウント認証をする
        /// </summary>
        /// <returns></returns>
        private async Task<IAuthorizer> AuthorizeTwitter()
        {
            _logger.Info("start");
            IAuthorizer auth;

            var dispatcher = Dispatcher.CurrentDispatcher;

            string accessToken = _configStore.GetTwitterAccessToken();
            string accessTokenSecret = _configStore.GetTwitterAccessTokenSecret();

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(accessTokenSecret))
            {
                auth = new PinAuthorizer()
                {
                    CredentialStore = new InMemoryCredentialStore
                    {
                        ConsumerKey = _configStore.GetTwitterApiKey(),
                        ConsumerSecret = _configStore.GetTwitterApiSecret()
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
            }
            else
            {
                auth = new SingleUserAuthorizer()
                {
                    CredentialStore = new SingleUserInMemoryCredentialStore()
                    {
                        ConsumerKey = _configStore.GetTwitterApiKey(),
                        ConsumerSecret = _configStore.GetTwitterApiSecret(),
                        AccessToken = accessToken,
                        AccessTokenSecret = accessTokenSecret
                    }
                };
            }

            try
            {
                await auth.AuthorizeAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

            var context = new TwitterContext(auth);
            _logger.Info("end");
            return context == null ? default : context.Authorizer;
        }

        /// <summary>
        /// 他のViewModelからマイクラブ更新を受け取ったときのイベント
        /// </summary>
        /// <param name="club">クラブ</param>
        private void UpdateMyClub(Club club)
        {
            _logger.Info("start");
            // 自クラブを取得
            MyClub = _configStore.GetConfig().MyClub ?? null;
            if (MyClub != null)
            {
                ScoreBoard.HomeClub = MyClub;

                // 全クラブから自クラブと同一ディビジョンのクラブを抽出
                AllClubs = new ObservableCollection<Club>(_clubsStore.GetClubs().Where(p => p.Division == MyClub.Division));

                // 自クラブを除外
                _ = AllClubs.Remove(MyClub);

                // 自クラブの選手一覧を取得
                AllPlayers = new ObservableCollection<Player>(_playersStore.GetPlayers(MyClub))
                {
                    // オウンゴールの人を作っておく
                    new Player { Club = null, Number = -1, Name = "オウンゴール", Position = default }
                };
            }
            _logger.Info("end");
        }

        /// <summary>
        /// 他のViewModelからマイクラブの選手情報更新を受け取ったときのイベント
        /// </summary>
        /// <param name="players">選手情報</param>
        private void UpdateAllPlayers(IList<Player> players)
        {
            _logger.Info("start");
            // 自クラブの選手一覧を取得
            AllPlayers = new ObservableCollection<Player>(_playersStore.GetPlayers(MyClub))
            {
                // オウンゴールの人を作っておく
                new Player { Club = null, Number = -1, Name = "オウンゴール", Position = default }
            };
            _logger.Info("end");
        }
    }
}
