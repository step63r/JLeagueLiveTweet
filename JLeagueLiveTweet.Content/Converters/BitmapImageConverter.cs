using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MinatoProject.Apps.JLeagueLiveTweet.Content.Converters
{
    /// <summary>
    /// URLからBitmapを取得するコンバータ
    /// </summary>
    public class BitmapImageConverter : IValueConverter
    {
        /// <summary>
        /// Convert
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string
                ? new BitmapImage(new Uri((string)value, UriKind.RelativeOrAbsolute))
                : value is Uri ? new BitmapImage((Uri)value) : throw new NotSupportedException();
        }

        /// <summary>
        /// ConvertBack
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
