using log4net;
using MaterialDesignThemes.Wpf;
using MinatoProject.Apps.JLeagueLiveTweet.Core.Models;
using MinatoProject.Apps.JLeagueLiveTweet.Core.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Reflection;

namespace MinatoProject.Apps.JLeagueLiveTweet.ViewModels
{
    /// <summary>
    /// MainWindow.xamlのViewModelクラス
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        #region プロパティ
        private string _title = "J League Live Tweet";
        /// <summary>
        /// アプリケーション タイトル
        /// </summary>
        public string Title
        {
            get => _title;
            set => _ = SetProperty(ref _title, value);
        }

        private bool _isMenuOpened = false;
        /// <summary>
        /// メニューが開かれているか
        /// </summary>
        public bool IsMenuOpened
        {
            get => _isMenuOpened;
            set => _ = SetProperty(ref _isMenuOpened, value);
        }

        private string _myClubName = string.Empty;
        /// <summary>
        /// マイクラブの名前
        /// </summary>
        public string MyClubName
        {
            get => _myClubName;
            set => _ = SetProperty(ref _myClubName, value);
        }

        private string _myClubDivision = string.Empty;
        /// <summary>
        /// マイクラブの所属ディビジョン
        /// </summary>
        public string MyClubDivision
        {
            get => _myClubDivision;
            set => _ = SetProperty(ref _myClubDivision, value);
        }

        private SnackbarMessageQueue _messageQueue = new SnackbarMessageQueue();
        /// <summary>
        /// スナックバーに表示するメッセージのキュー
        /// </summary>
        public SnackbarMessageQueue MessageQueue
        {
            get => _messageQueue;
            set => _ = SetProperty(ref _messageQueue, value);
        }
        #endregion

        #region コマンド
        /// <summary>
        /// スコアボードコマンド
        /// </summary>
        public DelegateCommand ScoreBoardCommand { get; private set; }
        /// <summary>
        /// クラブ情報コマンド
        /// </summary>
        public DelegateCommand ClubsCommand { get; private set; }
        /// <summary>
        /// 選手情報コマンド
        /// </summary>
        public DelegateCommand PlayersCommand { get; private set; }
        /// <summary>
        /// 設定コマンド
        /// </summary>
        public DelegateCommand SettingsCommand { get; private set; }
        #endregion

        #region インターフェイス
        /// <summary>
        /// IRegionManager
        /// </summary>
        private readonly IRegionManager _regionManager = null;
        /// <summary>
        /// IEventAggregator
        /// </summary>
        private readonly IEventAggregator _eventAggregator = null;
        #endregion

        #region メンバ変数
        /// <summary>
        /// ロガー
        /// </summary>
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="regionManager">IRegionManager</param>
        /// <param name="eventAggregator">IEventAggregator</param>
        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _logger.Info("start");
            // インターフェイスの取得
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            // コマンドの登録
            ScoreBoardCommand = new DelegateCommand(ExecuteScoreBoardCommand);
            ClubsCommand = new DelegateCommand(ExecuteClubsCommand);
            PlayersCommand = new DelegateCommand(ExecutePlayersCommand);
            SettingsCommand = new DelegateCommand(ExecuteSettingsCommand);

            // ストアを初期化
            _clubsStore.InitializeInstance();
            _playersStore.InitializeInstance();
            _configStore.InitializeInstance();

            var config = _configStore.GetConfig();
            if (config.MyClub != null)
            {
                RefreshHeader(config.MyClub);
            }

            // イベントの登録
            _ = _eventAggregator.GetEvent<PubSubEvent<Club>>().Subscribe(RefreshHeader);
            _ = _eventAggregator.GetEvent<PubSubEvent<string>>().Subscribe(ShowSnackbar);
            _logger.Info("end");
        }

        /// <summary>
        /// スコアボードコマンドを実行する
        /// </summary>
        private void ExecuteScoreBoardCommand()
        {
            _logger.Info("start");
            _regionManager.RequestNavigate("ContentRegion", "TopPage");
            IsMenuOpened = false;
            _logger.Info("end");
        }

        /// <summary>
        /// クラブ情報コマンドを実行する
        /// </summary>
        private void ExecuteClubsCommand()
        {
            _logger.Info("start");
            _regionManager.RequestNavigate("ContentRegion", "ClubsPage");
            IsMenuOpened = false;
            _logger.Info("end");
        }

        /// <summary>
        /// 選手情報コマンドを実行する
        /// </summary>
        private void ExecutePlayersCommand()
        {
            _logger.Info("start");
            _regionManager.RequestNavigate("ContentRegion", "PlayersPage");
            IsMenuOpened = false;
            _logger.Info("end");
        }

        /// <summary>
        /// 設定コマンドを実行する
        /// </summary>
        private void ExecuteSettingsCommand()
        {
            _logger.Info("start");
            _regionManager.RequestNavigate("ContentRegion", "SettingsPage");
            IsMenuOpened = false;
            _logger.Info("end");
        }

        /// <summary>
        /// ヘッダを更新する
        /// </summary>
        /// <param name="club">マイクラブ</param>
        private void RefreshHeader(Club club)
        {
            _logger.Info("start");
            MyClubName = club.Name;
            MyClubDivision = $"明治安田生命{club.Division}リーグ";
            _logger.Info("end");
        }

        /// <summary>
        /// スナックバーにメッセージを表示する
        /// </summary>
        /// <param name="message"></param>
        private void ShowSnackbar(string message)
        {
            _logger.Info("start");
            MessageQueue.Enqueue(message);
            _logger.Info("end");
        }
    }
}
