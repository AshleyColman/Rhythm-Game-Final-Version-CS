namespace Menu
{
    using System.Collections;
    using System.Text.RegularExpressions;
    using UnityEngine;
    using UnityEngine.Networking;
    using UnityEngine.UI;
    using UnityEngine.UI.Extensions;
    using TMPro;
    using Loading;
    using Profile;
    using Grade;
    using File;
    using ImageLoad;
    using Enums;
    using Player;

    public sealed class Leaderboard : MonoBehaviour
    {
        #region Constants
        private const int Placements = 10;
        private const int PlayernameUrlIndex = 0;
        private const int ScoreUrlIndex = 1;
        private const int AccuracyUrlIndex = 2;
        private const int ComboUrlIndex = 3;
        private const int DateUrlIndex = 4;
        private const int ClearPointsUrlIndex = 5;
        private const int HiddenPointsUrlIndex = 6;
        private const int MinePointsUrlIndex = 7;
        private const int LowApproachRatePointsUrlIndex = 8;
        private const int HighApproachRatePointsUrlIndex = 9;
        private const int FullComboPointsUrlIndex = 10;
        private const int MaxPercentagePointsUrlIndex = 11;
        private const int ImageUrlIndex = 12;
        private const int LevelUrlIndex = 13;
        private const int PersonalBestScoreUrlIndex = 0;
        private const int PersonalBestAccuracyUrlIndex = 1;
        private const int PersonalBestComboUrlIndex = 2;
        private const int PersonalBestPerfectUrlIndex = 3;
        private const int PersonalBestGreatUrlIndex = 4;
        private const int PersonalBestOkayUrlIndex = 5;
        private const int PersonalBestMissUrlIndex = 6;
        private const int PersonalBestFeverUrlIndex = 7;
        private const int PersonalBestBonusUrlIndex = 8;
        private const int PersonalBestDateUrlIndex = 9;
        private const int PersonalBestClearPointsUrlIndex = 10;
        private const int PersonalBestHiddenPointsUrlIndex = 11;
        private const int PersonalBestMinePointsUrlIndex = 12;
        private const int PersonalBestLowApproachRatePointsUrlIndex = 13;
        private const int PersonalBestHighApproachRatePointsUrlIndex = 14;
        private const int PersonalBestFullComboPointsUrlIndex = 15;
        private const int PersonalBestMaxPercentagePointsUrlIndex = 16;
        private const int PersonalBestPlacementUrlIndex = 17;
        private const int PersonalBestTotalRecordsUrlIndex = 18;
        private const int CategoryScoreIndex = 0;
        private const int CatagoryAccuracyIndex = 1;
        private const int CatagoryComboIndex = 2;
        private const int ViewGlobalIndex = 0;
        private const int ViewLocalIndex = 1;

        private const string RegixSplit = "->";
        private const string UrlErrorCode = "error";
        private const string FieldDatabaseTable = "databaseTable";
        private const string FieldUsername = "username";
        private const string FieldRankedBeatmapID = "id";
        private const string FieldIndex = "index";
        private const string FieldCategorySorting = "categorySorting";
        private const string RetrieveButtonInformationUrl = "http://localhost/RhythmGameOnlineScripts/RetrievePlayerBeatmapRankingScript.php";
        private const string RetrievePersonalBestInformationUrl = "http://localhost/RhythmGameOnlineScripts/RetrievePersonalBestRankingScript.php";
        private const string TestTableName = "testtable";
        #endregion

        #region Private Fields
        [SerializeField] private int imageUpdatedCount = 0;

        [SerializeField] private bool hasLoadedPersonalBest = false;

        [SerializeField] private string[][] placementDataArr;
        [SerializeField] private string[] personalBestPlacementDataArr;

        [SerializeField] private GameObject leaderboard = default;

        [SerializeField] private Transform contentTransform = default;

        [SerializeField] private Scrollbar scrollbar = default;

        [SerializeField] private TMP_Dropdown categorySortDropdown = default;
        [SerializeField] private TMP_Dropdown viewSortDropdown = default;

        [SerializeField] private LeaderboardButton buttonPrefab = default;
        [SerializeField] private PersonalBestLeaderboardButton personalBestButton = default;
        private LeaderboardButton[] buttonArr;

        [SerializeField] private LoadingIcon loadingIcon = default;

        [SerializeField] private UI_ScrollRectOcclusion scrollRectOcclusion = default;

        private IEnumerator requestLeaderboard;
        private IEnumerator displayAllButtons;

        private GradeData gradedata;
        private ImageLoader imageLoader;
        private Notification notification;
        private FileManager fileManager;
        #endregion

        #region Public Methods
        public void RequestLeaderboard()
        {
            ResetLeaderboard();

            if (requestLeaderboard != null)
            {
                StopCoroutine(requestLeaderboard);
            }

            requestLeaderboard = RequestLeaderboardCoroutine();
            StartCoroutine(requestLeaderboard);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            gradedata = FindObjectOfType<GradeData>();
            imageLoader = FindObjectOfType<ImageLoader>();
            notification = FindObjectOfType<Notification>();
            fileManager = FindObjectOfType<FileManager>();
        }

        private void Start()
        {
            SetupLeaderboard();
            HideLeaderboard();
        }

        private void SetupLeaderboard()
        {
            buttonArr = new LeaderboardButton[Placements];
            placementDataArr = new string[Placements][];

            for (int i = 0; i < Placements; i++)
            {
                LeaderboardButton instantiateButton = Instantiate(buttonPrefab, contentTransform);
                buttonArr[i] = instantiateButton;

                instantiateButton.SetPositionText($"#{i + 1}");
                instantiateButton.SetNewProfileImageMaterial(imageLoader.CreateMaterialForImage());
            }
        }

        private void ResetLeaderboard()
        {
            StopAllCoroutines();
            ResetReferencedScripts();
            HideLeaderboard();
            ResetLeaderboardButtonLevelSliders();
            ResetVariables();
            loadingIcon.DisplayLoadingIcon();
        }

        private void ResetReferencedScripts()
        {
            gradedata.StopAllCoroutines();
            imageLoader.StopAllCoroutines();
            notification.StopTransitionOutCoroutine();
            fileManager.StopAllCoroutines();
            scrollRectOcclusion.StopAllCoroutines();
            personalBestButton.StopAllCoroutines();
            loadingIcon.StopAllCoroutines();

            for (byte i = 0; i < buttonArr.Length; i++)
            {
                buttonArr[i].StopAllCoroutines();
            }
        }

        private void ResetVariables()
        {
            imageUpdatedCount = default;
            hasLoadedPersonalBest = false;
        }

        private void ResetLeaderboardButtonLevelSliders()
        {
            for (byte i = 0; i < buttonArr.Length; i++)
            {
                buttonArr[i].ResetLevelSliderValue();
            }
        }

        private IEnumerator RequestLeaderboardCoroutine()
        {
            for (int i = 0; i < Placements; i++)
            {
                RetrieveButtonInformation(i);
                yield return new WaitForSeconds(0.2f);
            }

            switch (Player.LoggedIn)
            {
                case true:
                    RetrievePersonalBestInformation();
                    break;
                case false:
                    DisplayPersonalBestNoRecord();
                    break;
            }

            yield return null;
        }

        private void RetrieveButtonInformation(int _index)
        {
            StartCoroutine(RetrieveButtonInformationCoroutine(_index));
        }

        private IEnumerator RetrieveButtonInformationCoroutine(int _index)
        {
            WWWForm form = new WWWForm();
            //form.AddField(FIELD_DATABASE_TABLE, FileManager.beatmap.DatabaseTable);
            form.AddField(FieldDatabaseTable, TestTableName);
            form.AddField(FieldIndex, _index);
            form.AddField(FieldCategorySorting, categorySortDropdown.value);

            UnityWebRequest www = UnityWebRequest.Post(RetrieveButtonInformationUrl, form);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError || www.downloadHandler.text == UrlErrorCode)
            {
                DisplayNoRecord(_index);
                IncrementTotalImagesUpdated();
            }
            else
            {
                // Split the information retrieved from the database into array cells.
                placementDataArr[_index] = Regex.Split(www.downloadHandler.text, RegixSplit);

                UpdateButtonVisuals(_index);
            }
        }

        private void RetrievePersonalBestInformation()
        {
            StartCoroutine(RetrievePersonalBestInformationCoroutine());
        }

        private IEnumerator RetrievePersonalBestInformationCoroutine()
        {
            WWWForm form = new WWWForm();
            //form.AddField(FIELD_DATABASE_TABLE, FileManager.beatmap.DatabaseTable);
            form.AddField(FieldDatabaseTable, TestTableName);
            form.AddField(FieldUsername, Player.Username);

            UnityWebRequest www = UnityWebRequest.Post(RetrievePersonalBestInformationUrl, form);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError || www.downloadHandler.text == UrlErrorCode)
            {
                DisplayPersonalBestNoRecord();
            }
            else
            {
                personalBestPlacementDataArr = Regex.Split(www.downloadHandler.text, RegixSplit);
                UpdatePersonalBestButtonVisuals();
                CheckIfReadyToDisplayLeaderboard();
            }
        }

        private void DisplayAllButtons()
        {
            if (displayAllButtons != null)
            {
                StopCoroutine(displayAllButtons);
            }

            displayAllButtons = DisplayAllButtonsCoroutine();
            StartCoroutine(displayAllButtons);
        }

        private IEnumerator DisplayAllButtonsCoroutine()
        {
            for (int i = 0; i < buttonArr.Length; i++)
            {
                buttonArr[i].gameObject.SetActive(true);
            }

            personalBestButton.DisplayPersonalBest();
            yield return new WaitForEndOfFrame();

            // Initialize scroll rect occlusion attached now that all buttons are instantiated.
            scrollRectOcclusion.Init();
            // Set scroll list to top.
            scrollbar.value = 1f;
            // Reset position to occlude off screen buttons.
            scrollRectOcclusion.ScrollToDefault();

            // Lerps button text and slider.
            PlayButtonAnimation();

            yield return null;
        }

        private void PlayButtonAnimation()
        {
            for (int i = 0; i < buttonArr.Length; i++)
            {
                if (buttonArr[i].gameObject.activeSelf == true)
                {
                    buttonArr[i].PlayFlashAnimationForAchievedButtons();

                    if (placementDataArr[i] != null)
                    {
                        buttonArr[i].PlayLerpAnimation(
                        float.Parse(placementDataArr[i][ScoreUrlIndex]),
                        float.Parse(placementDataArr[i][AccuracyUrlIndex]),
                        float.Parse(placementDataArr[i][ComboUrlIndex]),
                        float.Parse(placementDataArr[i][LevelUrlIndex])
                        );
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void IncrementTotalImagesUpdated()
        {
            imageUpdatedCount++;
            CheckIfReadyToDisplayLeaderboard();
        }

        private void CheckIfReadyToDisplayLeaderboard()
        {
            switch (hasLoadedPersonalBest)
            {
                case true:
                    switch (imageUpdatedCount)
                    {
                        case Placements:
                            DisplayLeaderboard();
                            break;
                    }
                    break;
            }
        }

        private void UpdatePersonalBestButtonVisuals()
        {
            UpdatePersonalBestChallengeButtonPanel();
            UpdatePersonalBestButtonText();
        }

        private void UpdatePersonalBestChallengeButtonPanel()
        {
            personalBestButton.SetChallengeButtonPanelVisual(
                personalBestPlacementDataArr[PersonalBestClearPointsUrlIndex],
                 personalBestPlacementDataArr[PersonalBestHiddenPointsUrlIndex],
                  personalBestPlacementDataArr[PersonalBestMinePointsUrlIndex],
                   personalBestPlacementDataArr[PersonalBestLowApproachRatePointsUrlIndex],
                    personalBestPlacementDataArr[PersonalBestHighApproachRatePointsUrlIndex],
                     personalBestPlacementDataArr[PersonalBestFullComboPointsUrlIndex],
                      personalBestPlacementDataArr[PersonalBestMaxPercentagePointsUrlIndex]
                );
        }

        private void UpdatePersonalBestButtonText()
        {
            // Index's are offset by 1 as the php script does not need to return a name.
            personalBestButton.SetButtonText(
                $"{personalBestPlacementDataArr[PersonalBestScoreUrlIndex]}",
                $"{personalBestPlacementDataArr[PersonalBestComboUrlIndex]}x",
                $"{personalBestPlacementDataArr[PersonalBestPerfectUrlIndex]}",
                $"{personalBestPlacementDataArr[PersonalBestGreatUrlIndex]}",
                $"{personalBestPlacementDataArr[PersonalBestOkayUrlIndex]}",
                $"{personalBestPlacementDataArr[PersonalBestMissUrlIndex]}",
                $"{personalBestPlacementDataArr[PersonalBestFeverUrlIndex]}",
                $"{personalBestPlacementDataArr[PersonalBestBonusUrlIndex]}",
                $"#{personalBestPlacementDataArr[PersonalBestPlacementUrlIndex]} of " +
                $"{personalBestPlacementDataArr[PersonalBestTotalRecordsUrlIndex]}"
                );

            //TextMeshProUGUI gradeText = gradedata.GetGradeText(float.Parse(personalBestPlacementDataArr
            //    [PersonalBestAccuracyUrlIndex]));
            //personalBestButton.SetGradeText(gradeText.text, gradeText.colorGradientPreset);

            hasLoadedPersonalBest = true;
        }

        private void DisplayPersonalBestNoRecord()
        {
            personalBestButton.SetButtonTextNoRecord();
            hasLoadedPersonalBest = true;
            CheckIfReadyToDisplayLeaderboard();
        }

        private void UpdateButtonVisuals(int _index)
        {
            UpdateButtonText(_index);

            imageLoader.LoadCompressedImageUrl(placementDataArr[_index][ImageUrlIndex], buttonArr[_index].ProfileImage,
                IncrementTotalImagesUpdated);

            buttonArr[_index].SetChallengeButtonPanelVisual(
                placementDataArr[_index][ClearPointsUrlIndex],
                placementDataArr[_index][HiddenPointsUrlIndex],
                placementDataArr[_index][MinePointsUrlIndex],
                placementDataArr[_index][LowApproachRatePointsUrlIndex],
                placementDataArr[_index][HighApproachRatePointsUrlIndex],
                placementDataArr[_index][FullComboPointsUrlIndex],
                placementDataArr[_index][MaxPercentagePointsUrlIndex]
                );
        }

        private void UpdateButtonText(int _index)
        {
            buttonArr[_index].ActivateContentPanel();
            buttonArr[_index].DeactivateNoRecordSetText();

            buttonArr[_index].SetNameText(placementDataArr[_index][PlayernameUrlIndex]);
            buttonArr[_index].SetComboAndAccuracyText($"{placementDataArr[_index][ComboUrlIndex]}  " +
                $"{placementDataArr[_index][AccuracyUrlIndex]}%");
            buttonArr[_index].SetLevel(placementDataArr[_index][LevelUrlIndex]);
            buttonArr[_index].SetDateText(UtilityMethods.GetTimeSinceDateInput(placementDataArr[_index][DateUrlIndex]));
            buttonArr[_index].SetScoreText(placementDataArr[_index][ScoreUrlIndex]);

            TextMeshProUGUI gradeText = gradedata.GetGradeText(float.Parse(placementDataArr[_index][AccuracyUrlIndex]));
            buttonArr[_index].SetGradeText(gradeText.text, gradeText.colorGradientPreset);
        }

        private void DisplayNoRecord(int _index)
        {
            buttonArr[_index].DeactivateContentPanel();
            buttonArr[_index].ActivateNoRecordText();
        }

        private void DisplayLeaderboard()
        {
            switch (leaderboard.gameObject.activeSelf)
            {
                case false:
                    leaderboard.gameObject.SetActive(true);
                    break;
            }

            DisplayAllButtons();
            loadingIcon.HideLoadingIcon();
        }

        private void HideAllButtons()
        {
            for (int i = 0; i < buttonArr.Length; i++)
            {
                switch (buttonArr[i].gameObject.activeSelf)
                {
                    case true:
                        buttonArr[i].gameObject.SetActive(false);
                        break;
                }
            }

            personalBestButton.DeactivatePersonalBest();
        }

        private void HideLeaderboard()
        {
            switch (leaderboard.gameObject.activeSelf)
            {
                case true:
                    leaderboard.gameObject.SetActive(false);
                    break;
            }

            HideAllButtons();
        }

        // Is called when the category dropdown value has been changed.
        public void CategoryDropdownValueChange()
        {
            switch (categorySortDropdown.value)
            {
                case CategoryScoreIndex:
                    notification.DisplayDescriptionNotification(ColorName.PURPLE, "score", "displaying top score plays", 4f);
                    break;
                case CatagoryAccuracyIndex:
                    notification.DisplayDescriptionNotification(ColorName.PURPLE, "accuracy", "displaying top accuracy plays", 4f);
                    break;
                case CatagoryComboIndex:
                    notification.DisplayDescriptionNotification(ColorName.PURPLE, "combo", "displaying top combo plays", 4f);
                    break;
            }

            RequestLeaderboard();
        }

        // Is called when the view dropdown value has been changed.
        public void ViewDropdownValueChange()
        {
            viewSortDropdown.value = ViewGlobalIndex;

            notification.DisplayDescriptionNotification(ColorName.RED, "global",
                "showing top global plays, local scores coming in a future update",
                4f);
        }
        #endregion
    }
}