using LinqToTwitter;
using log4net;
using MaterialDesignColors;
using MinatoProject.Apps.JLeagueLiveTweet.Content.Extensions;
using MinatoProject.Apps.JLeagueLiveTweet.Content.Models;
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
            set => _ = SetProperty(ref _myClub, value);
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
            set => _ = SetProperty(ref _selectedClub, value);
        }

        private bool _isMyClubAway = false;
        /// <summary>
        /// 自クラブがアウェイか
        /// </summary>
        public bool IsMyClubAway
        {
            get => _isMyClubAway;
            set => _ = SetProperty(ref _isMyClubAway, value);
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

        private string _homeClubName;
        /// <summary>
        /// ホームクラブ名
        /// </summary>
        public string HomeClubName
        {
            get => _homeClubName;
            set => _ = SetProperty(ref _homeClubName, value);
        }

        private string _awayClubName;
        /// <summary>
        /// アウェイクラブ名
        /// </summary>
        public string AwayClubName
        {
            get => _awayClubName;
            set => _ = SetProperty(ref _awayClubName, value);
        }

        private int _home1stScore;
        /// <summary>
        /// ホーム前半得点
        /// </summary>
        public int Home1stScore
        {
            get => _home1stScore;
            set => _ = SetProperty(ref _home1stScore, value);
        }

        private int _away1stScore;
        /// <summary>
        /// アウェイ前半得点
        /// </summary>
        public int Away1stScore
        {
            get => _away1stScore;
            set => _ = SetProperty(ref _away1stScore, value);
        }

        private int _home2ndScore;
        /// <summary>
        /// ホーム後半得点
        /// </summary>
        public int Home2ndScore
        {
            get => _home2ndScore;
            set => _ = SetProperty(ref _home2ndScore, value);
        }

        private int _away2ndScore;
        /// <summary>
        /// アウェイ後半得点
        /// </summary>
        public int Away2ndScore
        {
            get => _away2ndScore;
            set => _ = SetProperty(ref _away2ndScore, value);
        }

        private bool _secondHalfVisibility = false;
        /// <summary>
        /// 後半のスコアを表示するか
        /// </summary>
        public bool SecondHalfVisibility
        {
            get => _secondHalfVisibility;
            set => _ = SetProperty(ref _secondHalfVisibility, value);
        }

        /// <summary>
        /// ホーム得点
        /// </summary>
        public int HomeTotalScore => _home1stScore + _home2ndScore;

        /// <summary>
        /// アウェイ得点
        /// </summary>
        public int AwayTotalScore => _away1stScore + _away2ndScore;

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
                SetProperty(ref _gameProgress, value);
                RaisePropertyChanged(nameof(InGameProgress));
                RaisePropertyChanged(nameof(QuarterTimerCommandContent));
            }
        }

        /// <summary>
        /// 試合中フラグ
        /// </summary>
        public bool InGameProgress => GameProgress != GameProgress.Before &&
            GameProgress != GameProgress.HalfTime &&
            GameProgress != GameProgress.After;

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
        /// 対戦相手変更コマンド
        /// </summary>
        public DelegateCommand ClubChangedCommand { get; private set; }
        /// <summary>
        /// ホーム＆アウェイ変更コマンド
        /// </summary>
        public DelegateCommand HomeAwayChangedCommand { get; private set; }
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

        #region メンバ変数
        /// <summary>
        /// ロガー
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 
        /// </summary>
        private static readonly IEnumerable<Prefecture> Prefectures = Prefecture.GetPrefectures();
        /// <summary>
        /// 現在時刻用のディスパッチャタイマー
        /// </summary>
        private DispatcherTimer _currentDispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
        /// <summary>
        /// 45分計用のディスパッチャタイマー
        /// </summary>
        private DispatcherTimer _quarterDispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
        /// <summary>
        /// 45分計の開始時刻
        /// </summary>
        private DateTime _quarterDispatcherTimerStartedAt;
        /// <summary>
        /// 
        /// </summary>
        private PinAuthorizer _pinAuthorizer;
        /// <summary>
        /// 
        /// </summary>
        private IDialogService _dialogService = null;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dialogService"></param>
        public TopPageViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;

            _currentDispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            _currentDispatcherTimer.Tick += OnCurrentDispatcherTimerTicked;
            _currentDispatcherTimer.Start();

            // 匿名関数版
            //_currentDispatcherTimer.Tick += delegate (object sender, EventArgs e)
            //{
            //    CurrentDateTime = DateTime.Now;
            //};

            AllClubs = new ObservableCollection<Club>()
            {
                new Club()
                {
                    Division = Division.J2,
                    Name = "アルビレックス新潟",
                    Abbreviation = "新潟",
                    Prefecture = Prefectures.First(p => p.PrefCode == 15),
                    PrimaryColor = PrimaryColor.DeepOrange,
                    SecondaryColor = SecondaryColor.LightBlue,
                    HashTag = "#albirex"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "ジュビロ磐田",
                    Abbreviation = "磐田",
                    Prefecture = Prefectures.First(p => p.PrefCode == 22),
                    PrimaryColor = PrimaryColor.LightBlue,
                    SecondaryColor = SecondaryColor.LightBlue,
                    HashTag = "#jubilo"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "京都サンガF.C.",
                    Abbreviation = "京都",
                    Prefecture = Prefectures.First(p => p.PrefCode == 26),
                    PrimaryColor = PrimaryColor.Purple,
                    SecondaryColor = SecondaryColor.Purple,
                    HashTag = "#sanga"
                },
            };

            MyClub = AllClubs.First(p => p.Name.Equals("アルビレックス新潟"));

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

            HomeClubName = MyClub.Abbreviation;

            ClubChangedCommand = new DelegateCommand(ExecuteClubChangedCommand, CanExecuteClubChangedCommand);
            ClubChangedCommand.ObservesProperty(() => SelectedClub);
            HomeAwayChangedCommand = new DelegateCommand(ExecuteHomeAwayChangedCommand, CanExecuteHomeAwayChangedCommand);
            QuarterTimerCommand = new DelegateCommand(ExecuteQuarterTimerCommand, CanExecuteQuarterTimerCommand);
            GetScoreCommand = new DelegateCommand(ExecuteGetScoreCommand, CanExecuteGetScoreCommand);
            GetScoreCommand.ObservesProperty(() => SelectedPlayer);
            GetScoreCommand.ObservesProperty(() => InGameProgress);
            LostScoreCommand = new DelegateCommand(ExecuteLostScoreCommand, CanExecuteLostScoreCommand);
            LostScoreCommand.ObservesProperty(() => InGameProgress);
            TweetCommand = new DelegateCommand(ExecuteTweetCommand, CanExecuteTweetCommand);
            TweetCommand.ObservesProperty(() => TweetContent);
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
        /// 
        /// </summary>
        private void ExecuteClubChangedCommand()
        {
            if (IsMyClubAway)
            {
                HomeClubName = SelectedClub.Abbreviation;
            }
            else
            {
                AwayClubName = SelectedClub.Abbreviation;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteClubChangedCommand()
        {
            return SelectedClub != null;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteHomeAwayChangedCommand()
        {
            // ホームとアウェイのスコアボードを入れ替える
            string tempClubName = HomeClubName;
            int temp1stScore = Home1stScore;
            int temp2ndScore = Home2ndScore;
            HomeClubName = AwayClubName;
            Home1stScore = Away1stScore;
            Home2ndScore = Away2ndScore;
            AwayClubName = tempClubName;
            Away1stScore = temp1stScore;
            Away2ndScore = temp2ndScore;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteHomeAwayChangedCommand()
        {
            return true;
        }

        /// <summary>
        /// 
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
                    SecondHalfVisibility = true;
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
                    // 特に処理なし
                    break;

                default:
                    throw new Exception($"The value of {nameof(GameProgress)} is invalid");
            }
            GameProgress = GameProgress.Next();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteQuarterTimerCommand()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteGetScoreCommand()
        {
            // ホーム or アウェイ
            if (IsMyClubAway)
            {
                // 前半 or 後半
                if (GameProgress == GameProgress.FirstHalf)
                {
                    Away1stScore++;
                }
                else if (GameProgress == GameProgress.SecondHalf)
                {
                    Away2ndScore++;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                RaisePropertyChanged(nameof(AwayTotalScore));
            }
            else
            {
                // 前半 or 後半
                if (GameProgress == GameProgress.FirstHalf)
                {
                    Home1stScore++;
                }
                else if (GameProgress == GameProgress.SecondHalf)
                {
                    Home2ndScore++;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                RaisePropertyChanged(nameof(HomeTotalScore));
            }

            TweetContent = CreateTweetString(TweetType.GetScore);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteGetScoreCommand()
        {
            return InGameProgress && SelectedPlayer != null;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExecuteLostScoreCommand()
        {
            // ホーム or アウェイ
            if (IsMyClubAway)
            {
                // 前半 or 後半
                if (GameProgress == GameProgress.FirstHalf)
                {
                    Home1stScore++;
                }
                else if (GameProgress == GameProgress.SecondHalf)
                {
                    Home2ndScore++;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                RaisePropertyChanged(nameof(HomeTotalScore));
            }
            else
            {
                // 前半 or 後半
                if (GameProgress == GameProgress.FirstHalf)
                {
                    Away1stScore++;
                }
                else if (GameProgress == GameProgress.SecondHalf)
                {
                    Away2ndScore++;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                RaisePropertyChanged(nameof(AwayTotalScore));
            }

            TweetContent = CreateTweetString(TweetType.LostScore);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteLostScoreCommand()
        {
            return InGameProgress;
        }

        /// <summary>
        /// 
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
            //_ = await context.TweetAsync(TweetContent);
            _ = await context.TweetAsync("Hello! This is a test tweet from JLeagueLiveTweet!! Yeah!!!");

            TweetContent = string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteTweetCommand()
        {
            return !string.IsNullOrEmpty(TweetContent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCurrentDispatcherTimerTicked(object sender, EventArgs e)
        {
            CurrentDateTime = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnQuarterDispatcherTimerTicked(object sender, EventArgs e)
        {
            QuarterTime = DateTime.Now - _quarterDispatcherTimerStartedAt;
        }

        /// <summary>
        /// ツイート内容を生成する
        /// </summary>
        /// <returns></returns>
        private string CreateTweetString(TweetType tweetType)
        {
            string ret = string.Empty;
            switch (tweetType)
            {
                case TweetType.Status:
                    ret = $"{HomeClubName} {HomeTotalScore} {QuarterTimerCommandContent} {AwayTotalScore} {AwayClubName} {MyClub.HashTag}";
                    break;

                case TweetType.GetScore:
                    ret = $"{HomeClubName} {HomeTotalScore} - {AwayTotalScore} {AwayClubName}" + Environment.NewLine +
                        $"{QuarterTime.Minutes + 1}分 {SelectedPlayer.Name} {MyClub.HashTag}";
                    break;

                case TweetType.LostScore:
                    ret = $"{HomeClubName} {HomeTotalScore} - {AwayTotalScore} {AwayClubName}";
                    break;
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
