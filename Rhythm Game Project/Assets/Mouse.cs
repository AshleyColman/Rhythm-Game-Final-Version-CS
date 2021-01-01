namespace Settings
{
    using UnityEngine;

    public sealed class Mouse : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private GameObject mouse = default;

        private Transform mouseCachedTransform;

        private Vector3 lastFrameMousePosition;
        #endregion

        #region Private Methods
        private void Awake()
        {
            Cursor.visible = false;
            lastFrameMousePosition = Input.mousePosition;
            mouseCachedTransform = mouse.transform;
        }

        private void Update()
        {
            if (Input.mousePosition != lastFrameMousePosition)
            {
                UpdateMousePosition();
            }

            UpdateLastFrameMousePosition();
        }

        private void UpdateMousePosition()
        {
            mouseCachedTransform.position = Input.mousePosition;
        }

        private void UpdateLastFrameMousePosition()
        {
            lastFrameMousePosition = Input.mousePosition;
        }
        #endregion
    }
}
