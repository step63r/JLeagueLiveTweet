using System;
using System.Xml.Serialization;

namespace MinatoProject.Apps.JLeagueLiveTweet.Core.Models
{
    /// <summary>
    /// 選手データクラス
    /// </summary>
    [XmlRoot]
    public class Player : IEquatable<Player>
    {
        /// <summary>
        /// 所属クラブ
        /// </summary>
        [XmlElement]
        public Club Club { get; set; }
        /// <summary>
        /// 背番号
        /// </summary>
        [XmlElement]
        public int Number { get; set; }
        /// <summary>
        /// 名前
        /// </summary>
        [XmlElement]
        public string Name { get; set; }
        /// <summary>
        /// ポジション
        /// </summary>
        [XmlElement]
        public Position Position { get; set; }

        #region IEquatable
        /// <summary>
        /// インスタンスの等価性を判断する
        /// </summary>
        /// <param name="other">別のインスタンス</param>
        /// <returns></returns>
        public bool Equals(Player other)
        {
            return (Club == null || Club.Equals(other.Club)) &&
                Number == other.Number &&
                Name.Equals(other.Name) &&
                Position == other.Position;
        }
        #endregion
    }
}
