namespace ImageLoad
{
    using System.IO;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Networking;
    using UnityEngine.UI;
    using Loading;
    using Enums;
    using Menu;

    public sealed class ImageLoader : MonoBehaviour
    {
        #region Delegates
        public delegate void UpdatedImageIncrementer();
        private UpdatedImageIncrementer updatedImageIncrementer;
        #endregion

        #region Constants
        private const string ImageShaderLocation = "UI/Unlit/Transparent";
        #endregion

        #region Private Fields
        [SerializeField] private Material defaultMaterial = default;

        private Shader imageShader;

        private Notification notification;
        #endregion

        #region Public Methods
        public void LoadCompressedMaterialUrl(string _url, Material _material)
        {
            StartCoroutine(LoadCompressedMaterialUrlCoroutine(_url, _material));
        }

        public void LoadCompressedImageUrl(string _url, Image _image)
        {
            StartCoroutine(LoadCompressedImageUrlCoroutine(_url, _image));
        }

        public void LoadCompressedImageUrl(string _url, Image _image, UpdatedImageIncrementer _updatedImageIncrementer)
        {
            StartCoroutine(LoadCompressedImageUrlCoroutine(_url, _image, _updatedImageIncrementer));
        }

        public void LoadCompressedImageFile(string _url, Image _image)
        {
            StartCoroutine(LoadCompressedImageFileCoroutine(_url, _image));
        }

        public void LoadCompressedImageFile(string _url, Image _image, UpdatedImageIncrementer _updatedImageIncrementer)
        {
            StartCoroutine(LoadCompressedImageFileCoroutine(_url, _image, _updatedImageIncrementer));
        }

        public void LoadCompressedImageFile(string _url, Image _image, LoadingIcon _loadingIcon)
        {
            StartCoroutine(LoadCompressedImageFileCoroutine(_url, _image, _loadingIcon));
        }

        public Material CreateMaterialForImage()
        {
            Material material = new Material(imageShader);
            return material;
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            notification = FindObjectOfType<Notification>();
            ReferenceImageShader();
        }

        private void ReferenceImageShader()
        {
            imageShader = Shader.Find(ImageShaderLocation);
        }

        private IEnumerator LoadCompressedMaterialUrlCoroutine(string _url, Material _material)
        {
            switch (_url)
            {
                case "":
                    SetToDefaultMaterial(_material);
                    break;
                case null:
                    SetToDefaultMaterial(_material);
                    break;
                default:
                    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url))
                    {
                        yield return uwr.SendWebRequest();

                        if (uwr.isNetworkError == false)
                        {
                            Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(uwr);
                            SetPerformance(downloadedTexture);
                            ApplyMaterial(_material, downloadedTexture);
                        }
                    }
                    break;
            }
            yield return null;
        }

        private IEnumerator LoadCompressedImageUrlCoroutine(string _url, Image _image)
        {
            switch (_url)
            {
                case "":
                    SetToDefaultMaterial(_image);
                    break;
                case null:
                    SetToDefaultMaterial(_image);
                    break;
                default:
                    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url))
                    {
                        yield return uwr.SendWebRequest();

                        if (uwr.isNetworkError == false)
                        {
                            Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(uwr);
                            SetPerformance(downloadedTexture);
                            ApplyMaterial(_image, downloadedTexture);
                        }
                    }
                    break;
            }
            yield return null;
        }

        private IEnumerator LoadCompressedImageUrlCoroutine(string _url, Image _image, UpdatedImageIncrementer 
            _updatedImageIncrementer)
        {
            switch (_url)
            {
                case "":
                    SetToDefaultMaterial(_image);
                    _updatedImageIncrementer();
                    break;
                case null:
                    SetToDefaultMaterial(_image);
                    _updatedImageIncrementer();
                    break;
                default:
                    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url))
                    {
                        yield return uwr.SendWebRequest();

                        if (uwr.isNetworkError || uwr.isHttpError)
                        {
                            SetToDefaultMaterial(_image);
                            _updatedImageIncrementer();
                        }
                        else
                        {
                            Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(uwr);
                            SetPerformance(downloadedTexture);
                            ApplyMaterial(_image, downloadedTexture);
                            _updatedImageIncrementer();
                        }
                    }
                    break;
            }
            yield return null;
        }

        private IEnumerator LoadCompressedImageFileCoroutine(string _url, Image _image)
        {
            switch (File.Exists(_url))
            {
                case false:
                    SetToDefaultMaterial(_image);
                    break;
                case true:
                    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url))
                    {
                        yield return uwr.SendWebRequest();

                        if (uwr.isNetworkError == false)
                        {
                            Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(uwr);
                            SetPerformance(downloadedTexture);
                            ApplyMaterial(_image, downloadedTexture);
                        }
                    }
                    break;
            }
            yield return null;
        }

        private IEnumerator LoadCompressedImageFileCoroutine(string _url, Image _image, UpdatedImageIncrementer
            _updatedImageIncrementer)
        {
            switch (File.Exists(_url))
            {
                case false:
                    SetToDefaultMaterial(_image);
                    _updatedImageIncrementer();
                    break;
                case true:
                    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url))
                    {
                        yield return uwr.SendWebRequest();

                        if (uwr.isNetworkError)
                        {
                            SetToDefaultMaterial(_image);
                            _updatedImageIncrementer();
                        }
                        else
                        {
                            Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(uwr);
                            SetPerformance(downloadedTexture);
                            ApplyMaterial(_image, downloadedTexture);
                            _updatedImageIncrementer();
                        }
                    }
                    break;
            }
            yield return null;
        }

        private IEnumerator LoadCompressedImageFileCoroutine(string _url, Image _image, LoadingIcon _loadingIcon)
        {
            switch (File.Exists(_url))
            {
                case false:
                    SetToDefaultMaterial(_image);
                    break;
                case true:
                    using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url))
                    {
                        _loadingIcon.DisplayLoadingIcon();
                        yield return uwr.SendWebRequest();

                        if (uwr.isNetworkError)
                        {
                            notification.DisplayDescriptionNotification(ColorName.RED, "error loading image", _url, 4f);
                        }
                        else
                        {
                            Texture2D downloadedTexture = DownloadHandlerTexture.GetContent(uwr);
                            SetPerformance(downloadedTexture);
                            ApplyMaterial(_image, downloadedTexture);
                        }

                        _loadingIcon.HideLoadingIcon();
                    }
                    break;
            }
            yield return null;
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
            _image.material = new Material(imageShader);
            _image.material.mainTexture = _texture;
        }

        private void ApplyMaterial(Material _material, Texture2D _texture)
        {
            _material.mainTexture = _texture;
        }

        private void SetToDefaultMaterial(Image _image)
        {
            _image.material = defaultMaterial;
        }

        private void SetToDefaultMaterial(Material _material)
        {
            _material = defaultMaterial;
        }
        #endregion
    }
}