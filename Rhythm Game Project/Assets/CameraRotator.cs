namespace Settings
{
    using UnityEngine;

    public sealed class CameraRotator : MonoBehaviour
    {
        #region Constants
        public const float Sensitivity = 0.01f;

        private const string AxisY = "Mouse Y";
        private const string AxisX = "Mouse X";
        #endregion

        #region Private Fields
        [SerializeField] private Transform rotatorCachedTransform;

        private float newRotationX = 0f;
        private float newRotationY = 0f;

        private Vector3 lastFrameMousePosition;
        #endregion

        #region Private Methods
        private void Update()
        {
            if (Input.mousePosition != lastFrameMousePosition)
            {
                UpdateRotation();
            }

            UpdateLastFrameMousePosition();
        }

        private void UpdateRotation()
        {
            newRotationX = rotatorCachedTransform.localEulerAngles.x - Input.GetAxis(AxisY) * Sensitivity;
            newRotationX = Mathf.Clamp(newRotationX, 0f, 2f);

            newRotationY = rotatorCachedTransform.localEulerAngles.y + Input.GetAxis(AxisX) * Sensitivity;
            newRotationY = Mathf.Clamp(newRotationY, 0f, 2f);

            rotatorCachedTransform.localEulerAngles = new Vector3(newRotationX, newRotationY, 0);
        }

        private void UpdateLastFrameMousePosition()
        {
            lastFrameMousePosition = Input.mousePosition;
        }
        #endregion
    }
}