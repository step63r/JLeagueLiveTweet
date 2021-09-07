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
    }
}
