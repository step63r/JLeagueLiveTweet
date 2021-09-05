using MaterialDesignColors;
using System.Xml.Serialization;

namespace MinatoProject.Apps.JLeagueLiveTweet.Core.Models
{
    /// <summary>
    /// クラブ
    /// </summary>
    [XmlRoot]
    public class Club
    {
        /// <summary>
        /// 所属ディビジョン
        /// </summary>
        [XmlElement]
        public Division Division { get; set; }
        /// <summary>
        /// クラブ名
        /// </summary>
        [XmlElement]
        public string Name { get; set; }
        /// <summary>
        /// クラブ名略称
        /// </summary>
        [XmlElement]
        public string Abbreviation { get; set; }
        /// <summary>
        /// ホームタウン
        /// </summary>
        [XmlElement]
        public Prefecture Prefecture { get; set; }
        /// <summary>
        /// クラブカラー (1)
        /// </summary>
        [XmlIgnore]
        public PrimaryColor PrimaryColor { get; set; }
        /// <summary>
        /// クラブカラー (2)
        /// </summary>
        [XmlIgnore]
        public SecondaryColor SecondaryColor { get; set; }
        /// <summary>
        /// ハッシュタグ
        /// </summary>
        [XmlElement]
        public string HashTag { get; set; }
    }
}
