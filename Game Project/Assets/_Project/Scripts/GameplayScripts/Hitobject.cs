using UnityEngine;
using UnityEngine.UI;

public abstract class Hitobject : MonoBehaviour
{
    #region Private Fields
    [SerializeField] private Image colorImage = default;
    [SerializeField] private Image approachImage = default;

    [SerializeField] private CanvasGroup canvasGroup = default;

    [SerializeField] Transform cachedTransform = default;

    [SerializeField] RectTransform approachImageCachedRectTransform = default;

    protected KeyCode[] missKeyCodeArray;
    protected KeyCode[] hitKeyCodeArray;

    private static float approachTime = 0;
    #endregion

    #region Properties
    public Transform CachedTransform => cachedTransform;
    public KeyCode[] MissKeyCodeArray => missKeyCodeArray;
    public KeyCode[] HitKeyCodeArray => hitKeyCodeArray;
    public static float ApproachTime { set => approachTime = value; }
    #endregion

    #region Protected Methods
    protected virtual void SetMissKeyCodeArray()
    {
    }

    protected virtual void SetHitKeyCodeArray()
    {
    }
    #endregion

    #region Private Methods
    private void Awake()
    {
        SetMissKeyCodeArray();
        SetHitKeyCodeArray();
    }

    private void OnEnable()
    {
        SetLeanTween();
    }

    private void OnDisable()
    {
        CancelLeanTween();
    }

    private void SetLeanTween()
    {
        approachImageCachedRectTransform.localScale = Vector3.zero;
        LeanTween.scale(approachImage.gameObject, Vector3.one, approachTime);

        canvasGroup.alpha = 0f;
        LeanTween.alphaCanvas(canvasGroup, 1f, approachTime);
    }

    private void CancelLeanTween()
    {
        LeanTween.cancel(approachImage.gameObject);
        LeanTween.cancel(canvasGroup.gameObject);
    }
    #endregion
}
