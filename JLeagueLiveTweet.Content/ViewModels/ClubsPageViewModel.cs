using MinatoProject.Apps.JLeagueLiveTweet.Core.Models;
using MinatoProject.Apps.JLeagueLiveTweet.Core.Services;
using Prism.Mvvm;
using System.Collections.ObjectModel;

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
        #endregion

        #region メンバ変数
        /// <summary>
        /// クラブ情報をストアするインスタンス
        /// </summary>
        private ClubsStore _clubsStore = ClubsStore.GetInstance();
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ClubsPageViewModel()
        {
            Clubs = new ObservableCollection<Club>(_clubsStore.GetClubs());
        }
        #endregion
    }
}
