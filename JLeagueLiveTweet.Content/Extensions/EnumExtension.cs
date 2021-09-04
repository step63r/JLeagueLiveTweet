using System;

namespace MinatoProject.Apps.JLeagueLiveTweet.Content.Extensions
{
    /// <summary>
    /// 列挙型の拡張メソッドクラス
    /// </summary>
    internal static class EnumExtension
    {
        /// <summary>
        /// 次の要素を取得する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"Argument {typeof(T).FullName} is not an Enum");
            }

            var arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf(arr, src) + 1;
            return (arr.Length == j) ? arr[0] : arr[j];
        }
    }
}
