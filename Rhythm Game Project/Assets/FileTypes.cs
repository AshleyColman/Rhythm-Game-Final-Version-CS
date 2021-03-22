namespace File
{
    using UnityEngine;

    public sealed class FileTypes : MonoBehaviour
    {
        #region Constants
        public const string ImageFileType = "image.png";
        public static readonly string[] AudioFileTypesArray = new string[] { "audio.ogg", "audio.oga" };
        public const string EasyFileType = "easy.bm";
        public const string NormalFileType = "normal.bm";
        public const string HardFileType = "hard.bm";
        #endregion
    }
}