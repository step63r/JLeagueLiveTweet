using MaterialDesignColors;
using Prism.Mvvm;
using System;
using System.Xml.Serialization;

namespace MinatoProject.Apps.JLeagueLiveTweet.Core.Models
{
    /// <summary>
    /// クラブ
    /// </summary>
    [XmlRoot]
    public class Club : BindableBase, IEquatable<Club>
    {
        private Division _division;
        /// <summary>
        /// 所属ディビジョン
        /// </summary>
        [XmlElement]
        public Division Division
        {
            get => _division;
            set => _ = SetProperty(ref _division, value);
        }

        private string _name;
        /// <summary>
        /// クラブ名
        /// </summary>
        [XmlElement]
        public string Name
        {
            get => _name;
            set => _ = SetProperty(ref _name, value);
        }

        private string _abbreviation;
        /// <summary>
        /// クラブ名略称
        /// </summary>
        [XmlElement]
        public string Abbreviation
        {
            get => _abbreviation;
            set => _ = SetProperty(ref _abbreviation, value);
        }

        private Prefecture _prefecture;
        /// <summary>
        /// ホームタウン
        /// </summary>
        [XmlElement]
        public Prefecture Prefecture
        {
            get => _prefecture;
            set => _ = SetProperty(ref _prefecture, value);
        }

        private PrimaryColor _primaryColor;
        /// <summary>
        /// クラブカラー (1)
        /// </summary>
        [XmlIgnore]
        public PrimaryColor PrimaryColor
        {
            get => _primaryColor;
            set => _ = SetProperty(ref _primaryColor, value);
        }

        private SecondaryColor _secondaryColor;
        /// <summary>
        /// クラブカラー (2)
        /// </summary>
        [XmlIgnore]
        public SecondaryColor SecondaryColor
        {
            get => _secondaryColor;
            set => _ = SetProperty(ref _secondaryColor, value);
        }

        private string _hashTag;
        /// <summary>
        /// ハッシュタグ
        /// </summary>
        [XmlElement]
        public string HashTag
        {
            get => _hashTag;
            set => _ = SetProperty(ref _hashTag, value);
        }

        #region IEquatable
        /// <summary>
        /// インスタンスの等価性を判断する
        /// </summary>
        /// <param name="other">別のインスタンス</param>
        /// <returns></returns>
        public bool Equals(Club other)
        {
            return Name.Equals(other.Name);
        }
        #endregion
    }
}
