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
              "play the rhythm game, complete against others on user created maps",
              "create your own map of your favorite song for others to play",
              "download more maps",
              "view how you stand against other players overall and on individual maps",
              "configure settings",
              "view your profile and stats",
              "thanks for playing!" };
        #endregion

        #region Private Fields
        [SerializeField] private GameObject modePanel = default;

        [SerializeField] private TextMeshProUGUI modeText = default;
        [SerializeField] private TextMeshProUGUI modeDescriptionText = default;

        [SerializeField] private Image[] modeBackgroundImageArray = default;
        [SerializeField] private Image modeDescriptionColorImage = default;
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
        public void Hover_Button(int _modeButtonIndex)
        {
            PlayHoverButtonTween(_modeButtonIndex);
            SetModeText(_modeButtonIndex);
            SetModeDescriptionText(_modeButtonIndex);
            SetModeDescriptionColor(_modeButtonIndex);
            PlayModeTextTween();
            SetPreviousModeBackgroundImage(_modeButtonIndex);
            menuManager.SetCurrentHoverMenuIndex(_modeButtonIndex);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            transition = FindObjectOfType<Transition>();
            colorCollection = FindObjectOfType<ColorCollection>();
            topCanvasManager = FindObjectOfType<TopCanvasManager>();
            menuManager = FindObjectOfType<MenuManager>();
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

        private void SetModeDescriptionText(int _modeButtonIndex)
        {
            modeDescriptionText.SetText(ModeDescriptionTextStringArray[_modeButtonIndex]);
        }
        
        private void SetPreviousModeBackgroundImage(int _modeButtonIndex)
        {
            previousModeBackgroundImage = modeBackgroundImageArray[_modeButtonIndex];
        }

        private void SetModeDescriptionColor(int _modeButtonIndex)
        {
            // { "title", "quickplay", "editor", "download", "rankings", "settings", "profile", "exit" };
            switch (_modeButtonIndex)
            {
                case 0:
                    modeDescriptionColorImage.color = colorCollection.DarkBlueColor080;
                    break;
                case 1:
                    modeDescriptionColorImage.color = colorCollection.PinkColor080;
                    break;
                case 2:
                    modeDescriptionColorImage.color = colorCollection.OrangeColor080;
                    break;
                case 3:
                    modeDescriptionColorImage.color = colorCollection.LightGreenColor080;
                    break;
                case 4:
                    modeDescriptionColorImage.color = colorCollection.PurpleColor080;
                    break;
                case 5:
                    modeDescriptionColorImage.color = colorCollection.YellowColor080;
                    break;
                case 6:
                    modeDescriptionColorImage.color = colorCollection.LightBlueColor080;
                    break;
                case 7:
                    modeDescriptionColorImage.color = colorCollection.RedColor080;
                    break;
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
