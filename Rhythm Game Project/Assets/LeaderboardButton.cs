namespace Menu
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class LeaderboardButton : MonoBehaviour 
    {
        #region Private Fields
        [SerializeField] private GameObject contentPanel = default;

        [SerializeField] private Image profileImage = default;

        [SerializeField] private TextMeshProUGUI positionText = default;
        [SerializeField] private TextMeshProUGUI categoryText = default;
        [SerializeField] private TextMeshProUGUI nameText = default;
        [SerializeField] private TextMeshProUGUI gradeText = default;
        [SerializeField] private TextMeshProUGUI dateText = default;
        [SerializeField] private TextMeshProUGUI noRecordSetText = default;
        [SerializeField] private TextMeshProUGUI levelText = default;
        #endregion

        #region Properties
        public Image ProfileImage => profileImage;
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

        public void SetCategoryText(string _text)
        {
            categoryText.SetText(_text);
        }

        public void SetLevelText(string _text)
        {
            levelText.SetText(_text);
        }

        public void SetNameText(string _text)
        {
            nameText.SetText(_text);
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
        #endregion
    }
}