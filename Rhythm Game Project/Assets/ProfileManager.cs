namespace Profile
{
    using UnityEngine;
    using Loading;
    using TMPro;
    using UnityEngine.UI;
    using UnityEngine.Networking;
    using System.Text.RegularExpressions;
    using System.Collections;
    using Menu;
    using Enums;
    using Level;
    using ImageLoad;
    using Player;

    public sealed class ProfileManager : MonoBehaviour
    {
        #region Constants
        private const string RegixSplit = "->";
        private const string FieldUsername = "username";
        private const string UrlErrorCode = "error";
        private const string RetrieveProfileDataUrl = "http://localhost/RhythmGameOnlineScripts/RetrieveProfileScript.php";

        private const byte UsernameIndex = 0;
        private const byte DateJoinedIndex = 1;
        private const byte LevelIndex = 2;
        private const byte ImageUrlIndex = 3;
        private const byte BannerImageUrlIndex = 4;

        private const byte RankedPlayTimeIndex = 5;
        private const byte RankedScoreIndex = 6;
        private const byte RankedComboIndex = 7;
        private const byte RankedObjectsHitIndex = 8;
        private const byte RankedPerfectIndex = 9;
        private const byte RankedGreatIndex = 10;
        private const byte RankedOkayIndex = 11;
        private const byte RankedMissIndex = 12;
        private const byte RankedGradeSPlusIndex = 13;
        private const byte RankedGradeSIndex = 14;
        private const byte RankedGradeAPlusIndex = 15;
        private const byte RankedGradeAIndex = 16;
        private const byte RankedGradeBPlusIndex = 17;
        private const byte RankedGradeBIndex = 18;
        private const byte RankedGradeCPlusIndex = 19;
        private const byte RankedGradeCIndex = 20;
        private const byte RankedGradeDPlusIndex = 21;
        private const byte RankedGradeDIndex = 22;
        private const byte RankedGradeEPlusIndex = 23;
        private const byte RankedGradeEIndex = 24;
        private const byte RankedGradeFPlusIndex = 25;
        private const byte RankedGradeFIndex = 26;
        private const byte RankedClearPointsIndex = 27;
        private const byte RankedHiddenPointsIndex = 28;
        private const byte RankedMinePointsIndex = 29;
        private const byte RankedLowApproachRatePointsIndex = 30;
        private const byte RankedHighApproachRatePointsIndex = 31;
        private const byte RankedFullComboPointsIndex = 32;
        private const byte RankedMaxPercentagePointsIndex = 33;
        private const byte RankedFailsIndex = 34;
        private const byte RankedTwoKeyScoreIndex = 35;
        private const byte RankedFourKeyScoreIndex = 36;
        private const byte RankedSixKeyScoreIndex = 37;

        private const byte TotalPlayTimeIndex = 38;
        private const byte TotalScoreIndex = 39;
        private const byte TotalComboIndex = 40;
        private const byte TotalObjectsHitIndex = 41;
        private const byte TotalPerfectIndex = 42;
        private const byte TotalGreatIndex = 43;
        private const byte TotalOkayIndex = 44;
        private const byte TotalMissIndex = 45;
        private const byte TotalGradeSPlusIndex = 46;
        private const byte TotalGradeSIndex = 47;
        private const byte TotalGradeAPlusIndex = 48;
        private const byte TotalGradeAIndex = 49;
        private const byte TotalGradeBPlusIndex = 50;
        private const byte TotalGradeBIndex = 51;
        private const byte TotalGradeCPlusIndex = 52;
        private const byte TotalGradeCIndex = 53;
        private const byte TotalGradeDPlusIndex = 54;
        private const byte TotalGradeDIndex = 55;
        private const byte TotalGradeEPlusIndex = 56;
        private const byte TotalGradeEIndex = 57;
        private const byte TotalGradeFPlusIndex = 58;
        private const byte TotalGradeFIndex = 59;
        private const byte TotalClearPointsIndex = 60;
        private const byte TotalHiddenPointsIndex = 61;
        private const byte TotalMinePointsIndex = 62;
        private const byte TotalLowApproachRatePointsIndex = 63;
        private const byte TotalHighApproachRatePointsIndex = 64;
        private const byte TotalFullComboPointsIndex = 65;
        private const byte TotalMaxPercentagePointsIndex = 66;
        private const byte TotalFailsIndex = 67;
        private const byte TotalTwoKeyScoreIndex = 68;
        private const byte TotalFourKeyScoreIndex = 69;
        private const byte TotalSixKeyScoreIndex = 70;
        #endregion

        #region Private Fields
        private bool profileImageHasLoaded = false;
        private bool bannerImageHasLoaded = false;

        private enum CategorySorting { Ranked, Total };

        private IEnumerator retrieveProfileData;
        private IEnumerator checkIfProfileDataHasLoaded;

        [SerializeField] private GameObject profile = default;

        [SerializeField] private Transform profilePanelTransform = default;

        [SerializeField] private LoadingIcon loadingIcon = default;

        [SerializeField] private Button settingButton = default;

        [SerializeField] private Scrollbar rankedScrollbar = default;
        [SerializeField] private Scrollbar totalScrollbar = default;

        [SerializeField] private Slider levelSlider = default;

        [SerializeField] private Image profileImage = default;
        [SerializeField] private Image bannerImage = default;

        [SerializeField] private TextMeshProUGUI playerNameText = default;
        [SerializeField] private TextMeshProUGUI joinedValueText = default;
        [SerializeField] private TextMeshProUGUI levelValueText = default;
        [SerializeField] private TextMeshProUGUI RankedPlayTimeValueText = default;
        [SerializeField] private TextMeshProUGUI RankedScoreValueText = default;
        [SerializeField] private TextMeshProUGUI RankedComboValueText = default;
        [SerializeField] private TextMeshProUGUI RankedObjectsHitValueText = default;
        [SerializeField] private TextMeshProUGUI RankedPerfectValueText = default;
        [SerializeField] private TextMeshProUGUI RankedGreatValueText = default;
        [SerializeField] private TextMeshProUGUI RankedOkayValueText = default;
        [SerializeField] private TextMeshProUGUI RankedMissValueText = default;
        [SerializeField] private TextMeshProUGUI RankedGradeSPlusValueText = default;
        [SerializeField] private TextMeshProUGUI RankedGradeSValueText = default;
        [SerializeField] private TextMeshProUGUI RankedGradeAPlusValueText = default;
        [SerializeField] private TextMeshProUGUI RankedGradeAValueText = default;
        [SerializeField] private TextMeshProUGUI RankedGradeBPlusValueText = default;
        [SerializeField] private TextMeshProUGUI RankedGradeBValueText = default;
        [SerializeField] private TextMeshProUGUI RankedGradeCPlusValueText = default;
        [SerializeField] private TextMeshProUGUI RankedGradeCValueText = default;
        [SerializeField] private TextMeshProUGUI RankedGradeDPlusValueText = default;
        [SerializeField] private TextMeshProUGUI RankedGradeDValueText = default;
        [SerializeField] private TextMeshProUGUI RankedGradeEPlusValueText = default;
        [SerializeField] private TextMeshProUGUI RankedGradeEValueText = default;
        [SerializeField] private TextMeshProUGUI RankedGradeFPlusValueText = default;
        [SerializeField] private TextMeshProUGUI RankedGradeFValueText = default;
        [SerializeField] private TextMeshProUGUI RankedClearValueText = default;
        [SerializeField] private TextMeshProUGUI RankedHiddenValueText = default;
        [SerializeField] private TextMeshProUGUI RankedMineValueText = default;
        [SerializeField] private TextMeshProUGUI RankedLowApproachRateValueText = default;
        [SerializeField] private TextMeshProUGUI RankedHighApproachRateValueText = default;
        [SerializeField] private TextMeshProUGUI RankedFullComboValueText = default;
        [SerializeField] private TextMeshProUGUI RankedMaxPercentageValueText = default;
        [SerializeField] private TextMeshProUGUI RankedFailsValueText = default;
        [SerializeField] private TextMeshProUGUI RankedTwoKeyScoreValueText = default;
        [SerializeField] private TextMeshProUGUI RankedFourKeyScoreValueText = default;
        [SerializeField] private TextMeshProUGUI RankedSixKeyScoreValueText = default;

        [SerializeField] private TextMeshProUGUI TotalPlayTimeValueText = default;
        [SerializeField] private TextMeshProUGUI TotalScoreValueText = default;
        [SerializeField] private TextMeshProUGUI TotalComboValueText = default;
        [SerializeField] private TextMeshProUGUI TotalObjectsHitValueText = default;
        [SerializeField] private TextMeshProUGUI TotalPerfectValueText = default;
        [SerializeField] private TextMeshProUGUI TotalGreatValueText = default;
        [SerializeField] private TextMeshProUGUI TotalOkayValueText = default;
        [SerializeField] private TextMeshProUGUI TotalMissValueText = default;
        [SerializeField] private TextMeshProUGUI TotalGradeSPlusValueText = default;
        [SerializeField] private TextMeshProUGUI TotalGradeSValueText = default;
        [SerializeField] private TextMeshProUGUI TotalGradeAPlusValueText = default;
        [SerializeField] private TextMeshProUGUI TotalGradeAValueText = default;
        [SerializeField] private TextMeshProUGUI TotalGradeBPlusValueText = default;
        [SerializeField] private TextMeshProUGUI TotalGradeBValueText = default;
        [SerializeField] private TextMeshProUGUI TotalGradeCPlusValueText = default;
        [SerializeField] private TextMeshProUGUI TotalGradeCValueText = default;
        [SerializeField] private TextMeshProUGUI TotalGradeDPlusValueText = default;
        [SerializeField] private TextMeshProUGUI TotalGradeDValueText = default;
        [SerializeField] private TextMeshProUGUI TotalGradeEPlusValueText = default;
        [SerializeField] private TextMeshProUGUI TotalGradeEValueText = default;
        [SerializeField] private TextMeshProUGUI TotalGradeFPlusValueText = default;
        [SerializeField] private TextMeshProUGUI TotalGradeFValueText = default;
        [SerializeField] private TextMeshProUGUI TotalClearValueText = default;
        [SerializeField] private TextMeshProUGUI TotalHiddenValueText = default;
        [SerializeField] private TextMeshProUGUI TotalMineValueText = default;
        [SerializeField] private TextMeshProUGUI TotalLowApproachRateValueText = default;
        [SerializeField] private TextMeshProUGUI TotalHighApproachRateValueText = default;
        [SerializeField] private TextMeshProUGUI TotalFullComboValueText = default;
        [SerializeField] private TextMeshProUGUI TotalMaxPercentageValueText = default;
        [SerializeField] private TextMeshProUGUI TotalFailsValueText = default;
        [SerializeField] private TextMeshProUGUI TotalTwoKeyScoreValueText = default;
        [SerializeField] private TextMeshProUGUI TotalFourKeyScoreValueText = default;
        [SerializeField] private TextMeshProUGUI TotalSixKeyScoreValueText = default;

        private Notification notification;
        private LevelManager levelManager;
        private ImageLoader imageLoader;
        #endregion

        #region Public Methods
        public void ExitButton_OnClick()
        {
            CloseProfile();
        }

        public void SettingButton_OnClick()
        {

        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            notification = FindObjectOfType<Notification>();
            levelManager = FindObjectOfType<LevelManager>();
            imageLoader = FindObjectOfType<ImageLoader>();
        }

        private void HideSettingButton()
        {
            settingButton.gameObject.SetActive(false);
        }

        private void ShowSettingButton()
        {
            settingButton.gameObject.SetActive(true);
        }

        private void CheckIfProfileIsSignedInPlayer(string _username)
        {
            if (Player.LoggedIn == true)
            {
                if (_username == Player.Username)
                {
                    ShowSettingButton();
                }
                else
                {
                    HideSettingButton();
                }
            }
            else
            {
                HideSettingButton();
            }
        }

        private void CloseProfile()
        {
            StopAllCoroutines();
            loadingIcon.StopAllCoroutines();
            imageLoader.StopAllCoroutines();
            profile.gameObject.SetActive(false);
        }

        private void ShowProfile()
        {
            profile.gameObject.SetActive(true);
            notification.DisplayTitleNotification(ColorName.LIGHT_GREEN, $"viewing {playerNameText.text}'s profile", 4f);
        }

        private void HideProfilePanel()
        {
            profilePanelTransform.gameObject.SetActive(false);
        }

        private void ShowProfilePanel()
        {
            profilePanelTransform.gameObject.SetActive(true);
            ResetScrollbarPositions();
        }

        private void ResetScrollbarPositions()
        {
            rankedScrollbar.value = 1f;
            totalScrollbar.value = 1f;
        }

        private void LoadNewProfile(string _username)
        {
            HideProfilePanel();
            HasNotLoadedProfileImage();
            HasNotLoadedBannerImage();
            CheckIfProfileIsSignedInPlayer(_username);
            RetrieveProfileData(_username);
        }

        private void RetrieveProfileData(string _username)
        {
            if (retrieveProfileData != null)
            {
                StopCoroutine(retrieveProfileData);
            }

            retrieveProfileData = RetrieveProfileDataCoroutine(_username);
            StartCoroutine(retrieveProfileData);
        }

        private IEnumerator RetrieveProfileDataCoroutine(string _username)
        {
            loadingIcon.DisplayLoadingIcon();

            WWWForm form = new WWWForm();

            form.AddField(FieldUsername, _username);

            UnityWebRequest www = UnityWebRequest.Post(RetrieveProfileDataUrl, form);

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError || www.downloadHandler.text == UrlErrorCode)
            {
                notification.DisplayDescriptionNotification(ColorName.RED, "profile error", "could not load profile information", 
                    4f);
            }
            else
            {
                string[] dataArr = Regex.Split(www.downloadHandler.text, RegixSplit);
                SetProfileText(dataArr);
                SetLevelSlider(dataArr[LevelIndex]);
                SetProfileImage(dataArr[ImageUrlIndex]);
                SetBannerImage(dataArr[BannerImageUrlIndex]);
            }

            CheckIfProfileDataHasLoaded();
        }

        private void CheckIfProfileDataHasLoaded()
        {
            if (checkIfProfileDataHasLoaded != null)
            {
                StopCoroutine(checkIfProfileDataHasLoaded);
            }

            checkIfProfileDataHasLoaded = CheckIfProfileDataHasLoadedCoroutine();
            StartCoroutine(checkIfProfileDataHasLoaded);
        }

        private IEnumerator CheckIfProfileDataHasLoadedCoroutine()
        {
            while (profilePanelTransform.gameObject.activeSelf == false)
            {
                if (profileImageHasLoaded == true && bannerImageHasLoaded == true)
                {
                    loadingIcon.HideLoadingIcon();
                    ShowProfilePanel();
                    break;
                }
                yield return null;
            }
            yield return null;
        }

        private void SetProfileImage(string _imageUrl)
        {
            imageLoader.LoadCompressedImageUrl(_imageUrl, profileImage, HasLoadedProfileImage);
        }

        private void HasLoadedProfileImage()
        {
            profileImageHasLoaded = true;
        }

        private void HasLoadedBannerImage()
        {
            bannerImageHasLoaded = true;
        }

        private void HasNotLoadedProfileImage()
        {
            profileImageHasLoaded = false;
        }

        private void HasNotLoadedBannerImage()
        {
            bannerImageHasLoaded = false;
        }

        private void SetBannerImage(string _imageUrl)
        {
            imageLoader.LoadCompressedImageUrl(_imageUrl, bannerImage, HasLoadedBannerImage);
        }

        private void SetLevelSlider(string _level)
        {
            levelSlider.value = float.Parse(_level);
            levelManager.SetLevelSliderGradientColor(levelSlider);
        }

        private void SetProfileText(string[] _dataArr)
        {
            playerNameText.SetText(_dataArr[UsernameIndex]);
            joinedValueText.SetText($"joined {_dataArr[DateJoinedIndex]}");
            levelValueText.SetText($"level {_dataArr[LevelIndex]}");
            RankedPlayTimeValueText.SetText(UtilityMethods.GetTimeFromFloat(float.Parse(_dataArr[RankedPlayTimeIndex])));
            RankedScoreValueText.SetText(_dataArr[RankedScoreIndex]);
            RankedComboValueText.SetText(_dataArr[RankedComboIndex]);
            RankedObjectsHitValueText.SetText(_dataArr[RankedObjectsHitIndex]);
            RankedPerfectValueText.SetText(_dataArr[RankedPerfectIndex]);
            RankedGreatValueText.SetText(_dataArr[RankedGreatIndex]);
            RankedOkayValueText.SetText(_dataArr[RankedOkayIndex]);
            RankedMissValueText.SetText(_dataArr[RankedMissIndex]);
            RankedGradeSPlusValueText.SetText(_dataArr[RankedGradeSPlusIndex]);
            RankedGradeSValueText.SetText(_dataArr[RankedGradeSIndex]);
            RankedGradeAPlusValueText.SetText(_dataArr[RankedGradeAPlusIndex]);
            RankedGradeAValueText.SetText(_dataArr[RankedGradeAIndex]);
            RankedGradeBPlusValueText.SetText(_dataArr[RankedGradeBPlusIndex]);
            RankedGradeBValueText.SetText(_dataArr[RankedGradeBIndex]);
            RankedGradeCPlusValueText.SetText(_dataArr[RankedGradeCPlusIndex]);
            RankedGradeCValueText.SetText(_dataArr[RankedGradeCIndex]);
            RankedGradeDPlusValueText.SetText(_dataArr[RankedGradeDPlusIndex]);
            RankedGradeDValueText.SetText(_dataArr[RankedGradeDIndex]);
            RankedGradeEPlusValueText.SetText(_dataArr[RankedGradeEPlusIndex]);
            RankedGradeEValueText.SetText(_dataArr[RankedGradeEIndex]);
            RankedGradeFPlusValueText.SetText(_dataArr[RankedGradeFPlusIndex]);
            RankedGradeFValueText.SetText(_dataArr[RankedGradeFIndex]);
            RankedClearValueText.SetText(_dataArr[RankedClearPointsIndex]);
            RankedHiddenValueText.SetText(_dataArr[RankedHiddenPointsIndex]);
            RankedMineValueText.SetText(_dataArr[RankedMinePointsIndex]);
            RankedLowApproachRateValueText.SetText(_dataArr[RankedLowApproachRatePointsIndex]);
            RankedHighApproachRateValueText.SetText(_dataArr[RankedHighApproachRatePointsIndex]);
            RankedFullComboValueText.SetText(_dataArr[RankedFullComboPointsIndex]);
            RankedMaxPercentageValueText.SetText(_dataArr[RankedMaxPercentagePointsIndex]);
            RankedFailsValueText.SetText(_dataArr[RankedFailsIndex]);
            RankedTwoKeyScoreValueText.SetText(_dataArr[RankedTwoKeyScoreIndex]);
            RankedFourKeyScoreValueText.SetText(_dataArr[RankedFourKeyScoreIndex]);
            RankedSixKeyScoreValueText.SetText(_dataArr[RankedSixKeyScoreIndex]);

            TotalPlayTimeValueText.SetText(UtilityMethods.GetTimeFromFloat(float.Parse(_dataArr[TotalPlayTimeIndex])));
            TotalScoreValueText.SetText(_dataArr[TotalScoreIndex]);
            TotalComboValueText.SetText(_dataArr[TotalComboIndex]);
            TotalObjectsHitValueText.SetText(_dataArr[TotalObjectsHitIndex]);
            TotalPerfectValueText.SetText(_dataArr[TotalPerfectIndex]);
            TotalGreatValueText.SetText(_dataArr[TotalGreatIndex]);
            TotalOkayValueText.SetText(_dataArr[TotalOkayIndex]);
            TotalMissValueText.SetText(_dataArr[TotalMissIndex]);
            TotalGradeSPlusValueText.SetText(_dataArr[TotalGradeSPlusIndex]);
            TotalGradeSValueText.SetText(_dataArr[TotalGradeSIndex]);
            TotalGradeAPlusValueText.SetText(_dataArr[TotalGradeAPlusIndex]);
            TotalGradeAValueText.SetText(_dataArr[TotalGradeAIndex]);
            TotalGradeBPlusValueText.SetText(_dataArr[TotalGradeBPlusIndex]);
            TotalGradeBValueText.SetText(_dataArr[TotalGradeBIndex]);
            TotalGradeCPlusValueText.SetText(_dataArr[TotalGradeCPlusIndex]);
            TotalGradeCValueText.SetText(_dataArr[TotalGradeCIndex]);
            TotalGradeDPlusValueText.SetText(_dataArr[TotalGradeDPlusIndex]);
            TotalGradeDValueText.SetText(_dataArr[TotalGradeDIndex]);
            TotalGradeEPlusValueText.SetText(_dataArr[TotalGradeEPlusIndex]);
            TotalGradeEValueText.SetText(_dataArr[TotalGradeEIndex]);
            TotalGradeFPlusValueText.SetText(_dataArr[TotalGradeFPlusIndex]);
            TotalGradeFValueText.SetText(_dataArr[TotalGradeFIndex]);
            TotalClearValueText.SetText(_dataArr[TotalClearPointsIndex]);
            TotalHiddenValueText.SetText(_dataArr[TotalHiddenPointsIndex]);
            TotalMineValueText.SetText(_dataArr[TotalMinePointsIndex]);
            TotalLowApproachRateValueText.SetText(_dataArr[TotalLowApproachRatePointsIndex]);
            TotalHighApproachRateValueText.SetText(_dataArr[TotalHighApproachRatePointsIndex]);
            TotalFullComboValueText.SetText(_dataArr[TotalFullComboPointsIndex]);
            TotalMaxPercentageValueText.SetText(_dataArr[TotalMaxPercentagePointsIndex]);
            TotalFailsValueText.SetText(_dataArr[TotalFailsIndex]);
            TotalTwoKeyScoreValueText.SetText(_dataArr[TotalTwoKeyScoreIndex]);
            TotalFourKeyScoreValueText.SetText(_dataArr[TotalFourKeyScoreIndex]);
            TotalSixKeyScoreValueText.SetText(_dataArr[TotalSixKeyScoreIndex]);
        }

        private void HideProfile()
        {
            profile.gameObject.SetActive(false);
        }
        #endregion
    }
}