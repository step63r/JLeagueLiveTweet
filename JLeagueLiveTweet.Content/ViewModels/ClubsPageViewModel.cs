using log4net;
using MinatoProject.Apps.JLeagueLiveTweet.Core.Models;
using MinatoProject.Apps.JLeagueLiveTweet.Core.Services;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Reflection;

namespace MinatoProject.Apps.JLeagueLiveTweet.Content.ViewModels
{
    public class ClubsPageViewModel : BindableBase
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
                _clubsStore.SetClub(_selectedClub);
            }
        }
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
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ClubsPageViewModel()
        {
            _logger.Info("start");
            Clubs = new ObservableCollection<Club>(_clubsStore.GetClubs());
            _logger.Info("end");
        }
        #endregion
    }
}
