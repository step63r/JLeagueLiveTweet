using MaterialDesignColors;
using MinatoProject.Apps.JLeagueLiveTweet.Models;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private Club _myClub;
        /// <summary>
        /// 自クラブ
        /// </summary>
        public Club MyClub
        {
            get { return _myClub; }
            set { SetProperty(ref _myClub, value); }
        }

        private ObservableCollection<Club> _allClubs = new ObservableCollection<Club>();
        /// <summary>
        /// 全クラブ
        /// </summary>
        public ObservableCollection<Club> AllClubs
        {
            get { return _allClubs; }
            set { SetProperty(ref _allClubs, value); }
        }

        private Club _selectedClub;
        /// <summary>
        /// 選択されたクラブ
        /// </summary>
        public Club SelectedClub
        {
            get { return _selectedClub; }
            set { SetProperty(ref _selectedClub, value); }
        }

        private bool _isMyClubAway = false;
        /// <summary>
        /// 自クラブがアウェイか
        /// </summary>
        public bool IsMyClubAway
        {
            get { return _isMyClubAway; }
            set { SetProperty(ref _isMyClubAway, value); }
        }

        private ObservableCollection<Player> _allPlayers = new ObservableCollection<Player>();
        /// <summary>
        /// 自クラブの選手一覧
        /// </summary>
        public ObservableCollection<Player> AllPlayers
        {
            get { return _allPlayers; }
            set { SetProperty(ref _allPlayers, value); }
        }

        private string _homeClubName;
        /// <summary>
        /// ホームクラブ名
        /// </summary>
        public string HomeClubName
        {
            get { return _homeClubName; }
            set { SetProperty(ref _homeClubName, value); }
        }

        private string _awayClubName;
        /// <summary>
        /// アウェイクラブ名
        /// </summary>
        public string AwayClubName
        {
            get { return _awayClubName; }
            set { SetProperty(ref _awayClubName, value); }
        }

        private int _home1stScore;
        /// <summary>
        /// ホーム前半得点
        /// </summary>
        public int Home1stScore
        {
            get { return _home1stScore; }
            set { SetProperty(ref _home1stScore, value); }
        }

        private int _away1stScore;
        /// <summary>
        /// アウェイ前半得点
        /// </summary>
        public int Away1stScore
        {
            get { return _away1stScore; }
            set { SetProperty(ref _away1stScore, value); }
        }

        private int _home2ndScore;
        /// <summary>
        /// ホーム後半得点
        /// </summary>
        public int Home2ndScore
        {
            get { return _home2ndScore; }
            set { SetProperty(ref _home2ndScore, value); }
        }

        private int _away2ndScore;
        /// <summary>
        /// アウェイ後半得点
        /// </summary>
        public int Away2ndScore
        {
            get { return _away2ndScore; }
            set { SetProperty(ref _away2ndScore, value); }
        }

        /// <summary>
        /// ホーム得点
        /// </summary>
        public int HomeTotalScore
        {
            get { return _home1stScore + _home2ndScore; }
        }

        /// <summary>
        /// アウェイ得点
        /// </summary>
        public int AwayTotalScore
        {
            get { return _away1stScore + _away2ndScore; }
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
        #endregion

        #region メンバ変数
        /// <summary>
        /// 
        /// </summary>
        private static IEnumerable<Prefecture> Prefectures = Prefecture.GetPrefectures();
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            AllClubs = new ObservableCollection<Club>()
            {
                new Club()
                {
                    Division = Division.J2,
                    Name = "アルビレックス新潟",
                    Abbreviation = "新潟",
                    Prefecture = Prefectures.Where(p => p.PrefCode == 15).First(),
                    PrimaryColor = PrimaryColor.DeepOrange,
                    SecondaryColor = SecondaryColor.LightBlue,
                    HashTag = "#albirex"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "ジュビロ磐田",
                    Abbreviation = "磐田",
                    Prefecture = Prefectures.Where(p => p.PrefCode == 22).First(),
                    PrimaryColor = PrimaryColor.LightBlue,
                    SecondaryColor = SecondaryColor.LightBlue,
                    HashTag = "#jubilo"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "京都サンガF.C.",
                    Abbreviation = "京都",
                    Prefecture = Prefectures.Where(p => p.PrefCode == 26).First(),
                    PrimaryColor = PrimaryColor.Purple,
                    SecondaryColor = SecondaryColor.Purple,
                    HashTag = "#sanga"
                },
            };

            MyClub = AllClubs.Where(p => p.Name.Equals("アルビレックス新潟")).First();

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

            HomeClubName = MyClub.Abbreviation;

            ClubChangedCommand = new DelegateCommand(ExecuteClubChangedCommand, CanExecuteClubChangedCommand);
            HomeAwayChangedCommand = new DelegateCommand(ExecuteHomeAwayChangedCommand, CanExecuteHomeAwayChangedCommand);
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
    }
}
