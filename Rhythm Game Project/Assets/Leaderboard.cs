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

    public sealed class Leaderboard : MonoBehaviour
    {
        #region Constants
        private const int Placements = 10;
        private const int PlayernameUrlIndex = 0;
        private const int CategoryUrlIndex = 1;
        private const int AccuracyUrlIndex = 2;
        private const int DateUrlIndex = 3;
        private const int ImageUrlIndex = 4;
        private const int LevelUrlIndex = 5;
        private const int PersonalBestScoreUrlIndex = 0;
        private const int PersonalBestAccuracyUrlIndex = 1;
        private const int PersonalBestDateUrlIndex = 2;
        private const int PersonalBestPerfectUrlIndex = 3;
        private const int PersonalBestGreatUrlIndex = 4;
        private const int PersonalBestOkayUrlIndex = 5;
        private const int PersonalBestMissUrlIndex = 6;
        private const int PersonalBestFeverUrlIndex = 7;
        private const int PersonalBestBonusUrlIndex = 8;
        private const int PersonalBestComboUrlIndex = 9;
        private const int PersonalBestPlacementUrlIndex = 10;
        private const int PersonalBestTotalRecordsUrlIndex = 11;
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
        private const string CategoryScoreNotification = "score";
        private const string CategoryAccuracyNotification = "accuracy";
        private const string CategoryComboNotification = "combo";
        private const string ViewNotification = "local scores not available, please wait for a future update.";
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
        private ColorCollection colorCollection;
        private FileManager fileManager;
        #endregion

        #region Private Methods
        private void Awake()
        {
            gradedata = FindObjectOfType<GradeData>();
            imageLoader = FindObjectOfType<ImageLoader>();
            notification = FindObjectOfType<Notification>();
            colorCollection = FindObjectOfType<ColorCollection>();
            fileManager = FindObjectOfType<FileManager>();
        }

        private void Start()
        {
            SetupLeaderboard();
            HideLeaderboard();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                RequestNewLeaderboard();
            }
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

        private void RequestLeaderboard()
        {
            if (requestLeaderboard != null)
            {
                StopCoroutine(requestLeaderboard);
            }

            requestLeaderboard = RequestLeaderboardCoroutine();
            StartCoroutine(requestLeaderboard);
        }

        private IEnumerator RequestLeaderboardCoroutine()
        {
            for (int i = 0; i < Placements; i++)
            {
                RetrieveButtonInformation(i);
                yield return new WaitForSeconds(0.2f);
            }

            switch (ProfileManager.LoggedIn)
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
                // Update button text.
                UpdateButtonText(_index);
                // Load image.
                imageLoader.LoadCompressedImageUrl(placementDataArr[_index][ImageUrlIndex], buttonArr[_index].ProfileImage,
                    IncrementTotalImagesUpdated);
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
            form.AddField(FieldUsername, ProfileManager.Username);

            UnityWebRequest www = UnityWebRequest.Post(RetrievePersonalBestInformationUrl, form);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError || www.downloadHandler.text == UrlErrorCode)
            {
                DisplayPersonalBestNoRecord();
            }
            else
            {
                personalBestPlacementDataArr = Regex.Split(www.downloadHandler.text, RegixSplit);
                UpdatePersonalBestButtonText();
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

            personalBestButton.gameObject.SetActive(true);
            yield return new WaitForEndOfFrame();

            // Initialize scroll rect occlusion attached now that all buttons are instantiated.
            scrollRectOcclusion.Init();
            // Set scroll list to top.
            scrollbar.value = 1f;
            // Reset position to occlude off screen buttons.
            scrollRectOcclusion.ScrollToDefault();

            yield return null;
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

        private void UpdatePersonalBestButtonText()
        {
            personalBestButton.DeactivateNoRecordSetText();
            personalBestButton.ActivateContentPanel();

            // Index's are offset by 1 as the php script does not need to return a name.
            personalBestButton.SetPlayerScoreComboText($"{ProfileManager.Username} " +
                $"{personalBestPlacementDataArr[PersonalBestScoreUrlIndex]} " +
                $"{personalBestPlacementDataArr[PersonalBestComboUrlIndex]}x");

            personalBestButton.SetAccuracyText($"{personalBestPlacementDataArr[PersonalBestAccuracyUrlIndex]}%");

            personalBestButton.SetPerfectText($"p:{personalBestPlacementDataArr[PersonalBestPerfectUrlIndex]}");

            personalBestButton.SetGreatText($"g:{personalBestPlacementDataArr[PersonalBestGreatUrlIndex]}");

            personalBestButton.SetOkayText($"o:{personalBestPlacementDataArr[PersonalBestOkayUrlIndex]}");

            personalBestButton.SetMissText($"m:{personalBestPlacementDataArr[PersonalBestMissUrlIndex]}");

            personalBestButton.SetFeverText($"f:{personalBestPlacementDataArr[PersonalBestFeverUrlIndex]}");

            personalBestButton.SetBonusText($"b:{personalBestPlacementDataArr[PersonalBestBonusUrlIndex]}");

            personalBestButton.SetDateText(UtilityMethods.GetTimeSinceDateInput(personalBestPlacementDataArr
                [PersonalBestDateUrlIndex]));

            personalBestButton.SetPositionText($"#{personalBestPlacementDataArr[PersonalBestPlacementUrlIndex]} of " +
                $"{personalBestPlacementDataArr[PersonalBestTotalRecordsUrlIndex]}");

            TextMeshProUGUI gradeText = gradedata.GetGradeText(float.Parse(personalBestPlacementDataArr
                [PersonalBestAccuracyUrlIndex]));
            personalBestButton.SetGradeText(gradeText.text, gradeText.colorGradientPreset);

            hasLoadedPersonalBest = true;
        }

        private void DisplayPersonalBestNoRecord()
        {
            personalBestButton.DeactivateContentPanel();
            personalBestButton.ActivateNoRecordText();
            hasLoadedPersonalBest = true;
            CheckIfReadyToDisplayLeaderboard();
        }

        private void UpdateButtonText(int _index)
        {
            buttonArr[_index].ActivateContentPanel();
            buttonArr[_index].DeactivateNoRecordSetText();

            buttonArr[_index].SetNameText(placementDataArr[_index][PlayernameUrlIndex]);
            buttonArr[_index].SetLevelText($"lvl {placementDataArr[_index][LevelUrlIndex]}");
            buttonArr[_index].SetDateText(UtilityMethods.GetTimeSinceDateInput(placementDataArr[_index][DateUrlIndex]));
            buttonArr[_index].SetCategoryText($"{placementDataArr[_index][CategoryUrlIndex]}");

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

            personalBestButton.gameObject.SetActive(false);
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

        // Reset leaderboard.
        private void ResetLeaderboard()
        {
            StopAllCoroutines();
            HideLeaderboard();
            ResetVariables();
            loadingIcon.DisplayLoadingIcon();
        }

        // Resets and requests a new leaderboard to be loaded.
        public void RequestNewLeaderboard()
        {
            ResetLeaderboard();
            RequestLeaderboard();
        }

        private void ResetVariables()
        {
            imageUpdatedCount = default;
            hasLoadedPersonalBest = false;
        }

        // Is called when the category dropdown value has been changed.
        public void CategoryDropdownValueChange()
        {
            switch (categorySortDropdown.value)
            {
                case CategoryScoreIndex:
                    notification.DisplayNotification(colorCollection.PurpleColor080, CategoryScoreNotification, 4f);
                    break;
                case CatagoryAccuracyIndex:
                    notification.DisplayNotification(colorCollection.PurpleColor080, CategoryAccuracyNotification, 4f);
                    break;
                case CatagoryComboIndex:
                    notification.DisplayNotification(colorCollection.PurpleColor080, CategoryComboNotification, 4f);
                    break;
            }

            RequestNewLeaderboard();
        }

        // Is called when the view dropdown value has been changed.
        public void ViewDropdownValueChange()
        {
            viewSortDropdown.value = ViewGlobalIndex;

            notification.DisplayNotification(colorCollection.RedColor080, ViewNotification, 4f);
        }
        #endregion
    }
}