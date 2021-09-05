using MinatoProject.Apps.JLeagueLiveTweet.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

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
        #endregion

        #region メンバ変数
        /// <summary>
        /// クラブ情報をストアするインスタンス
        /// </summary>
        private ClubsStore _clubsStore = ClubsStore.GetInstance();
        /// <summary>
        /// 選手情報をストアするインスタンス
        /// </summary>
        private PlayersStore _playersStore = PlayersStore.GetInstance();
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel(IRegionManager regionManager)
        {
            // インターフェイスの取得
            _regionManager = regionManager;

            // コマンドの登録
            ScoreBoardCommand = new DelegateCommand(ExecuteScoreBoardCommand);
            ClubsCommand = new DelegateCommand(ExecuteClubsCommand);
            PlayersCommand = new DelegateCommand(ExecutePlayersCommand);
            SettingsCommand = new DelegateCommand(ExecuteSettingsCommand);

            // ストアを初期化
            _clubsStore.InitializeInstance();
            _playersStore.InitializeInstance();
        }

        /// <summary>
        /// スコアボードコマンドを実行する
        /// </summary>
        private void ExecuteScoreBoardCommand()
        {
            _regionManager.RequestNavigate("ContentRegion", "TopPage");
            IsMenuOpened = false;
        }

        /// <summary>
        /// クラブ情報コマンドを実行する
        /// </summary>
        private void ExecuteClubsCommand()
        {
            _regionManager.RequestNavigate("ContentRegion", "ClubsPage");
            IsMenuOpened = false;
        }

        /// <summary>
        /// 選手情報コマンドを実行する
        /// </summary>
        private void ExecutePlayersCommand()
        {
            _regionManager.RequestNavigate("ContentRegion", "PlayersPage");
            IsMenuOpened = false;
        }

        /// <summary>
        /// 設定コマンドを実行する
        /// </summary>
        private void ExecuteSettingsCommand()
        {
            _regionManager.RequestNavigate("ContentRegion", "SettingsPage");
            IsMenuOpened = false;
        }
    }
}
