using System.Xml.Serialization;

namespace MinatoProject.Apps.JLeagueLiveTweet.Core.Models
{
    /// <summary>
    /// ユーザー設定情報
    /// </summary>
    [XmlRoot]
    public class Config
    {
        /// <summary>
        /// 自クラブ
        /// </summary>
        [XmlElement]
        public Club MyClub { get; set; }

        /// <summary>
        /// Twitterのアクセストークン
        /// </summary>
        [XmlElement]
        public string TwitterAccessToken { get; set; }

        /// <summary>
        /// Twitterのアクセストークンシークレット
        /// </summary>
        [XmlElement]
        public string TwitterAccessTokenSecret { get; set; }
    }
}
