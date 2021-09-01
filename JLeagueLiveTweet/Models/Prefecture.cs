using System.Collections.Generic;

namespace MinatoProject.Apps.JLeagueLiveTweet.Models
{
    /// <summary>
    /// 都道府県
    /// </summary>
    public class Prefecture
    {
        /// <summary>
        /// 都道府県コード
        /// </summary>
        public int PrefCode { get; set; }
        /// <summary>
        /// 都道府県名
        /// </summary>
        public string PrefName { get; set; }

        #region メソッド
        /// <summary>
        /// 都道府県を全て取得する
        /// </summary>
        /// <returns>都道府県一覧</returns>
        public static IEnumerable<Prefecture> GetPrefectures()
        {
            var ret = new List<Prefecture>()
            {
                new Prefecture() { PrefCode = 1, PrefName = "北海道" },
                new Prefecture() { PrefCode = 2, PrefName = "青森" },
                new Prefecture() { PrefCode = 3, PrefName = "岩手" },
                new Prefecture() { PrefCode = 4, PrefName = "宮城" },
                new Prefecture() { PrefCode = 5, PrefName = "秋田" },
                new Prefecture() { PrefCode = 6, PrefName = "山形" },
                new Prefecture() { PrefCode = 7, PrefName = "福島" },
                new Prefecture() { PrefCode = 8, PrefName = "茨城" },
                new Prefecture() { PrefCode = 9, PrefName = "栃木" },
                new Prefecture() { PrefCode = 10, PrefName = "群馬" },
                new Prefecture() { PrefCode = 11, PrefName = "埼玉" },
                new Prefecture() { PrefCode = 12, PrefName = "千葉" },
                new Prefecture() { PrefCode = 13, PrefName = "東京" },
                new Prefecture() { PrefCode = 14, PrefName = "神奈川" },
                new Prefecture() { PrefCode = 15, PrefName = "新潟" },
                new Prefecture() { PrefCode = 16, PrefName = "富山" },
                new Prefecture() { PrefCode = 17, PrefName = "石川" },
                new Prefecture() { PrefCode = 18, PrefName = "福井" },
                new Prefecture() { PrefCode = 19, PrefName = "山梨" },
                new Prefecture() { PrefCode = 20, PrefName = "長野" },
                new Prefecture() { PrefCode = 21, PrefName = "岐阜" },
                new Prefecture() { PrefCode = 22, PrefName = "静岡" },
                new Prefecture() { PrefCode = 23, PrefName = "愛知" },
                new Prefecture() { PrefCode = 24, PrefName = "三重" },
                new Prefecture() { PrefCode = 25, PrefName = "滋賀" },
                new Prefecture() { PrefCode = 26, PrefName = "京都" },
                new Prefecture() { PrefCode = 27, PrefName = "大阪" },
                new Prefecture() { PrefCode = 28, PrefName = "兵庫" },
                new Prefecture() { PrefCode = 29, PrefName = "奈良" },
                new Prefecture() { PrefCode = 30, PrefName = "和歌山" },
                new Prefecture() { PrefCode = 31, PrefName = "鳥取" },
                new Prefecture() { PrefCode = 32, PrefName = "島根" },
                new Prefecture() { PrefCode = 33, PrefName = "岡山" },
                new Prefecture() { PrefCode = 34, PrefName = "広島" },
                new Prefecture() { PrefCode = 35, PrefName = "山口" },
                new Prefecture() { PrefCode = 36, PrefName = "徳島" },
                new Prefecture() { PrefCode = 37, PrefName = "香川" },
                new Prefecture() { PrefCode = 38, PrefName = "愛媛" },
                new Prefecture() { PrefCode = 39, PrefName = "高知" },
                new Prefecture() { PrefCode = 40, PrefName = "福岡" },
                new Prefecture() { PrefCode = 41, PrefName = "佐賀" },
                new Prefecture() { PrefCode = 42, PrefName = "長崎" },
                new Prefecture() { PrefCode = 43, PrefName = "熊本" },
                new Prefecture() { PrefCode = 44, PrefName = "大分" },
                new Prefecture() { PrefCode = 45, PrefName = "宮崎" },
                new Prefecture() { PrefCode = 46, PrefName = "鹿児島" },
                new Prefecture() { PrefCode = 47, PrefName = "沖縄" },
            };

            return ret;
        }
        #endregion
    }
}
