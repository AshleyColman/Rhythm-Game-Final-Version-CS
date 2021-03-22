namespace Menu
{
    using System.IO;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Networking;
    using UnityEngine.UI;
    using Loading;
    using Enums;

    public sealed class ImageLoader : MonoBehaviour
    {
        #region Delegates
        public delegate void UpdatedImageIncrementer();
        public UpdatedImageIncrementer updatedImageIncrementerDelegate;
        #endregion

        #region Constants
        private const string ImageShader = "UI/Unlit/Transparent";
        #endregion

        #region Private Fields
        [SerializeField] private Material defaultMaterial = default;

        private Notification notification;
        #endregion

        #region Public Methods
        public void LoadCompressedImageUrl(string _url, Image _image)
        {
            StartCoroutine(LoadCompressedImageUrlCoroutine(_url, _image));
        }

        public void LoadCompressedImageFile(string _url, Image _image)
        {
            StartCoroutine(LoadCompressedImageFileCoroutine(_url, _image));
        }

        public void LoadCompressedImageFile(string _url, Image _image, LoadingIcon _loadingIcon)
        {
            StartCoroutine(LoadCompressedImageFileCoroutine(_url, _image, _loadingIcon));
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            notification = FindObjectOfType<Notification>();
        }

        private IEnumerator LoadCompressedImageUrlCoroutine(string _url, Image _image)
        {
            switch (_url)
            {
                case "":
                    SetToDefaultMaterial(_image);
                    updatedImageIncrementerDelegate();
                    break;
                case null:
                    SetToDefaultMaterial(_image);
                    updatedImageIncrementerDelegate();
                    break;
                default:
                    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url))
                    {
                        yield return uwr.SendWebRequest();

                        switch (uwr.isNetworkError)
                        {
                            case false:
                                Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(uwr);
                                SetPerformance(downloadedTexture);
                                ApplyMaterial(_image, downloadedTexture);
                                updatedImageIncrementerDelegate();
                                break;
                        }
                    }
                    break;
            }
        }

        private IEnumerator LoadCompressedImageFileCoroutine(string _url, Image _image)
        {
            switch (File.Exists(_url))
            {
                case false:
                    SetToDefaultMaterial(_image);
                    updatedImageIncrementerDelegate();
                    break;
                case true:
                    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url))
                    {
                        yield return uwr.SendWebRequest();

                        switch (uwr.isNetworkError)
                        {
                            case false:
                                Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(uwr);
                                SetPerformance(downloadedTexture);
                                ApplyMaterial(_image, downloadedTexture);
                                updatedImageIncrementerDelegate();
                                break;
                        }
                    }
                    break;
            }
        }

        private IEnumerator LoadCompressedImageFileCoroutine(string _url, Image _image, LoadingIcon _loadingIcon)
        {
            switch (File.Exists(_url))
            {
                case false:
                    SetToDefaultMaterial(_image);
                    updatedImageIncrementerDelegate();
                    break;
                case true:
                    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url))
                    {
                        _loadingIcon.DisplayLoadingIcon();
                        yield return uwr.SendWebRequest();

                        switch (uwr.isNetworkError)
                        {
                            case false:
                                Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(uwr);
                                SetPerformance(downloadedTexture);
                                ApplyMaterial(_image, downloadedTexture);
                                updatedImageIncrementerDelegate();
                                break;
                            case true:
                                notification.DisplayNotification(NotificationType.Error, "error loading image", 4f);
                                break;
                        }

                        _loadingIcon.HideLoadingIcon();
                    }
                    break;
            }
        }

        private void SetPerformance(Texture2D _texture)
        {
            _texture.filterMode = FilterMode.Trilinear;
            _texture.wrapMode = TextureWrapMode.Clamp;
            _texture.Compress(true);
            _texture.Apply(true);
        }

        private void ApplyMaterial(Image _image, Texture2D _texture)
        {
            _image.material = new Material(Shader.Find(ImageShader));
            _image.material.mainTexture = _texture;
        }

        private void SetToDefaultMaterial(Image _image)
        {
            _image.material = defaultMaterial;
        }
        #endregion
    }
}