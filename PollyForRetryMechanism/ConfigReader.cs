using System;
using System.Configuration;

namespace PollyForRetryMechanism
{
    public static class ConfigReader
    {
        /// <summary>
        ///     Gets the value of the passed in key from the app.config file.
        /// </summary>
        /// <typeparam name="T">type of param</typeparam>
        /// <param name="key">value of the key in app.config,</param>
        /// <returns>value of the key passed as input param.</returns>
        public static string GetSettings(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return ConfigurationSettings.AppSettings[key].ToString();
            }
            else
            {
                throw new InvalidOperationException("Please pass a non null key value in the method call.");
            }
        }
    }

    public static class ConfigKeyConstants
    {
        internal const string ValidUrl = "ValidApiUrl";

        internal const string InValidUrl = "InvalidApiUrl";
    }
}
