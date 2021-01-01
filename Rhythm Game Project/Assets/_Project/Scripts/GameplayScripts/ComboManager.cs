namespace Gameplay
{
    using TMPro;
    using UnityEngine;
    using Audio;

    public sealed class ComboManager : MonoBehaviour
    {
        #region Constants
        private const byte ComboBreak = 5;

        private readonly Vector3 comboScaleTo = new Vector3(1.25f, 1.25f, 1f);
        private readonly Vector3 comboEffectScaleTo = new Vector3(1.75f, 1.75f, 1f);
        #endregion

        #region Private Fields
        private uint combo = 0;
        private uint highestCombo = 0;

        private Transform comboTextCachedTransform;
        private Transform comboEffectTextCachedTransform;

        [SerializeField] private TextMeshProUGUI comboText = default;
        [SerializeField] private TextMeshProUGUI comboEffectText = default;

        private ColorCollection colorCollection;
        private Leaderboard leaderboard;
        private GameplayAudioManager gameplayAudioManager;
        #endregion

        #region Properties
        public TextMeshProUGUI ComboText => comboText;
        #endregion

        #region Public Methods
        public void ResetCombo()
        {
            CheckIfHighestCombo();
            CheckIfComboBreak();
            combo = 0;
            UpdateComboText();
        }

        public void IncreaseCombo()
        {
            combo++;
            CheckIfHighestCombo();
            UpdateComboText();
            PlayComboIncreaseTween();
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            colorCollection = MonoBehaviour.FindObjectOfType<ColorCollection>();
            leaderboard = MonoBehaviour.FindObjectOfType<Leaderboard>();
            gameplayAudioManager = MonoBehaviour.FindObjectOfType<GameplayAudioManager>();
            ReferenceCachedTransforms();
        }

        private void ReferenceCachedTransforms()
        {
            comboTextCachedTransform = comboText.transform;
            comboEffectTextCachedTransform = comboEffectText.transform;
        }

        private void CheckIfComboBreak()
        {
            if (combo >= ComboBreak)
            {
                PlayComboBreakTween();
                gameplayAudioManager.PlayMissSound();
            }
            else
            {
                PlayComboResetTween();
            }
        }

        private void CheckIfHighestCombo()
        {
            if (combo > highestCombo)
            {
                highestCombo = combo;
            }
        }

        private void ResetComboProperties()
        {
            CancelComboTextTween();
            ResetComboTextTransformScale();
            ResetComboTextColor();
        }

        private void CancelComboTextTween()
        {
            LeanTween.cancel(comboText.gameObject);
            LeanTween.cancel(comboEffectText.gameObject);
        }

        private void ResetComboTextTransformScale()
        {
            comboTextCachedTransform.localScale = Vector3.one;
            comboEffectTextCachedTransform.localScale = Vector3.one;
        }

        private void ResetComboTextColor()
        {
            if (comboText.color != colorCollection.WhiteColor)
            {
                comboText.color = colorCollection.WhiteColor;
                comboEffectText.color = colorCollection.WhiteColor025;
            }
        }

        private void PlayComboBreakTween()
        {
            ResetComboProperties();
            LeanTween.scale(comboText.gameObject, comboScaleTo, 1f).setEasePunch();
            LeanTween.scale(comboEffectText.gameObject, comboEffectScaleTo, 1f).setEasePunch();
            comboText.color = colorCollection.RedColor;
            comboEffectText.color = colorCollection.RedColor025;
        }

        private void PlayComboIncreaseTween()
        {
            ResetComboProperties();
            LeanTween.scale(comboText.gameObject, comboScaleTo, 1f).setEasePunch();
            LeanTween.scale(comboEffectText.gameObject, comboEffectScaleTo, 1f).setEasePunch();
        }

        private void PlayComboResetTween()
        {

        }

        private void UpdateComboText()
        {
            comboText.SetText($"{combo}x");
            comboEffectText.SetText(comboText.text);
            leaderboard.UpdatePersonalCombo();
        }
        #endregion
    }
}
