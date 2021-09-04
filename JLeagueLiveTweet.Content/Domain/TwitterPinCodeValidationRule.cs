using System.Globalization;
using System.Windows.Controls;

namespace MinatoProject.Apps.JLeagueLiveTweet.Content.Domain
{
    /// <summary>
    /// Twitterアカウント認証時のPINコードのValidationRule
    /// </summary>
    public class TwitterPinCodeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty((value ?? "").ToString()))
            {
                return new ValidationResult(false, "必須入力です");
            }

            if (!int.TryParse(value.ToString(), out _))
            {
                return new ValidationResult(false, "数値で入力してください");
            }

            if (value.ToString().Length != 7)
            {
                return new ValidationResult(false, "7桁で入力してください");
            }

            return ValidationResult.ValidResult;
        }
    }
}
