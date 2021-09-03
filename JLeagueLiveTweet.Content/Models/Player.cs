namespace MinatoProject.Apps.JLeagueLiveTweet.Content.Models
{
    /// <summary>
    /// 選手データクラス
    /// </summary>
    public class Player
    {
        /// <summary>
        /// 所属クラブ
        /// </summary>
        public Club Club { get; set; }
        /// <summary>
        /// 背番号
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ポジション
        /// </summary>
        public Position Position { get; set; }
    }
}
