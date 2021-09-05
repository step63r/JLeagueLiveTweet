using MinatoProject.Apps.JLeagueLiveTweet.Core.Extensions;
using System.ComponentModel;

namespace MinatoProject.Apps.JLeagueLiveTweet.Core.Models
{
    /// <summary>
    /// ディビジョン列挙型
    /// </summary>
    public enum Division
    {
        /// <summary>
        /// J1
        /// </summary>
        [Description("J1")]
        J1,
        /// <summary>
        /// J2
        /// </summary>
        [Description("J2")]
        J2,
        /// <summary>
        /// J3
        /// </summary>
        [Description("J3")]
        J3,
        /// <summary>
        /// JFL
        /// </summary>
        [Description("JFL")]
        JFL
    }
}
