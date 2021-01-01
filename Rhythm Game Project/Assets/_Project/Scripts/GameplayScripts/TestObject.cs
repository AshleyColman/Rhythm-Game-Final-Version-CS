namespace Gameplay
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class TestObject : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup = default;
        bool keyUp = false;
        private void Update()
        {
            if (keyUp == false)
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    LeanTween.cancel(this.gameObject);
                    LeanTween.scale(this.gameObject, new Vector3(1.25f, 1.25f, 1f), 0.25f);

                    LeanTween.cancel(canvasGroup.gameObject);
                    LeanTween.alphaCanvas(canvasGroup, 1f, 0.25f);
                    keyUp = false;
                }
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                keyUp = true;
            }
            else
            {
                keyUp = false;
            }
            //else if (Input.GetKeyUp(KeyCode.D))
            //{
            //    LeanTween.cancel(this.gameObject);
            //    LeanTween.scale(this.gameObject, Vector3.one, 1f);

            //    LeanTween.cancel(canvasGroup.gameObject);
            //    LeanTween.alphaCanvas(canvasGroup, 0f, 0.25f);
            //}
        }
    }
}
