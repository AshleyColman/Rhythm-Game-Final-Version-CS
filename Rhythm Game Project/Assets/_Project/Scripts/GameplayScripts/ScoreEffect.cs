namespace Gameplay
{
    using UnityEngine;
    using TMPro;

    public sealed class ScoreEffect : MonoBehaviour
    {
        #region Constants
        private const int LeftMaxTargetPositionX = -100;
        private const int RightMaxTargetPositionX = 100;
        #endregion

        #region Private Fields
        [SerializeField] private TextMeshProUGUI[] leftScoreEffectTextArray = default;
        [SerializeField] private TextMeshProUGUI[] rightScoreEffectTextArray = default;

        private Transform[] leftScoreEffectTextCachedTransformArray;
        private Transform[] rightScoreEffectTextCachedTransformArray;

        private CanvasGroup[] leftScoreEffectTextCanvasGroupArray;
        private CanvasGroup[] rightScoreEffectTextCanvasGroupArray;

        private Vector3 leftStartingPosition = Vector3.zero;
        private Vector3 rightStartingPosition = Vector3.zero;

        private byte leftScoreEffectIndex = 0;
        private byte rightScoreEffectIndex = 0;

        private bool playLeft = false;
        #endregion

        #region Public Methods
        public void PlayScoreEffect(uint _score)
        {
            if (playLeft == true)
            {
                ErrorCheckLeftScoreEffectIndex();
                leftScoreEffectTextArray[leftScoreEffectIndex].SetText($"{_score}+");
                PlayLeftScoreEffectTween();
                leftScoreEffectIndex++;
                playLeft = false;
            }
            else
            {
                ErrorCheckRightScoreEffectIndex();
                rightScoreEffectTextArray[rightScoreEffectIndex].SetText($"+{_score}"); ;
                PlayRightScoreEffectTween();
                rightScoreEffectIndex++;
                playLeft = true;
            }
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            SetArrays();
            SetStartingPositions();
        }

        private void SetStartingPositions()
        {
            leftStartingPosition = leftScoreEffectTextCachedTransformArray[0].localPosition;
            rightStartingPosition = rightScoreEffectTextCachedTransformArray[0].localPosition;
        }

        private void ErrorCheckLeftScoreEffectIndex()
        {
            if (leftScoreEffectIndex >= leftScoreEffectTextArray.Length)
            {
                leftScoreEffectIndex = 0;
            }
        }

        private void ErrorCheckRightScoreEffectIndex()
        {
            if (rightScoreEffectIndex >= rightScoreEffectTextArray.Length)
            {
               rightScoreEffectIndex = 0;
            }
        }

        private void SetArrays()
        {
            rightScoreEffectTextCachedTransformArray = new Transform[rightScoreEffectTextArray.Length];
            leftScoreEffectTextCachedTransformArray = new Transform[leftScoreEffectTextArray.Length];

            rightScoreEffectTextCanvasGroupArray = new CanvasGroup[rightScoreEffectTextArray.Length];
            leftScoreEffectTextCanvasGroupArray = new CanvasGroup[leftScoreEffectTextArray.Length];

            for (byte i = 0; i < rightScoreEffectTextArray.Length; i++)
            {
                rightScoreEffectTextCachedTransformArray[i] = rightScoreEffectTextArray[i].transform;
                rightScoreEffectTextCanvasGroupArray[i] = rightScoreEffectTextArray[i].GetComponent<CanvasGroup>();

                leftScoreEffectTextCachedTransformArray[i] = leftScoreEffectTextArray[i].transform;
                leftScoreEffectTextCanvasGroupArray[i] = leftScoreEffectTextArray[i].GetComponent<CanvasGroup>();
            }
        }

        private void PlayLeftScoreEffectTween()
        {
            LeanTween.cancel(leftScoreEffectTextArray[leftScoreEffectIndex].gameObject);
            leftScoreEffectTextCachedTransformArray[leftScoreEffectIndex].localPosition = leftStartingPosition;
            leftScoreEffectTextCanvasGroupArray[leftScoreEffectIndex].alpha = 0f;
  
            Vector3 endPosition = new Vector3((leftStartingPosition.x + LeftMaxTargetPositionX), 
                leftStartingPosition.y, leftStartingPosition.z);

            LeanTween.moveLocal(leftScoreEffectTextArray[leftScoreEffectIndex].gameObject, endPosition, 1f).setEaseOutExpo();
            LeanTween.alphaCanvas(leftScoreEffectTextCanvasGroupArray[leftScoreEffectIndex], 1f, 
                0.5f).setEaseOutExpo().setLoopPingPong(1);
        }

        private void PlayRightScoreEffectTween()
        {
            LeanTween.cancel(rightScoreEffectTextArray[rightScoreEffectIndex].gameObject);
            rightScoreEffectTextCachedTransformArray[rightScoreEffectIndex].localPosition = rightStartingPosition;
            rightScoreEffectTextCanvasGroupArray[rightScoreEffectIndex].alpha = 0f;

            Vector3 endPosition = new Vector3((rightStartingPosition.x + RightMaxTargetPositionX),
                rightStartingPosition.y, rightStartingPosition.z);

            LeanTween.moveLocal(rightScoreEffectTextArray[rightScoreEffectIndex].gameObject, endPosition, 1f).setEaseOutExpo();
            LeanTween.alphaCanvas(rightScoreEffectTextCanvasGroupArray[rightScoreEffectIndex], 1f,
                0.5f).setEaseOutExpo().setLoopPingPong(1);
        }
        #endregion
    }
}
