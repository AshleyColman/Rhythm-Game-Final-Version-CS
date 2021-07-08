using Background;
using Enums;
using Menu;
using SceneLoading;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class ModePanel : MonoBehaviour 
{
    #region Constants
    private readonly string[] ModeTextStringArray = new string[] { "mode select", "quickplay", "editor",
            "download", "rankings", "profile", "settings", "exit" };

    private readonly string[] ModeDescriptionTextStringArray = new string[]
        {
              "select a mode from the menu",
              "play the rhythm game to level up and compete on leaderboards",
              "create your own map for others to play",
              "download more beatmaps online",
              "view game rankings",
              "view your profile",
              "configure settings to how you like to play",
              "leave the game - thanks for playing" };

    private readonly ColorName[] ModeColorArray = new ColorName[]
    {
        ColorName.DARK_BLUE,
        ColorName.PINK,
        ColorName.ORANGE,
        ColorName.LIGHT_GREEN,
        ColorName.PURPLE,
        ColorName.YELLOW,
        ColorName.LIGHT_BLUE,
        ColorName.RED };
    #endregion

    #region Private Fields
    [SerializeField] private GameObject modePanel = default;

    [SerializeField] private TextMeshProUGUI modeText = default;

    [SerializeField] private Image[] imageArray = default;

    [SerializeField] private CanvasGroup flashCanvasGroup = default;

    private Transform[] imageTransformArray;
    private Transform modeTextTransform;

    private Vector3 defaultModeTextPosition;

    private IEnumerator checkModeMenuInput;
    private IEnumerator transitionInCoroutine;

    private Transition transition;
    private ColorCollection colorCollection;
    private OverlayCanvasManager overlayCanvasManager;
    private MenuManager menuManager;
    private DescriptionPanel descriptionPanel;
    private NavigationPanel navigationPanel;
    private BackgroundManager backgroundManager;
    private TextPanel textPanel;
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

    public IEnumerator TransitionInCoroutine()
    {
        navigationPanel.DisableButtonEventTriggers();
        modePanel.gameObject.SetActive(true);
        backgroundManager.SetNewImageReferences(imageArray);
        navigationPanel.TransitionIn();
        navigationPanel.OnClickButton(MenuManager.StartMenuIndex);
        CheckModeMenuInput();
        yield return new WaitForSeconds(2f);
        navigationPanel.EnableButtonEventTriggers();
        yield return null;
    }

    public void TransitionOut()
    {
        modePanel.gameObject.SetActive(false);
        navigationPanel.DisableButtonEventTriggers();
        StopAllCoroutines();
    }

    public void NavigationButton_OnPointerEnter(int _modeIndex)
    {
        if (modePanel.gameObject.activeSelf == true)
        {
            PlayOnSelectAnimation(_modeIndex);
            modeText.SetText(ModeTextStringArray[_modeIndex]);
            textPanel.TypeText(ModeDescriptionTextStringArray[_modeIndex]);
            descriptionPanel.SetPanelColor(ModeColorArray[_modeIndex]);
            PlayModeTextTween();
        }
    }

    public void NavigationButton_OnPointerExit()
    {
        if (modePanel.gameObject.activeSelf == true)
        {
            byte modeIndex = navigationPanel.CurrentSelectedNavigationButton.ButtonMenuIndex;
            PlayOnSelectAnimation(modeIndex);
            modeText.SetText(ModeTextStringArray[modeIndex]);
            textPanel.TypeText(ModeDescriptionTextStringArray[modeIndex]);
            descriptionPanel.SetPanelColor(ModeColorArray[modeIndex]);
            PlayModeTextTween();
        }
    }
    #endregion

    #region Private Methods
    private void Awake()
    {
        transition = FindObjectOfType<Transition>();
        colorCollection = FindObjectOfType<ColorCollection>();
        menuManager = FindObjectOfType<MenuManager>();
        descriptionPanel = FindObjectOfType<DescriptionPanel>();
        navigationPanel = FindObjectOfType<NavigationPanel>();
        backgroundManager = FindObjectOfType<BackgroundManager>();
        textPanel = FindObjectOfType<TextPanel>();

        SetmodeBackgroundImageTransformArray();
        modeTextTransform = modeText.transform;
        defaultModeTextPosition = modeTextTransform.localPosition;
    }

    private void SetmodeBackgroundImageTransformArray()
    {
        imageTransformArray = new Transform[imageArray.Length];

        for (byte i = 0; i < imageArray.Length; i++)
        {
            imageTransformArray[i] = imageArray[i].transform;
        }
    }

    private void PlayOnSelectAnimation(int _modeIndex)
    {
        backgroundManager.TransitionToImageFromIndex(_modeIndex);
        backgroundManager.PlayImageScaleTween(VectorConstants.Vector075, Vector3.one, 0.25f);

        flashCanvasGroup.alpha = 1f;
        LeanTween.cancel(flashCanvasGroup.gameObject);
        LeanTween.alphaCanvas(flashCanvasGroup, 0f, 1f).setEaseOutExpo();
    }

    private Color32 GetModeDescriptionColor(int _modeIndex)
    {
        switch (_modeIndex)
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
        LeanTween.cancel(modeText.gameObject);
        modeTextTransform.localPosition = defaultModeTextPosition;

        LeanTween.moveLocalY(modeText.gameObject, (modeTextTransform.localPosition.y + 20f), 0.5f).setEaseInOutSine()
            .setLoopPingPong(-1);
    }

    private void CheckModeMenuInput()
    {
        StopCheckModeMenuInputCoroutine();

        checkModeMenuInput = CheckModeMenuInputCoroutine();
        StartCoroutine(checkModeMenuInput);
    }

    private void StopCheckModeMenuInputCoroutine()
    {
        if (checkModeMenuInput != null)
        {
            StopCoroutine(checkModeMenuInput);
        }
    }

    private IEnumerator CheckModeMenuInputCoroutine()
    {
        while (modePanel.gameObject.activeSelf == true)
        {
            if (Input.anyKey)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if ((navigationPanel.CurrentSelectedNavigationButton.ButtonMenuIndex - 1) < MenuManager.StartMenuIndex)
                    {
                        navigationPanel.OnClickButton(MenuManager.ExitMenuIndex);
                    }
                    else
                    {
                        navigationPanel.OnClickButton(navigationPanel.CurrentSelectedNavigationButton.ButtonMenuIndex - 1);
                    }
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if ((navigationPanel.CurrentSelectedNavigationButton.ButtonMenuIndex + 1) > MenuManager.ExitMenuIndex)
                    {
                        navigationPanel.OnClickButton(MenuManager.StartMenuIndex);
                    }
                    else
                    {
                        navigationPanel.OnClickButton(navigationPanel.CurrentSelectedNavigationButton.ButtonMenuIndex + 1);
                    }
                }
            }
            yield return null;
        }
        yield return null;
    }
    #endregion
}