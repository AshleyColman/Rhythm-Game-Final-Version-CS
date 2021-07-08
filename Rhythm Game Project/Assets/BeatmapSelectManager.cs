namespace Menu
{
    using System.IO;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.UI.Extensions;
    using File;
    using System.Collections.Generic;
    using Grade;
    using TMPro;
    using Enums;
    using Settings;
    using Audio;
    using Background;
    using SceneLoading;
    using Player;
    using ImageLoad;

    public sealed class BeatmapSelectManager : MonoBehaviour, IMenu
    {
        #region Private Fields
        private int currentBeatmapPreviewIndex = -1;
        private int imagesUpdated = 0;

        private bool buttonListInstantiated = false;

        [SerializeField] private GameObject beatmapSelectScreen = default;

        [SerializeField] private BeatmapButton beatmapButton = default;
        private List<BeatmapButton> beatmapButtonList = new List<BeatmapButton>();

        [SerializeField] private ScrollRect scrollRect = default;

        [SerializeField] private UI_ScrollRectOcclusion scrollRectOcclusion = default;

        [SerializeField] private Transform contentPanel = default;

        [SerializeField] private SongSlider songSlider = default;

        private IEnumerator instantiateBeatmapButtonsCoroutine;
        private IEnumerator checkMenuInputCoroutine;
        private IEnumerator previewFirstButtonCoroutine;
        private IEnumerator transitionInCoroutine;
        private IEnumerator loadBeatmapPreviewCoroutine;
        private IEnumerator enableScrollRectCoroutine;

        private Transition transition;
        private ImageLoader imageLoader;
        private FileManager fileManager;
        private GradeData gradeData;
        private MenuAudioManager menuAudioManager;
        private Notification notification;
        private BackgroundManager backgroundManager;
        private MenuTimeManager menuTimeManager;
        #endregion

        #region Properties
        public int CurrentBeatmapPreviewIndex => currentBeatmapPreviewIndex;
        public List<BeatmapButton> BeatmapButtonList => beatmapButtonList;
        #endregion

        #region Public Methods
        public void TransitionIn()
        {
            if (transitionInCoroutine != null)
            {
                StopCoroutine(transitionInCoroutine);
            }

            transitionInCoroutine = TransitionInCoroutine();
            StartCoroutine(transitionInCoroutine);
        }

        public void TransitionOut()
        {
            beatmapSelectScreen.gameObject.SetActive(false);
        }

        public void OnTick()
        {

        }

        public void OnMeasure()
        {

        }

        public void OpenDirectory()
        {
            Application.OpenURL(fileManager.BeatmapDirectoryPath);
        }

        public void LoadBeatmapPreview(int _beatmapButtonIndex, float _audioStartTime, Texture _imageTexture)
        {
            if (loadBeatmapPreviewCoroutine != null)
            {
                StopCoroutine(loadBeatmapPreviewCoroutine);
            }

            loadBeatmapPreviewCoroutine = LoadBeatmapPreviewCoroutine(_beatmapButtonIndex, _audioStartTime, _imageTexture);
            StartCoroutine(loadBeatmapPreviewCoroutine);
        }

        public void OverviewLoadBeatmapPreview(int _beatmapButtonIndex, float _audioStartTime, Texture _imageTexture)
        {
            if (loadBeatmapPreviewCoroutine != null)
            {
                StopCoroutine(loadBeatmapPreviewCoroutine);
            }

            loadBeatmapPreviewCoroutine = LoadBeatmapPreviewCoroutine(_beatmapButtonIndex, _audioStartTime, _imageTexture);

            StartCoroutine(loadBeatmapPreviewCoroutine);
        }

        public void PreviewPreviousBeatmapButton()
        {
            if (beatmapButtonList.Count != 0)
            {
                if ((currentBeatmapPreviewIndex - 1) < 0)
                {
                    beatmapButtonList[beatmapButtonList.Count - 1].Button_OnHover();
                }
                else
                {
                    beatmapButtonList[currentBeatmapPreviewIndex - 1].Button_OnHover();
                }
            }
        }

        public void PreviewNextBeatmapButton()
        {
            if (beatmapButtonList.Count != 0)
            {
                if ((currentBeatmapPreviewIndex + 1) < beatmapButtonList.Count)
                {
                    beatmapButtonList[currentBeatmapPreviewIndex + 1].Button_OnHover();
                }
                else
                {
                    beatmapButtonList[0].Button_OnHover();
                }
            }
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            transition = FindObjectOfType<Transition>();
            imageLoader = FindObjectOfType<ImageLoader>();
            fileManager = FindObjectOfType<FileManager>();
            gradeData = FindObjectOfType<GradeData>();
            menuAudioManager = FindObjectOfType<MenuAudioManager>();
            notification = FindObjectOfType<Notification>();
            backgroundManager = FindObjectOfType<BackgroundManager>();
            menuTimeManager = FindObjectOfType<MenuTimeManager>();
        }
            
        private IEnumerator TransitionInCoroutine()
        {
            transition.PlayFadeInTween();
            InstantiateBeatmapButtons();
            yield return new WaitForSeconds(Transition.TransitionDuration);
            CheckModeInput();
            beatmapSelectScreen.gameObject.SetActive(true);
            yield return null;
        }

        private IEnumerator LoadBeatmapPreviewCoroutine(int _beatmapButtonIndex, float _audioStartTime, Texture _imageTexture)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(MenuAudioManager.AudioClipLoadDelayDuration);

            currentBeatmapPreviewIndex = _beatmapButtonIndex;
            backgroundManager.TransitionAndLoadNewImage(_imageTexture);
            SetSongSlider(_beatmapButtonIndex);
            menuAudioManager.LoadSongAudioClipFromFile($"{fileManager.BeatmapDirectories[_beatmapButtonIndex]}", _audioStartTime,
                menuTimeManager, songSlider);
            yield return null;
        }

        private IEnumerator InstantiateBeatmapButtonsCoroutine()
        {
            WaitForSeconds instantiationDelay = new WaitForSeconds(0.1f);
            WaitForSeconds activationDelay = new WaitForSeconds(1f);

            if (buttonListInstantiated == false)
            {
                for (int i = 0; i < fileManager.BeatmapDirectories.Length; i++)
                {
                    BeatmapButton newBeatmapButton = Instantiate(beatmapButton, contentPanel);
                    beatmapButtonList.Add(newBeatmapButton);
                    LoadNewBeatmapButtonImage(i);
                    LoadDefaultBeatmapInformation(i);
                    LoadDifficultyInformation(i);

                    yield return instantiationDelay;
                }

                buttonListInstantiated = true;

                yield return activationDelay;

                ActivateAllButtons();
            }

            yield return null;
        }

        private void InstantiateBeatmapButtons()
        {
            if (instantiateBeatmapButtonsCoroutine != null)
            {
                StopCoroutine(instantiateBeatmapButtonsCoroutine);
            }

            instantiateBeatmapButtonsCoroutine = InstantiateBeatmapButtonsCoroutine();
            StartCoroutine(instantiateBeatmapButtonsCoroutine);
        }

        private void LoadNewBeatmapButtonImage(int _index)
        {
            imageLoader.LoadCompressedImageFile(GetBeatmapImagePath(_index), beatmapButtonList[_index].BeatmapImage, 
                IncrementTotalImagesLoaded);
        }

        private string GetBeatmapImagePath(int _index)
        {
            return $"{fileManager.BeatmapDirectories[_index]}/{FileTypes.ImageFileType}";
        }

        private void IncrementTotalImagesLoaded()
        {
            imagesUpdated++;
        }

        private void LoadDefaultBeatmapInformation(int _index)
        {
            beatmapButtonList[_index].SetBeatmapInformation(_index, fileManager.Beatmap.SongName, fileManager.Beatmap.ArtistName,
            fileManager.Beatmap.CreatorName, fileManager.Beatmap.CreatedDate, fileManager.Beatmap.AudioStartTime);
        }

        private void LoadDifficultyInformation(int _index)
        {
            string twoKeyFilePath = $"{fileManager.BeatmapDirectories[_index]}/{FileTypes.TwoKeyFileType}";
            string fourKeyFilePath = $"{fileManager.BeatmapDirectories[_index]}/{FileTypes.FourKeyFileType}";
            string sixKeyFilePath = $"{fileManager.BeatmapDirectories[_index]}/{FileTypes.SixKeyFileType}";

            LoadDifficultyFile(_index, Difficulty.TwoKey, twoKeyFilePath);
            LoadDifficultyFile(_index, Difficulty.FourKey, fourKeyFilePath);
            LoadDifficultyFile(_index, Difficulty.SixKey, sixKeyFilePath);

            beatmapButtonList[_index].CalculateDifficultyMasteryAccuracy();
        }

        private void LoadDifficultyFile(int _index, Difficulty _difficulty, string _filePath)
        {
            if (File.Exists(_filePath))
            {
                fileManager.Load(_filePath);

                SetDifficultyButtonInformation(_index, _difficulty);
            }
            else
            {
                beatmapButtonList[_index].SetDifficultyGradeFalse(_difficulty);
            }
        }

        private void SetDifficultyButtonInformation(int _index, Difficulty _difficulty)
        {
            TMP_ColorGradient gradeColor = gradeData.GetCurrentGradeGradient(fileManager.Beatmap.PlayerDifficultyGrade);

            // Check if the player name saved on the beatmap matches the current signed in user to confirm grade achieved.
            if (Player.Username == fileManager.Beatmap.PlayerDifficultyGradeUsername)
            {
                beatmapButtonList[_index].SetDifficultyGradeTrue(_difficulty, fileManager.Beatmap.PlayerDifficultyGrade,
                    fileManager.Beatmap.DifficultyAccuracy, gradeColor);
            }
            else
            {
                beatmapButtonList[_index].SetDifficultyGradeFalse(_difficulty);
            }
        }

        private void ActivateAllButtons()
        {
            for (int i = 0; i < beatmapButtonList.Count; i++)
            {
                beatmapButtonList[i].gameObject.SetActive(true);
            }

            EnableScrollRect();
            PreviewFirstButton();
            DisplayBeatmapCountNotification();
        }

        private void SetCurrentBeatmapPreviewIndex(int _index)
        {
            currentBeatmapPreviewIndex = _index;
        }

        private void EnableScrollRect()
        {
            if (enableScrollRectCoroutine != null)
            {
                StopCoroutine(enableScrollRectCoroutine);
            }

            enableScrollRectCoroutine = EnableScrollRectCoroutine();
            StartCoroutine(enableScrollRectCoroutine);
        }

        private IEnumerator EnableScrollRectCoroutine()
        {
            yield return new WaitForEndOfFrame();
            scrollRectOcclusion.Init();
            yield return new WaitForEndOfFrame();
            scrollRect.ScrollToTop();
            yield return null;
        }

        private void PreviewFirstButton()
        {
            if (previewFirstButtonCoroutine != null)
            {
                StopCoroutine(previewFirstButtonCoroutine);
            }

            previewFirstButtonCoroutine = PreviewFirstButtonCoroutine();
            StartCoroutine(previewFirstButtonCoroutine);
        }

        private IEnumerator PreviewFirstButtonCoroutine()
        {
            yield return new WaitForSeconds(0.5f);

            if (beatmapButtonList.Count != 0)
            {
                beatmapButtonList[0].Button_OnHover();
            }

            yield return null;
        }

        private void DisplayBeatmapCountNotification()
        {
            notification.DisplayTitleNotification(ColorName.LIGHT_BLUE, $"{fileManager.BeatmapDirectories.Length} beatmaps found",
                4f);
        }

        private void SetSongSlider(int _beatmapButtonIndex)
        {
            RepositionSongSlider(_beatmapButtonIndex);
        }

        private void RepositionSongSlider(int _beatmapButtonIndex)
        {
            Vector3 pos = new Vector3(196f, 0f, 0f);
            songSlider.SongTimeSliderCachedTransform.SetParent(beatmapButtonList[_beatmapButtonIndex].transform, true);
            songSlider.SongTimeSliderCachedTransform.SetSiblingIndex(1);
            songSlider.SongTimeSliderCachedTransform.localPosition = pos;
        }

        private void CheckModeInput()
        {
            if (checkMenuInputCoroutine != null)
            {
                StopCoroutine(checkMenuInputCoroutine);
            }

            checkMenuInputCoroutine = CheckMenuInputCoroutine();
            StartCoroutine(checkMenuInputCoroutine);
        }

        private IEnumerator CheckMenuInputCoroutine()
        {
            while (beatmapSelectScreen.gameObject.activeSelf == true)
            {
                if (Input.anyKey)
                {
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        PreviewNextBeatmapButton();
                    }

                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        PreviewPreviousBeatmapButton();
                    }
                }

                yield return null;
            }
            yield return null;
        }
        #endregion
    }
}