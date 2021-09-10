using LinqToTwitter;
using log4net;
using MinatoProject.Apps.JLeagueLiveTweet.Core.Models;
using MinatoProject.Apps.JLeagueLiveTweet.Core.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MinatoProject.Apps.JLeagueLiveTweet.Content.ViewModels
{
    /// <summary>
    /// SettingsPage.xamlのViewModelクラス
    /// </summary>
    public class SettingsPageViewModel : BindableBase
    {
        #region 定数
        /// <summary>
        /// Twitter連携用ID
        /// </summary>
        private const string TwitterAppId = "21855160";
        #endregion

        #region プロパティ
        private ObservableCollection<Club> _clubs = new ObservableCollection<Club>();
        /// <summary>
        /// クラブ一覧
        /// </summary>
        public ObservableCollection<Club> Clubs
        {
            get => _clubs;
            set => _ = SetProperty(ref _clubs, value);
        }

        private Club _selectedClub = null;
        /// <summary>
        /// 選択されたクラブ
        /// </summary>
        public Club SelectedClub
        {
            get => _selectedClub;
            set
            {
                _ = SetProperty(ref _selectedClub, value);
                if (_selectedClub != null)
                {
                    _ = _configStore.SetMyClub(_selectedClub);
                    _eventAggregator?.GetEvent<PubSubEvent<Club>>().Publish(_selectedClub);
                }
            }
        }

        private Account _twitterAccount = null;
        /// <summary>
        /// Twitterアカウント
        /// </summary>
        public Account TwitterAccount
        {
            get => _twitterAccount;
            set => _ = SetProperty(ref _twitterAccount, value);
        }
        #endregion

        #region コマンド
        /// <summary>
        /// Twitter認証コマンド
        /// </summary>
        public DelegateCommand AuthorizeTwitterCommand { get; private set; }
        /// <summary>
        /// Twitter連携解除コマンド
        /// </summary>
        public DelegateCommand RevokeTwitterCommand { get; private set; }
        #endregion

        #region インターフェイス
        /// <summary>
        /// IEventAggregator
        /// </summary>
        private readonly IEventAggregator _eventAggregator = null;
        /// <summary>
        /// IDialogService
        /// </summary>
        private readonly IDialogService _dialogService = null;
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
        /// ユーザー設定情報をストアするインスタンス
        /// </summary>
        private readonly ConfigStore _configStore = ConfigStore.GetInstance();
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="eventAggregator">IEventAggregator</param>
        /// <param name="dialogService">IDialogService</param>
        public SettingsPageViewModel(IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _logger.Info("start");
            // インターフェイスの取得
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;

            Clubs = new ObservableCollection<Club>(_clubsStore.GetClubs());

            if (_configStore.GetConfig().MyClub != null)
            {
                SelectedClub = Clubs.First(p => p.Equals(_configStore.GetConfig().MyClub));
            }

            // コマンドの登録
            AuthorizeTwitterCommand = new DelegateCommand(async () => await ExecuteAuthorizeTwitterCommand(), CanExecuteAuthorizeTwitterCommand)
                .ObservesProperty(() => TwitterAccount);
            RevokeTwitterCommand = new DelegateCommand(ExecuteRevokeTwitterCommand, CanExecuteRevokeTwitterCommand)
                .ObservesProperty(() => TwitterAccount);

            if (!string.IsNullOrEmpty(_configStore.GetTwitterAccessToken()) &&
                !string.IsNullOrEmpty(_configStore.GetTwitterAccessTokenSecret()))
            {
                var auth = SingleUserAuthorizeAsync().Result;
                if (auth == null)
                {
                    _configStore.SetTwitterAccessToken(string.Empty);
                    _configStore.SetTwitterAccessTokenSecret(string.Empty);
                    _eventAggregator.GetEvent<PubSubEvent<string>>().Publish("アカウント取得に失敗しました。再度ログインしてください");
                }
                else
                {
                    var context = new TwitterContext(auth);
                    TwitterAccount = context.Account.Where(p => p.Type == AccountType.VerifyCredentials).Single();
                }
            }
            _logger.Info("end");
        }
        #endregion

        #region メソッド
        /// <summary>
        /// Twitter認証コマンドを実行する
        /// </summary>
        private async Task ExecuteAuthorizeTwitterCommand()
        {
            _logger.Info("start");
            var auth = await PinAuthorizeAsync();
            if (auth == null)
            {
                _configStore.SetTwitterAccessToken(string.Empty);
                _configStore.SetTwitterAccessTokenSecret(string.Empty);
                _eventAggregator.GetEvent<PubSubEvent<string>>().Publish("ログインに失敗しました");
            }
            else
            {
                var context = new TwitterContext(auth);
                TwitterAccount = context.Account.Where(p => p.Type == AccountType.VerifyCredentials).Single();

                _configStore.SetTwitterAccessToken(auth.CredentialStore.OAuthToken);
                _configStore.SetTwitterAccessTokenSecret(auth.CredentialStore.OAuthTokenSecret);
                _eventAggregator.GetEvent<PubSubEvent<string>>().Publish("ログインに成功しました");
            }
            _logger.Info("end");
        }
        /// <summary>
        /// Twitter認証コマンドが実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteAuthorizeTwitterCommand()
        {
            return TwitterAccount == null;
        }

        /// <summary>
        /// Twitter連携解除コマンドを実行する
        /// </summary>
        private void ExecuteRevokeTwitterCommand()
        {
            _logger.Info("start");
            _configStore.SetTwitterAccessToken(string.Empty);
            _configStore.SetTwitterAccessTokenSecret(string.Empty);
            TwitterAccount = null;
            _eventAggregator.GetEvent<PubSubEvent<string>>().Publish("設定を削除しました。Twitterアカウントからアプリの許可を取り消してください");

            _ = Process.Start($@"https://twitter.com/settings/applications/{TwitterAppId}");
            _logger.Info("end");
        }
        /// <summary>
        /// Twitter連携解除コマンドが実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteRevokeTwitterCommand()
        {
            return TwitterAccount != null;
        }

        /// <summary>
        /// アカウント認証を行う
        /// </summary>
        /// <returns>SingleUserAuthorizer</returns>
        private async Task<SingleUserAuthorizer> SingleUserAuthorizeAsync()
        {
            _logger.Info("start");
            string accessToken = _configStore.GetTwitterAccessToken();
            string accessTokenSecret = _configStore.GetTwitterAccessTokenSecret();

            var auth = new SingleUserAuthorizer()
            {
                CredentialStore = new SingleUserInMemoryCredentialStore()
                {
                    ConsumerKey = _configStore.GetTwitterApiKey(),
                    ConsumerSecret = _configStore.GetTwitterApiSecret(),
                    AccessToken = accessToken,
                    AccessTokenSecret = accessTokenSecret
                }
            };

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
            return context == null ? default : context.Authorizer as SingleUserAuthorizer;
        }

        /// <summary>
        /// PIN認証を行う
        /// </summary>
        /// <returns>PinAuthorizer</returns>
        private async Task<PinAuthorizer> PinAuthorizeAsync()
        {
            _logger.Info("start");
            var dispatcher = Dispatcher.CurrentDispatcher;

            string accessToken = _configStore.GetTwitterAccessToken();
            string accessTokenSecret = _configStore.GetTwitterAccessTokenSecret();

            var auth = new PinAuthorizer()
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
            return context == null ? default : context.Authorizer as PinAuthorizer;
        }
        #endregion
    }
}
