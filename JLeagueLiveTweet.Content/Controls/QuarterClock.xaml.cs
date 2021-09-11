using System;
using System.Windows;
using System.Windows.Controls;

namespace MinatoProject.Apps.JLeagueLiveTweet.Content.Controls
{
    /// <summary>
    /// QuarterClock.xaml の相互作用ロジック
    /// </summary>
    public partial class QuarterClock : UserControl
    {
        #region 依存関係プロパティ
        /// <summary>
        /// このコントロールに紐づくTimeSpan
        /// </summary>
        public static readonly DependencyProperty TimeSpanProperty = DependencyProperty.Register(
            nameof(TimeSpan),
            typeof(TimeSpan),
            typeof(QuarterClock),
            new PropertyMetadata(new TimeSpan(), new PropertyChangedCallback((sender, e) =>
            {
                (sender as QuarterClock).OnTimeSpanPropertyChanged(sender, e);
            })));
        #endregion

        #region プロパティ
        /// <summary>
        /// このコントロールに紐づくTimeSpan
        /// </summary>
        public TimeSpan TimeSpan
        {
            get => (TimeSpan)GetValue(TimeSpanProperty);
            set => SetValue(TimeSpanProperty, value);
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public QuarterClock()
        {
            InitializeComponent();
            TimeSpanText.Text = $"{TimeSpan.TotalMinutes:00}:{TimeSpan.Seconds:D2}";
        }
        #endregion

        #region イベントハンドラ
        /// <summary>
        /// このコントロールに紐づくTimeSpanが変更されたときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimeSpanPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TimeSpanText.Text = $"{TimeSpan.Minutes:00}:{TimeSpan.Seconds:D2}";

            // 上限45分とする
            AngleMinute.Angle = TimeSpan > new TimeSpan(0, 45, 0)
                ? 45.0 * 360.0 / 60.0
                : (TimeSpan.Minutes + TimeSpan.Seconds / 60.0) * 360.0 / 60.0;
        }
        #endregion
    }
}
