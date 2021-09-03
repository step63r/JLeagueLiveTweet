using Prism.Mvvm;

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
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
        }
    }
}
