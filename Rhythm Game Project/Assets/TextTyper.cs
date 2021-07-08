namespace Menu
{
    using System.Collections;
    using System.Text;
    using TMPro;
    using UnityEngine;

    public sealed class TextTyper : MonoBehaviour
    {
        #region Constants
        public const float PerLetterTypeDuration = 0.02f;
        #endregion

        #region Private Fields
        private IEnumerator typeTextCancelCoroutine;
        private IEnumerator typeTextNoCancelCoroutine;
        #endregion

        #region Public Methods
        public float GetTextTypeDuration(string _string)
        {
            return _string.Length * PerLetterTypeDuration;
        }

        public void TypeTextNoCancel(string _string, TextMeshProUGUI _text)
        {
            typeTextNoCancelCoroutine = TypeTextNoCancelCoroutine(_string, _text);
            StartCoroutine(typeTextNoCancelCoroutine);
        }

        public void TypeTextNoCancel(string _string, TextMeshProUGUI _text1, TextMeshProUGUI _text2)
        {
            typeTextNoCancelCoroutine = TypeTextNoCancelCoroutine(_string, _text1, _text2);
            StartCoroutine(typeTextNoCancelCoroutine);
        }

        public void TypeTextCancel(string _string, TextMeshProUGUI _text)
        {
            if (typeTextCancelCoroutine != null)
            {
                StopCoroutine(typeTextCancelCoroutine);
            }

            typeTextCancelCoroutine = TypeTextCancelCoroutine(_string, _text);
            StartCoroutine(typeTextCancelCoroutine);
        }

        public void TypeTextCancel(string _string, TextMeshProUGUI _text1, TextMeshProUGUI _text2)
        {
            if (typeTextCancelCoroutine != null)
            {
                StopCoroutine(typeTextCancelCoroutine);
            }

            typeTextCancelCoroutine = TypeTextCancelCoroutine(_string, _text1, _text2);
            StartCoroutine(typeTextCancelCoroutine);
        }
        #endregion

        #region Private Methods
        private IEnumerator TypeTextNoCancelCoroutine(string _string, TextMeshProUGUI _text)
        {
            WaitForSeconds textSpeedWaitForSeconds = new WaitForSeconds(PerLetterTypeDuration);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _string.Length; i++)
            {
                sb.Append(_string[i]);

                _text.SetText(sb);

                yield return textSpeedWaitForSeconds;
            }

            yield return null;
        }

        private IEnumerator TypeTextNoCancelCoroutine(string _string, TextMeshProUGUI _text1, TextMeshProUGUI _text2)
        {
            WaitForSeconds textSpeedWaitForSeconds = new WaitForSeconds(PerLetterTypeDuration);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _string.Length; i++)
            {
                sb.Append(_string[i]);

                _text1.SetText(sb);
                _text2.SetText(sb);

                yield return textSpeedWaitForSeconds;
            }

            yield return null;
        }

        private IEnumerator TypeTextCancelCoroutine(string _string, TextMeshProUGUI _text)
        {
            WaitForSeconds textSpeedWaitForSeconds = new WaitForSeconds(PerLetterTypeDuration);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _string.Length; i++)
            {
                sb.Append(_string[i]);

                _text.SetText(sb);

                yield return textSpeedWaitForSeconds;
            }

            yield return null;
        }

        private IEnumerator TypeTextCancelCoroutine(string _string, TextMeshProUGUI _text1, TextMeshProUGUI _text2)
        {
            WaitForSeconds textSpeedWaitForSeconds = new WaitForSeconds(PerLetterTypeDuration);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _string.Length; i++)
            {
                sb.Append(_string[i]);

                _text1.SetText(sb);
                _text2.SetText(sb);

                yield return textSpeedWaitForSeconds;
            }

            yield return null;
        }
        #endregion
    }
}