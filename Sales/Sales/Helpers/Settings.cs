
namespace Sales.Helpers
{

    using Plugin.Settings;
    using Plugin.Settings.Abstractions;
    using System;

    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants
        private const string tokenType = "TokenType";
        private const string accessToken = "AccessToken";
        private const  string  isRemenber= "IsRemenber";
        private const string expiresDate = "";
        private static readonly string stringSettingsDefault = string.Empty;
        private static readonly bool boolDefault = false;
        private static readonly DateTime dateUTC = DateTime.UtcNow;
        #endregion


        public static string TokenType
        {
            get
            {
                return AppSettings.GetValueOrDefault(tokenType, stringSettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(tokenType, value);
            }
        }


        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(accessToken, stringSettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(accessToken, value);
            }
        }


        public static bool IsRemenber
        {
            get
            {
                return AppSettings.GetValueOrDefault(isRemenber, boolDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(isRemenber, value);
            }
        }

        public static DateTime ExpiresDate
        {
            get
            {
                return AppSettings.GetValueOrDefault(expiresDate, dateUTC);
            }
            set
            {
                AppSettings.AddOrUpdateValue(expiresDate, value);
            }
        }

    }
}
