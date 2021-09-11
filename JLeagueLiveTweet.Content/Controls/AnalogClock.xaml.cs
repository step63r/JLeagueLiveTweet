using System;
using System.Windows;
using System.Windows.Controls;

namespace MinatoProject.Apps.JLeagueLiveTweet.Content.Controls
{
    /// <summary>
    /// AnalogClock.xaml の相互作用ロジック
    /// </summary>
    public partial class AnalogClock : UserControl
    {
        #region 依存関係プロパティ
        /// <summary>
        /// このコントロールに紐づく日付時刻
        /// </summary>
        public static readonly DependencyProperty DateTimeProperty = DependencyProperty.Register(
            nameof(DateTime),
            typeof(DateTime),
            typeof(AnalogClock),
            new PropertyMetadata(DateTime.Now, new PropertyChangedCallback((sender, e) =>
            {
                (sender as AnalogClock).OnDateTimePropertyChanged(sender, e);
            })));
        /// <summary>
        /// 時針を表示するか
        /// </summary>
        public static readonly DependencyProperty IsHourVisibleProperty = DependencyProperty.Register(
            nameof(IsHourVisible),
            typeof(bool),
            typeof(AnalogClock),
            new PropertyMetadata(true, new PropertyChangedCallback((sender, e) =>
            {
                (sender as AnalogClock).OnIsHourVisiblePropertyChanged(sender, e);
            })));
        /// <summary>
        /// 分針を表示するか
        /// </summary>
        public static readonly DependencyProperty IsMinuteVisibleProperty = DependencyProperty.Register(
            nameof(IsMinuteVisible),
            typeof(bool),
            typeof(AnalogClock),
            new PropertyMetadata(true, new PropertyChangedCallback((sender, e) =>
            {
                (sender as AnalogClock).OnIsMinuteVisiblePropertyChanged(sender, e);
            })));
        /// <summary>
        /// 秒針を表示するか
        /// </summary>
        public static readonly DependencyProperty IsSecondVisibleProperty = DependencyProperty.Register(
            nameof(IsSecondVisible),
            typeof(bool),
            typeof(AnalogClock),
            new PropertyMetadata(true, new PropertyChangedCallback((sender, e) =>
            {
                (sender as AnalogClock).OnIsSecondVisiblePropertyChanged(sender, e);
            })));
        #endregion

        #region プロパティ
        /// <summary>
        /// このコントロールに紐づく日付時刻
        /// </summary>
        public DateTime DateTime
        {
            get => (DateTime)GetValue(DateTimeProperty);
            set => SetValue(DateTimeProperty, value);
        }
        /// <summary>
        /// 時針の表示有無
        /// </summary>
        public bool IsHourVisible
        {
            get => (bool)GetValue(IsHourVisibleProperty);
            set => SetValue(IsHourVisibleProperty, value);
        }
        /// <summary>
        /// 分針の表示有無
        /// </summary>
        public bool IsMinuteVisible
        {
            get => (bool)GetValue(IsMinuteVisibleProperty);
            set => SetValue(IsMinuteVisibleProperty, value);
        }
        /// <summary>
        /// 秒針の表示有無
        /// </summary>
        public bool IsSecondVisible
        {
            get => (bool)GetValue(IsSecondVisibleProperty);
            set => SetValue(IsSecondVisibleProperty, value);
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AnalogClock()
        {
            InitializeComponent();
        }
        #endregion

        #region イベントハンドラ
        /// <summary>
        /// このコントロールに紐づく日付時刻が変更されたときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDateTimePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            AngleSecond.Angle = DateTime.Second * 360.0 / 60.0;
            AngleMinute.Angle = (DateTime.Minute + DateTime.Second / 60.0) * 360.0 / 60.0;
            AngleHour.Angle = (DateTime.Hour + DateTime.Minute / 60.0) * 360.0 / 12;
        }
        /// <summary>
        /// 時針の表示有無が変更されたときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIsHourVisiblePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            bool? ret = (sender as AnalogClock)?.GetValue(IsHourVisibleProperty) as bool?;
            HourLine.Visibility = ret == null ? Visibility.Visible : (bool)ret ? Visibility.Visible : Visibility.Collapsed;
        }
        /// <summary>
        /// 分針の表示有無が変更されたときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIsMinuteVisiblePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            bool? ret = (sender as AnalogClock)?.GetValue(IsMinuteVisibleProperty) as bool?;
            MinuteLine.Visibility = ret == null ? Visibility.Visible : (bool)ret ? Visibility.Visible : Visibility.Collapsed;
        }
        /// <summary>
        /// 秒針の表示有無が変更されたときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIsSecondVisiblePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            bool? ret = (sender as AnalogClock)?.GetValue(IsSecondVisibleProperty) as bool?;
            SecondLine.Visibility = ret == null ? Visibility.Visible : (bool)ret ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion
    }
}
