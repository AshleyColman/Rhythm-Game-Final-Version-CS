namespace Menu
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using SceneLoading;

    public sealed class ModeMenuManager : MonoBehaviour, IMenu
    {
        #region Constants
        private readonly string[] ModeTextStringArray = new string[] { "welcome", "quickplay", "editor",
            "download", "rankings", "settings", "profile", "exit" };
        private readonly string[] ModeDescriptionTextStringArray = new string[]
            {
              "return to the title screen",
              "play",
              "create your own map",
              "download more maps",
              "view game rankings",
              "configure settings",
              "view your profile and stats",
              "thanks for playing!" };
        #endregion

        #region Private Fields
        [SerializeField] private GameObject modePanel = default;

        [SerializeField] private TextMeshProUGUI modeText = default;

        [SerializeField] private Image[] modeBackgroundImageArray = default;
        private Image previousModeBackgroundImage;

        private Transform[] modeBackgroundImageCachedTransformArray;
        private Transform modeTextCachedTransform;

        private Vector3 defaultModeTextPosition;

        [SerializeField] private CanvasGroup flashCanvasGroup = default;

        private IEnumerator checkModeMenuInputCoroutine;

        private Transition transition;
        private ColorCollection colorCollection;
        private TopCanvasManager topCanvasManager;
        private MenuManager menuManager;
        private DescriptionPanel descriptionPanel;
        #endregion

        #region Public Methods
        public void TransitionIn()
        {
            modePanel.gameObject.SetActive(true);
            topCanvasManager.EnableModeButtonTriggers();
            Hover_Button(MenuManager.StartMenuIndex);
            CheckModeMenuInput();
        }

        public void TransitionOut()
        {
            topCanvasManager.DisableModeButtonTriggers();
            StopCheckModeMenuInputCoroutine();
            modePanel.gameObject.SetActive(false);
        }

        public void OnTick()
        {

        }

        public void OnMeasure()
        {

        }

        public void Hover_Button(int _modeButtonIndex)
        {
            if (modePanel.gameObject.activeSelf == true)
            {
                PlayHoverButtonTween(_modeButtonIndex);
                SetModeText(_modeButtonIndex);
                descriptionPanel.SetSingleDescription(ModeDescriptionTextStringArray[_modeButtonIndex],
                    GetModeDescriptionColor(_modeButtonIndex));
                PlayModeTextTween();
                SetPreviousModeBackgroundImage(_modeButtonIndex);
                menuManager.SetCurrentHoverMenuIndex(_modeButtonIndex);
            }
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            transition = FindObjectOfType<Transition>();
            colorCollection = FindObjectOfType<ColorCollection>();
            topCanvasManager = FindObjectOfType<TopCanvasManager>();
            menuManager = FindObjectOfType<MenuManager>();
            descriptionPanel = FindObjectOfType<DescriptionPanel>();
            SetModeBackgroundImageCachedTransformArray();
            modeTextCachedTransform = modeText.transform;
            defaultModeTextPosition = modeTextCachedTransform.localPosition;
            previousModeBackgroundImage = modeBackgroundImageArray[0];
        }

        private void SetModeBackgroundImageCachedTransformArray()
        {
            modeBackgroundImageCachedTransformArray = new Transform[modeBackgroundImageArray.Length];

            for (byte i = 0; i < modeBackgroundImageArray.Length; i++)
            {
                modeBackgroundImageCachedTransformArray[i] = modeBackgroundImageArray[i].transform;
            }
        }

        private void PlayHoverButtonTween(int _modeButtonIndex)
        {
            previousModeBackgroundImage.gameObject.SetActive(false);
            modeBackgroundImageCachedTransformArray[_modeButtonIndex].gameObject.SetActive(true);

            modeBackgroundImageCachedTransformArray[_modeButtonIndex].localScale = new Vector3(0.75f, 0.75f, 1f);
            LeanTween.cancel(modeBackgroundImageArray[_modeButtonIndex].gameObject);
            LeanTween.scale(modeBackgroundImageArray[_modeButtonIndex].gameObject, Vector3.one, 0.25f).setEaseOutExpo();

            flashCanvasGroup.alpha = 1f;
            LeanTween.cancel(flashCanvasGroup.gameObject);
            LeanTween.alphaCanvas(flashCanvasGroup, 0f, 1f).setEaseOutExpo();
        }

        private void SetModeText(int _modeButtonIndex)
        {
            modeText.SetText(ModeTextStringArray[_modeButtonIndex]);
        }

        private void SetPreviousModeBackgroundImage(int _modeButtonIndex)
        {
            previousModeBackgroundImage = modeBackgroundImageArray[_modeButtonIndex];
        }

        private Color32 GetModeDescriptionColor(int _modeButtonIndex)
        {
            // { "title", "quickplay", "editor", "download", "rankings", "settings", "profile", "exit" };
            switch (_modeButtonIndex)
            {
                case 0:
                    return colorCollection.DarkBlueColor080;
                case 1:
                    return colorCollection.PinkColor080;
                case 2:
                    return colorCollection.OrangeColor080;
                case 3:
                    return colorCollection.LightGreenColor080;
                case 4:
                    return colorCollection.PurpleColor080;
                case 5:
                    return colorCollection.YellowColor080;
                case 6:
                    return colorCollection.LightBlueColor080;
                case 7:
                    return colorCollection.RedColor080;
                default:
                    return colorCollection.WhiteColor080;
            }
        }

        private void PlayModeTextTween()
        {
            modeTextCachedTransform.localPosition = defaultModeTextPosition;
            LeanTween.cancel(modeText.gameObject);

            LeanTween.moveLocalY(modeText.gameObject, (modeTextCachedTransform.localPosition.y + 20f), 0.5f).setEaseInOutSine()
                .setLoopPingPong(-1);
        }

        private void CheckModeMenuInput()
        {
            if (checkModeMenuInputCoroutine != null)
            {
                StopCoroutine(checkModeMenuInputCoroutine);
            }

            checkModeMenuInputCoroutine = CheckModeMenuInputCoroutine();
            StartCoroutine(checkModeMenuInputCoroutine);
        }

        private void StopCheckModeMenuInputCoroutine()
        {
            StopCoroutine("CheckModeMenuInputCoroutine");
        }

        private IEnumerator CheckModeMenuInputCoroutine()
        {
            while (modePanel.gameObject.activeSelf == true)
            {
                if (Input.anyKey)
                {
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if ((menuManager.CurrentHoverMenuIndex - 1) < MenuManager.StartMenuIndex)
                        {
                            topCanvasManager.Button_Click(MenuManager.ExitMenuIndex);
                            Hover_Button(MenuManager.ExitMenuIndex);
                        }
                        else
                        {
                            topCanvasManager.Button_Click(menuManager.CurrentHoverMenuIndex - 1);
                            Hover_Button(menuManager.CurrentHoverMenuIndex - 1);
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if ((menuManager.CurrentHoverMenuIndex + 1) > MenuManager.ExitMenuIndex)
                        {
                            topCanvasManager.Button_Click(MenuManager.StartMenuIndex);
                            Hover_Button(MenuManager.StartMenuIndex);
                        }
                        else
                        {
                            topCanvasManager.Button_Click(menuManager.CurrentHoverMenuIndex + 1);
                            Hover_Button(menuManager.CurrentHoverMenuIndex + 1);
                        }
                    }
                }

                yield return null;
            }

            yield return null;
        }
        #endregion
    }
}
