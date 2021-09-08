using log4net;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace MinatoProject.Apps.JLeagueLiveTweet.Core.Services
{
    /// <summary>
    /// データをストアする基底クラス
    /// </summary>
    public abstract class ServiceStoreBase
    {
        /// <summary>
        /// ロガー
        /// </summary>
        protected readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// インスタンスを初期化する
        /// </summary>
        public virtual void InitializeInstance()
        {
        }

        /// <summary>
        /// オブジェクトをXMLにシリアル化する
        /// </summary>
        /// <typeparam name="T">型テンプレート</typeparam>
        /// <param name="obj">オブジェクト</param>
        /// <param name="filePath">ファイルパス</param>
        protected static void Serialize<T>(T obj, string filePath) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                serializer.Serialize(sw, obj);
            }
        }

        /// <summary>
        /// XMLからオブジェクトに逆シリアル化する
        /// </summary>
        /// <typeparam name="T">型テンプレート</typeparam>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>オブジェクト</returns>
        protected static T Deserialize<T>(string filePath) where T : class
        {
            var ret = default(T);
            var serializer = new XmlSerializer(typeof(T));
            using (var sr = new StreamReader(filePath, Encoding.UTF8))
            {
                ret = serializer.Deserialize(sr) as T;
            }
            return ret;
        }
    }
}
