namespace File
{
    using UnityEngine;

    public sealed class FileTypes : MonoBehaviour
    {
        #region Constants
        public const string ImageFileType = "image.png";
        public static readonly string[] AudioFileTypesArray = new string[] { "audio.ogg", "audio.oga" };
        public const string TwoKeyFileType = "TwoKey.bm";
        public const string FourKeyFileType = "FourKey.bm";
        public const string SixKeyFileType = "SixKey.bm";
        #endregion
    }
}