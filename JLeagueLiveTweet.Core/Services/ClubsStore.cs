using MinatoProject.Apps.JLeagueLiveTweet.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace MinatoProject.Apps.JLeagueLiveTweet.Core.Services
{
    /// <summary>
    /// クラブ情報をストアするクラス
    /// </summary>
    public sealed class ClubsStore
    {
        #region Singleton
        /// <summary>
        /// シングルトン インスタンス
        /// </summary>
        private static ClubsStore _instance = new ClubsStore();

        /// <summary>
        /// インスタンスを取得する
        /// </summary>
        /// <returns></returns>
        public static ClubsStore GetInstance()
        {
            return _instance;
        }
        #endregion

        #region メンバ変数
        /// <summary>
        /// クラブ情報
        /// </summary>
        private IList<Club> _clubs = new List<Club>();
        /// <summary>
        /// クラブ情報ファイルパス
        /// </summary>
        private static readonly string _clubsFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\JLeagueLiveTweet\clubs.xml";
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private ClubsStore()
        {
        }
        #endregion

        #region メソッド
        /// <summary>
        /// インスタンスを初期化する
        /// </summary>
        public void InitializeInstance()
        {
            // ファイルが存在しない場合、新規作成
            if (!File.Exists(_clubsFilePath))
            {
                _clubs = InitializeClubs();
                Serialize(_clubs as List<Club>, _clubsFilePath);
            }
            _clubs = Deserialize<List<Club>>(_clubsFilePath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<Club> GetClubs()
        {
            return _clubs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="club"></param>
        /// <returns></returns>
        public IList<Club> AddClub(Club club)
        {
            _clubs.Add(club);
            return _clubs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="club"></param>
        /// <returns></returns>
        public IList<Club> RemoveClub(Club club)
        {
            _clubs.Remove(club);
            return _clubs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        private static void Serialize<T>(T obj, string filePath) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                serializer.Serialize(sw, obj);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static T Deserialize<T>(string filePath) where T : class
        {
            var ret = default(T);
            var serializer = new XmlSerializer(typeof(T));
            using (var sr = new StreamReader(filePath, Encoding.UTF8))
            {
                ret = serializer.Deserialize(sr) as T;
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static IList<Club> InitializeClubs()
        {
            return new List<Club>()
            {
                new Club()
                {
                    Division = Division.J1,
                    Name = "北海道コンサドーレ札幌",
                    Abbreviation = "札幌",
                    Prefecture = Prefecture.北海道,
                    HashTag = "#consadole",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "ベガルタ仙台",
                    Abbreviation = "仙台",
                    Prefecture = Prefecture.宮城,
                    HashTag = "#vegalta",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "鹿島アントラーズ",
                    Abbreviation = "鹿島",
                    Prefecture = Prefecture.茨城,
                    HashTag = "#antlers",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "浦和レッズ",
                    Abbreviation = "浦和",
                    Prefecture = Prefecture.埼玉,
                    HashTag = "#urawareds",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "柏レイソル",
                    Abbreviation = "柏",
                    Prefecture = Prefecture.千葉,
                    HashTag = "#reysol",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "FC東京",
                    Abbreviation = "FC東京",
                    Prefecture = Prefecture.東京,
                    HashTag = "#fctokyo",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "川崎フロンターレ",
                    Abbreviation = "川崎F",
                    Prefecture = Prefecture.神奈川,
                    HashTag = "#frontale",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "横浜F・マリノス",
                    Abbreviation = "横浜FM",
                    Prefecture = Prefecture.神奈川,
                    HashTag = "#fmarinos",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "横浜FC",
                    Abbreviation = "横浜FC",
                    Prefecture = Prefecture.神奈川,
                    HashTag = "#yokohamafc",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "湘南ベルマーレ",
                    Abbreviation = "湘南",
                    Prefecture = Prefecture.神奈川,
                    HashTag = "#bellmare",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "清水エスパルス",
                    Abbreviation = "清水",
                    Prefecture = Prefecture.静岡,
                    HashTag = "#spulse",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "名古屋グランパス",
                    Abbreviation = "名古屋",
                    Prefecture = Prefecture.愛知,
                    HashTag = "#grampus",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "ガンバ大阪",
                    Abbreviation = "G大阪",
                    Prefecture = Prefecture.大阪,
                    HashTag = "#gamba",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "セレッソ大阪",
                    Abbreviation = "C大阪",
                    Prefecture = Prefecture.大阪,
                    HashTag = "#cerezo",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "ヴィッセル神戸",
                    Abbreviation = "神戸",
                    Prefecture = Prefecture.兵庫,
                    HashTag = "#vissel",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "サンフレッチェ広島",
                    Abbreviation = "広島",
                    Prefecture = Prefecture.広島,
                    HashTag = "#sanfrecce",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "徳島ヴォルティス",
                    Abbreviation = "徳島",
                    Prefecture = Prefecture.徳島,
                    HashTag = "#vortis",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "アビスパ福岡",
                    Abbreviation = "福岡",
                    Prefecture = Prefecture.福岡,
                    HashTag = "#avispa",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "サガン鳥栖",
                    Abbreviation = "鳥栖",
                    Prefecture = Prefecture.佐賀,
                    HashTag = "#sagantosu",
                },
                new Club()
                {
                    Division = Division.J1,
                    Name = "大分トリニータ",
                    Abbreviation = "大分",
                    Prefecture = Prefecture.大分,
                    HashTag = "#trinita",
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "ブラウブリッツ秋田",
                    Abbreviation = "秋田",
                    Prefecture = Prefecture.秋田,
                    HashTag = "#bbakita"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "モンテディオ山形",
                    Abbreviation = "山形",
                    Prefecture = Prefecture.山形,
                    HashTag = "#montedio"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "水戸ホーリーホック",
                    Abbreviation = "水戸",
                    Prefecture = Prefecture.茨城,
                    HashTag = "#hollyhock"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "栃木SC",
                    Abbreviation = "栃木",
                    Prefecture = Prefecture.栃木,
                    HashTag = "#tochigisc"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "ザスパクサツ群馬",
                    Abbreviation = "群馬",
                    Prefecture = Prefecture.群馬,
                    HashTag = "#thespakusatsu"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "大宮アルディージャ",
                    Abbreviation = "大宮",
                    Prefecture = Prefecture.埼玉,
                    HashTag = "#ardija"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "ジェフユナイテッド千葉",
                    Abbreviation = "千葉",
                    Prefecture = Prefecture.千葉,
                    HashTag = "#jefunited"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "東京ヴェルディ",
                    Abbreviation = "東京V",
                    Prefecture = Prefecture.東京,
                    HashTag = "#verdy"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "FC町田ゼルビア",
                    Abbreviation = "町田",
                    Prefecture = Prefecture.東京,
                    HashTag = "#zelvia"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "SC相模原",
                    Abbreviation = "相模原",
                    Prefecture = Prefecture.神奈川,
                    HashTag = "#sc_sagamihara"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "ヴァンフォーレ甲府",
                    Abbreviation = "甲府",
                    Prefecture = Prefecture.山梨,
                    HashTag = "#ventforet"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "松本山雅FC",
                    Abbreviation = "松本",
                    Prefecture = Prefecture.長野,
                    HashTag = "#yamaga"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "アルビレックス新潟",
                    Abbreviation = "新潟",
                    Prefecture = Prefecture.新潟,
                    HashTag = "#albirex"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "ツエーゲン金沢",
                    Abbreviation = "金沢",
                    Prefecture = Prefecture.石川,
                    HashTag = "#zweigen"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "ジュビロ磐田",
                    Abbreviation = "磐田",
                    Prefecture = Prefecture.静岡,
                    HashTag = "#jubilo"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "京都サンガF.C.",
                    Abbreviation = "京都",
                    Prefecture = Prefecture.京都,
                    HashTag = "#sanga"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "ファジアーノ岡山",
                    Abbreviation = "岡山",
                    Prefecture = Prefecture.岡山,
                    HashTag = "#fagiano"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "レノファ山口FC",
                    Abbreviation = "山口",
                    Prefecture = Prefecture.山口,
                    HashTag = "#renofa"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "愛媛FC",
                    Abbreviation = "愛媛",
                    Prefecture = Prefecture.愛媛,
                    HashTag = "#ehimefc"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "ギラヴァンツ北九州",
                    Abbreviation = "北九州",
                    Prefecture = Prefecture.福岡,
                    HashTag = "#giravanz"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "V・ファーレン長崎",
                    Abbreviation = "長崎",
                    Prefecture = Prefecture.長崎,
                    HashTag = "#vvaren"
                },
                new Club()
                {
                    Division = Division.J2,
                    Name = "FC琉球",
                    Abbreviation = "琉球",
                    Prefecture = Prefecture.沖縄,
                    HashTag = "#fcryukyu"
                },
                new Club()
                {
                    Division = Division.J3,
                    Name = "ヴァンラーレ八戸",
                    Abbreviation = "八戸",
                    Prefecture = Prefecture.青森,
                    HashTag = "#全緑"
                },
                new Club()
                {
                    Division = Division.J3,
                    Name = "いわてグルージャ盛岡",
                    Abbreviation = "盛岡",
                    Prefecture = Prefecture.岩手,
                    HashTag = "#grulla"
                },
                new Club()
                {
                    Division = Division.J3,
                    Name = "福島ユナイテッドFC",
                    Abbreviation = "福島",
                    Prefecture = Prefecture.福島,
                    HashTag = "#福島ユナイテッド"
                },
                new Club()
                {
                    Division = Division.J3,
                    Name = "Y.S.C.C.横浜",
                    Abbreviation = "YS横浜",
                    Prefecture = Prefecture.神奈川,
                    HashTag = "#yscc"
                },
                new Club()
                {
                    Division = Division.J3,
                    Name = "AC長野パルセイロ",
                    Abbreviation = "長野",
                    Prefecture = Prefecture.長野,
                    HashTag = "#acnp"
                },
                new Club()
                {
                    Division = Division.J3,
                    Name = "カターレ富山",
                    Abbreviation = "富山",
                    Prefecture = Prefecture.富山,
                    HashTag = "#kataller"
                },
                new Club()
                {
                    Division = Division.J3,
                    Name = "藤枝MYFC",
                    Abbreviation = "藤枝",
                    Prefecture = Prefecture.静岡,
                    HashTag = "#藤枝MYFC"
                },
                new Club()
                {
                    Division = Division.J3,
                    Name = "アスルクラロ沼津",
                    Abbreviation = "沼津",
                    Prefecture = Prefecture.静岡,
                    HashTag = "#アスルクラロ沼津"
                },
                new Club()
                {
                    Division = Division.J3,
                    Name = "FC岐阜",
                    Abbreviation = "岐阜",
                    Prefecture = Prefecture.岐阜,
                    HashTag = "#fcgifu"
                },
                new Club()
                {
                    Division = Division.J3,
                    Name = "ガイナーレ鳥取",
                    Abbreviation = "鳥取",
                    Prefecture = Prefecture.鳥取,
                    HashTag = "#ガイナーレ鳥取"
                },
                new Club()
                {
                    Division = Division.J3,
                    Name = "カマタマーレ讃岐",
                    Abbreviation = "讃岐",
                    Prefecture = Prefecture.香川,
                    HashTag = "#カマタマーレ讃岐"
                },
                new Club()
                {
                    Division = Division.J3,
                    Name = "FC今治",
                    Abbreviation = "今治",
                    Prefecture = Prefecture.愛媛,
                    HashTag = "#FC今治"
                },
                new Club()
                {
                    Division = Division.J3,
                    Name = "ロアッソ熊本",
                    Abbreviation = "熊本",
                    Prefecture = Prefecture.熊本,
                    HashTag = "#ロアッソ熊本"
                },
                new Club()
                {
                    Division = Division.J3,
                    Name = "テゲバジャーロ宮崎",
                    Abbreviation = "宮崎",
                    Prefecture = Prefecture.宮崎,
                    HashTag = "#テゲバ"
                },
                new Club()
                {
                    Division = Division.J3,
                    Name = "鹿児島ユナイテッドFC",
                    Abbreviation = "鹿児島",
                    Prefecture = Prefecture.鹿児島,
                    HashTag = "#鹿児島ユナイテッドFC"
                },
            };
        }
        #endregion
    }
}