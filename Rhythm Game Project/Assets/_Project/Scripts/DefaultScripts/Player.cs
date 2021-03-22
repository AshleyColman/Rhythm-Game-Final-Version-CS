namespace Settings
{
    using UnityEngine;

    public static class Player
    {
        #region Private Fields
        private static string username = "Ashley"; //string.Empty;
        private static uint level = 0;
        #endregion

        #region Properties
        public static string Username => username;
        #endregion

        #region Public Methods
        public static void SetUsername(string _username)
        {
            username = _username;
        }
        #endregion
    }
}