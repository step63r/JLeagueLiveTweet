using log4net;
using MinatoProject.Apps.JLeagueLiveTweet.Core.Models;
using MinatoProject.Apps.JLeagueLiveTweet.Core.Services;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace MinatoProject.Apps.JLeagueLiveTweet.Content.ViewModels
{
    /// <summary>
    /// SettingsPage.xamlのViewModelクラス
    /// </summary>
    public class SettingsPageViewModel : BindableBase
    {
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
        #endregion

        #region インターフェイス
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
        /// ユーザー設定情報をストアするインスタンス
        /// </summary>
        private readonly ConfigStore _configStore = ConfigStore.GetInstance();
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="eventAggregator">IEventAggregator</param>
        public SettingsPageViewModel(IEventAggregator eventAggregator)
        {
            _logger.Info("start");
            _eventAggregator = eventAggregator;

            Clubs = new ObservableCollection<Club>(_clubsStore.GetClubs());

            if (_configStore.GetConfig().MyClub != null)
            {
                SelectedClub = Clubs.First(p => p.Name.Equals(_configStore.GetConfig().MyClub.Name));
            }
            _logger.Info("end");
        }
        #endregion
    }
}
