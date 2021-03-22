namespace Gameplay
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public sealed class Hitobject : MonoBehaviour
    {
        #region Private Fields
        private static float approachTime = 0;

        private Vector3 missRotateTo = Vector3.zero;

        private Quaternion startingRotation = Quaternion.identity;

        [SerializeField] private TextMeshProUGUI numberText = default;

        [SerializeField] private GameObject hitobjectContainer = default;
        [SerializeField] private GameObject judgementContainer = default;

        [SerializeField] private ParticleSystem judgementParticleSystem = default;

        [SerializeField] private Image colorImage = default;
        [SerializeField] private Image approachImage = default;
        [SerializeField] private Image judgementColorImage = default;

        [SerializeField] private CanvasGroup canvasGroup = default;

        [SerializeField] private Transform cachedTransform = default;

        [SerializeField] private RectTransform approachImageCachedRectTransform = default;

        private IEnumerator playHitTweenCoroutine;
        private IEnumerator playMissTweenCoroutine;
        #endregion

        #region Properties
        public static float ApproachTime { set => approachTime = value; }
        public Transform CachedTransform => cachedTransform;
        #endregion

        #region Public Methods
        public void PlayHitTween()
        {
            if (playHitTweenCoroutine != null)
            {
                StopCoroutine(playHitTweenCoroutine);
            }

            playHitTweenCoroutine = PlayHitTweenCoroutine();
            StartCoroutine(playHitTweenCoroutine);
        }

        public void PlayMissTween()
        {
            if (playMissTweenCoroutine != null)
            {
                StopCoroutine(playMissTweenCoroutine);
            }

            playMissTweenCoroutine = PlayMissTweenCoroutine();
            StartCoroutine(playMissTweenCoroutine);
        }

        public void UpdateHitParticleSystemColor(Color _color)
        {
            ParticleSystem.MainModule judgementParticleSystemMainModule = judgementParticleSystem.main;
            judgementParticleSystemMainModule.startColor = _color;
        }

        public void UpdateJudgementImageColor(Color32 _color)
        {
            judgementColorImage.color = _color;
        }

        public void UpdateColorImageColor(Color32 _color)
        {
            colorImage.color = _color;
        }

        public void UpdateNumberText(string _text)
        {
            numberText.SetText(_text);
        }
        #endregion

        #region Private Methods
        private void OnEnable()
        {
            ResetProperties();
            PlayApproachTween();
        }

        private void OnDisable()
        {
            CancelTween();
        }

        private void Awake()
        {
            GetStartingRotation();
            SetMissRotation();
        }

        private void GetStartingRotation()
        {
            startingRotation = cachedTransform.localRotation;
        }

        private void SetMissRotation()
        {
            missRotateTo = new Vector3(cachedTransform.localRotation.x, cachedTransform.localRotation.y,
                cachedTransform.localRotation.z - 45f);
        }

        private void ResetProperties()
        {
            hitobjectContainer.gameObject.SetActive(true);
            cachedTransform.localScale = Vector3.zero;
            approachImageCachedRectTransform.localScale = Vector3.zero;
            cachedTransform.localRotation = startingRotation;
            canvasGroup.alpha = 0f;
        }

        private void PlayApproachTween()
        {
            LeanTween.scale(cachedTransform.gameObject, Vector3.one, 0.25f).setEaseOutExpo();
            LeanTween.scale(approachImage.gameObject, Vector3.one, approachTime);
            LeanTween.alphaCanvas(canvasGroup, 1f, approachTime);
        }

        private void CancelTween()
        {
            LeanTween.cancel(cachedTransform.gameObject);
            LeanTween.cancel(approachImage.gameObject);
            LeanTween.cancel(canvasGroup.gameObject);
        }

        private IEnumerator PlayHitTweenCoroutine()
        {
            CancelTween();
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.25f);
            hitobjectContainer.gameObject.SetActive(false);
            judgementContainer.gameObject.SetActive(true);
            LeanTween.alphaCanvas(canvasGroup, 0f, 0.25f).setEaseOutExpo();
            LeanTween.scale(gameObject, new Vector3(1.5f, 1.5f, 1f), 0.25f).setEaseOutExpo();
            yield return waitForSeconds;
            judgementContainer.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            yield return null;
        }

        private IEnumerator PlayMissTweenCoroutine()
        {
            CancelTween();
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.25f);
            hitobjectContainer.gameObject.SetActive(false);
            judgementContainer.gameObject.SetActive(true);
            LeanTween.alphaCanvas(canvasGroup, 0f, 0.25f).setEaseOutExpo();
            LeanTween.moveLocalY(gameObject, (cachedTransform.localPosition.y - 50f), 0.25f).setEaseOutExpo();
            LeanTween.rotateLocal(gameObject, missRotateTo, 0.25f);
            yield return waitForSeconds;
            judgementContainer.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            yield return null;
        }
        #endregion
    }
}
