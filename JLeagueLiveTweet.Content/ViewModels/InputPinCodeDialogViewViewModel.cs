﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MinatoProject.Apps.JLeagueLiveTweet.Content.ViewModels
{
    /// <summary>
    /// InputPinCodeDialogView.xamlのViewModelクラス
    /// </summary>
    public class InputPinCodeDialogViewViewModel : BindableBase, IDialogAware
    {
        #region プロパティ
        /// <summary>
        /// タイトル
        /// </summary>
        public string Title => "Twitter認証";

        private string _pinCode = string.Empty;
        /// <summary>
        /// PinCode
        /// </summary>
        public string PinCode
        {
            get => _pinCode;
            set => _ = SetProperty(ref _pinCode, value);
        }
        #endregion

        #region コマンド
        /// <summary>
        /// 認証コマンド
        /// </summary>
        public ReactiveCommand AuthorizeCommand { get; } = new ReactiveCommand();
        #endregion

        #region イベント
        /// <summary>
        /// ダイアログのCloseを要求するAction
        /// </summary>
        public event Action<IDialogResult> RequestClose;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InputPinCodeDialogViewViewModel()
        {
            _ = AuthorizeCommand.Subscribe(() =>
            {
                var param = new DialogParameters();
                param.Add(nameof(PinCode), PinCode);
                var ret = new DialogResult(ButtonResult.OK, param);
                RequestClose?.Invoke(ret);
            });
        }

        /// <summary>
        /// ダイアログがClose可能か
        /// </summary>
        /// <returns></returns>
        public bool CanCloseDialog()
        {
            return true;
        }

        /// <summary>
        /// ダイアログClose時のイベントハンドラ
        /// </summary>
        public void OnDialogClosed()
        {

        }

        /// <summary>
        /// ダイアログOpen時のイベントハンドラ
        /// </summary>
        /// <param name="parameters"></param>
        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
    }
}
