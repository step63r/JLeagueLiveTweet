using MaterialDesignThemes.Wpf;
using Microsoft.VisualBasic;
using MinatoProject.Apps.JLeagueLiveTweet.Core.Models;
using MinatoProject.Apps.JLeagueLiveTweet.Core.Services;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MinatoProject.Apps.JLeagueLiveTweet.Content.ViewModels
{
    /// <summary>
    /// PlayersPage.xamlのViewModelクラス
    /// </summary>
    public class PlayersPageViewModel : BindableBase
    {
        /// <summary>
        /// XPath定義
        /// </summary>
        static class XPath
        {
            /// <summary>
            /// ディビジョン プルダウン
            /// </summary>
            public const string DivisionsPulldown = "//*[@id=\"pullDown\"]";
            /// <summary>
            /// チーム プルダウン
            /// </summary>
            public const string TeamsPulldown = "//*[@id=\"teams\"]";
            /// <summary>
            /// 検索ボタン
            /// </summary>
            public const string SearchButton = "//*[@id=\"search\"]";
            /// <summary>
            /// 背番号と名前のテキスト（PLAYER_ROW_NUM は 3 から始まる）
            /// </summary>
            public const string NumberAndNameText = "//*[@id=\"contents\"]/form/div[3]/div[PLAYER_ROW_NUM]/p";
            /// <summary>
            /// ポジションのテキスト（PLAYER_ROW_NUM は 3 から始まる）
            /// </summary>
            public const string PositionText = "//*[@id=\"contents\"]/form/div[3]/div[PLAYER_ROW_NUM]/dl/dd[1]";
        }

        #region 定数
        /// <summary>
        /// J. League Data SiteのURL
        /// </summary>
        private const string JLeagueDataSiteUrl = @"https://data.j-league.or.jp/";
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
                RaisePropertyChanged(nameof(Players));
            }
        }

        /// <summary>
        /// 選手一覧
        /// </summary>
        public ObservableCollection<Player> Players => _playersStore == null || _selectedClub == null
                    ? new ObservableCollection<Player>()
                    : new ObservableCollection<Player>(_playersStore.GetPlayers(_selectedClub));

        private bool _isProgressing = false;
        /// <summary>
        /// 処理中フラグ
        /// </summary>
        public bool IsProgressing
        {
            get => _isProgressing;
            set => _ = SetProperty(ref _isProgressing, value);
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
        /// 選手情報更新コマンド
        /// </summary>
        public DelegateCommand UpdatePlayersCommand { get; private set; }
        #endregion

        #region メンバ変数
        /// <summary>
        /// クラブ情報をストアするインスタンス
        /// </summary>
        private readonly ClubsStore _clubsStore = ClubsStore.GetInstance();
        /// <summary>
        /// 選手情報をストアするインスタンス
        /// </summary>
        private readonly PlayersStore _playersStore = PlayersStore.GetInstance();
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PlayersPageViewModel()
        {
            Clubs = new ObservableCollection<Club>(_clubsStore.GetClubs());
            UpdatePlayersCommand = new DelegateCommand(async () => await ExecuteUpdatePlayersCommand(), CanExecuteUpdatePlayersCommand)
                .ObservesProperty(() => SelectedClub);
        }
        #endregion

        #region メソッド
        /// <summary>
        /// 選手情報更新コマンドを実行する
        /// </summary>
        private async Task ExecuteUpdatePlayersCommand()
        {
            string snackbarMessage = string.Empty;
            var players = new ObservableCollection<Player>();
            IsProgressing = true;
            try
            {
                await Task.Run(() =>
                {
                    string url = $@"{JLeagueDataSiteUrl}/SFIX02";
                    using (var driver = WebDriverFactory.CreateInstance(BrowserName.Chrome))
                    {
                        driver.Url = url;

                        var divisionsElement = new SelectElement(driver.FindElement(By.XPath(XPath.DivisionsPulldown)));
                        divisionsElement.SelectByIndex((int)SelectedClub.Division + 1);

                        Thread.Sleep(1000);

                        var teamsElement = new SelectElement(driver.FindElement(By.XPath(XPath.TeamsPulldown)));
                        try
                        {
                            teamsElement.SelectByText(SelectedClub.Name);
                        }
                        catch (NoSuchElementException)
                        {
                            string teamNameWide = Strings.StrConv(SelectedClub.Name, VbStrConv.Wide, 0x411);
                            Console.WriteLine($"全角で再検索：{teamNameWide}");
                            teamsElement.SelectByText(teamNameWide);
                        }

                        Thread.Sleep(1000);

                        driver.FindElement(By.XPath(XPath.SearchButton)).Click();

                        int index = 3;
                        while (true)
                        {
                            try
                            {
                                string numberAndNameText = driver.FindElement(By.XPath(XPath.NumberAndNameText.Replace("PLAYER_ROW_NUM", index.ToString()))).Text;
                                bool numberParseResult = int.TryParse(Regex.Match(numberAndNameText, @"\d+").Value, out int number);
                                if (!numberParseResult)
                                {
                                    Console.WriteLine($"番号登録なし（2種登録選手の可能性）: {numberAndNameText}");
                                    index++;
                                    continue;
                                }
                                string name = Regex.Match(numberAndNameText, @"[^ -~｡-ﾟ]+").Value;
                                string positionText = driver.FindElement(By.XPath(XPath.PositionText.Replace("PLAYER_ROW_NUM", index.ToString()))).Text;
                                var position = (Position)Enum.Parse(typeof(Position), positionText);
                                Console.WriteLine($"{number} {position} {name}");

                                players.Add(new Player()
                                {
                                    Club = SelectedClub,
                                    Number = number,
                                    Name = name,
                                    Position = position
                                });
                            }
                            catch (NoSuchElementException)
                            {
                                Console.WriteLine("検索が終了しました");
                                break;
                            }
                            index++;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                snackbarMessage = "選手情報の取得に失敗しました";
            }
            IsProgressing = false;
            _ = _playersStore.SetPlayers(SelectedClub, new List<Player>(players));
            RaisePropertyChanged(nameof(Players));

            if (string.IsNullOrEmpty(snackbarMessage))
            {
                snackbarMessage = "選手情報の取得が完了しました";
            }
            MessageQueue.Enqueue(snackbarMessage);
        }
        /// <summary>
        /// 選手情報更新コマンドが実行可能かどうかを判定する
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteUpdatePlayersCommand()
        {
            return SelectedClub != null;
        }
        #endregion
    }
}
