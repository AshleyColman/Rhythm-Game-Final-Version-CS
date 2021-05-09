namespace Menu
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class PersonalBestLeaderboardButton : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private GameObject contentPanel = default;

        [SerializeField] private Image profileImage = default;

        [SerializeField] private TextMeshProUGUI positionText = default;
        [SerializeField] private TextMeshProUGUI playerScoreComboText = default;
        [SerializeField] private TextMeshProUGUI gradeText = default;
        [SerializeField] private TextMeshProUGUI dateText = default;
        [SerializeField] private TextMeshProUGUI noRecordSetText = default;
        [SerializeField] private TextMeshProUGUI levelText = default;
        [SerializeField] private TextMeshProUGUI perfectText = default;
        [SerializeField] private TextMeshProUGUI greatText = default;
        [SerializeField] private TextMeshProUGUI okayText = default;
        [SerializeField] private TextMeshProUGUI missText = default;
        [SerializeField] private TextMeshProUGUI accuracyText = default;
        [SerializeField] private TextMeshProUGUI feverText = default;
        [SerializeField] private TextMeshProUGUI bonusText = default;
        #endregion

        #region Public Methods
        public void DeactivateContentPanel()
        {
            if (contentPanel.gameObject.activeSelf == true)
            {
                contentPanel.gameObject.SetActive(false);
            }
        }

        public void ActivateContentPanel()
        {
            if (contentPanel.gameObject.activeSelf == false)
            {
                contentPanel.gameObject.SetActive(true);
            }
        }

        public void DeactivateNoRecordSetText()
        {
            if (noRecordSetText.gameObject.activeSelf == true)
            {
                noRecordSetText.gameObject.SetActive(false);
            }
        }

        public void ActivateNoRecordText()
        {
            if (noRecordSetText.gameObject.activeSelf == false)
            {
                noRecordSetText.gameObject.SetActive(true);
            }
        }

        public void SetProfileImage(Texture _texture)
        {
        }

        public void SetNewProfileImageMaterial(Material _material)
        {
            profileImage.material = _material;
        }

        public void SetPositionText(string _text)
        {
            positionText.SetText(_text);
        }

        public void SetLevelText(string _text)
        {
            levelText.SetText(_text);
        }

        public void SetGradeText(string _text, TMP_ColorGradient _color)
        {
            gradeText.SetText(_text);
            gradeText.colorGradientPreset = _color;
        }

        public void SetDateText(string _text)
        {
            dateText.SetText(_text);
        }

        public void SetPlayerScoreComboText(string _text)
        {
            playerScoreComboText.SetText(_text);
        }

        public void SetPerfectText(string _text)
        {
            perfectText.SetText(_text);
        }

        public void SetGreatText(string _text)
        {
            greatText.SetText(_text);
        }

        public void SetOkayText(string _text)
        {
            okayText.SetText(_text);
        }

        public void SetMissText(string _text)
        {
            missText.SetText(_text);
        }

        public void SetAccuracyText(string _text)
        {
            accuracyText.SetText(_text);
        }

        public void SetFeverText(string _text)
        {
            feverText.SetText(_text);
        }

        public void SetBonusText(string _text)
        {
            bonusText.SetText(_text);
        }
        #endregion
    }
}