namespace Player
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.Networking;
    using System.Text.RegularExpressions;
    using ImageLoad;

    public sealed class Player : MonoBehaviour
    {
        #region Constants
        private const string UrlErrorCode = "error";
        private const string FieldUsername = "username";
        private const string RegixSplit = "->";
        private const string RetrievePlayerProfileImageUrlScript =
            "http://localhost/RhythmGameOnlineScripts/RetrievePlayerProfileImageUrlScript.php";
        #endregion

        #region Private Fields
        [SerializeField] private Material profileImageMaterial = default;

        private static string username = string.Empty;
        private static byte level = 0;
        private static bool loggedIn = false;

        private ImageLoader imageLoader;
        #endregion

        #region Properties
        public static string Username => username;
        public static bool LoggedIn => loggedIn;
        #endregion

        #region Public Methods
        public void LoginWithAccount(string _username)
        {
            username = _username;
            loggedIn = true;
            RetrieveProfileImageUrl();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            imageLoader = FindObjectOfType<ImageLoader>();
        }

        private void LoadProfileImageMaterial(string _url)
        {
            imageLoader.LoadCompressedMaterialUrl(_url, profileImageMaterial);
        }

        private void RetrieveProfileImageUrl()
        {
            StartCoroutine(RetrieveProfileImageUrlCoroutine());
        }

        private IEnumerator RetrieveProfileImageUrlCoroutine()
        {
            WWWForm form = new WWWForm();
            form.AddField(FieldUsername, Username);
            UnityWebRequest www = UnityWebRequest.Post(RetrievePlayerProfileImageUrlScript, form);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError || www.downloadHandler.text == UrlErrorCode)
            {

            }
            else
            {
                string[] dataArr = Regex.Split(www.downloadHandler.text, RegixSplit);
                byte imageUrlIndex = 0;
                LoadProfileImageMaterial(dataArr[imageUrlIndex]);
            }
        }
        #endregion
    }
}