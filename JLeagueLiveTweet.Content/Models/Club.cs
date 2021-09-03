using MaterialDesignColors;

namespace MinatoProject.Apps.JLeagueLiveTweet.Content.Models
{
    /// <summary>
    /// クラブ
    /// </summary>
    public class Club
    {
        /// <summary>
        /// 所属ディビジョン
        /// </summary>
        public Division Division { get; set; }
        /// <summary>
        /// クラブ名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// クラブ名略称
        /// </summary>
        public string Abbreviation { get; set; }
        /// <summary>
        /// ホームタウン
        /// </summary>
        public Prefecture Prefecture { get; set; }
        /// <summary>
        /// クラブカラー (1)
        /// </summary>
        public PrimaryColor PrimaryColor { get; set; }
        /// <summary>
        /// クラブカラー (2)
        /// </summary>
        public SecondaryColor SecondaryColor { get; set; }
        /// <summary>
        /// ハッシュタグ
        /// </summary>
        public string HashTag { get; set; }
    }
}
