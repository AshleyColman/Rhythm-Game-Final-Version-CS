namespace Gameplay
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public sealed class LeaderboardButton : MonoBehaviour
    {
        #region Private Fields
        private byte position = 0;

        private uint score = 0;

        [SerializeField] private Image profileImage = default;

        [SerializeField] private TextMeshProUGUI positionText = default;
        [SerializeField] private TextMeshProUGUI scoreText = default;
        [SerializeField] private TextMeshProUGUI nameText = default;
        [SerializeField] private TextMeshProUGUI gradeText = default;
        #endregion

        #region Properties
        public byte Position => position; 
        public uint Score => score;
        #endregion

        #region Public Methods
        public void SetPosition(byte _position)
        {
            position = _position;
            positionText.SetText($"{_position}#");
        }

        public void SetPositionValueOnly(byte _position)
        {
            position = _position;
        }

        public void SetPosition(byte _bytePosition, string _stringPosition)
        {
            position = _bytePosition;
            positionText.SetText(_stringPosition);
        }

        public void SetScore(uint _score)
        {
            score = _score;
            scoreText.SetText(UtilityMethods.AddZerosToScoreString(_score.ToString()));
        }

        public void SetScore(TextMeshProUGUI _text)
        {
            scoreText.SetText(_text.text);
        }

        public void SetGrade(TMP_ColorGradient _color, string _text)
        {
            gradeText.colorGradientPreset = _color;
            gradeText.SetText(_text);
        }

        public void SetGrade(TextMeshProUGUI _text)
        {
            gradeText.colorGradientPreset = _text.colorGradientPreset;
            gradeText.SetText(_text.text);
        }

        public void SetName(string _name)
        {
            name = _name;
            nameText.SetText(_name);
        }

        public void Deactivate()
        {
            this.gameObject.SetActive(false);
        }
        #endregion
    }
}
