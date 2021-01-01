namespace SceneManager
{
    using UnityEngine;

    public sealed class SceneManager : MonoBehaviour
    {
        #region Constants
        public const byte GameplaySceneIndex = 0;
        #endregion

        #region Private Fields
        private byte sceneIndex = 0;
        #endregion

        #region Properties
        public byte SceneIndex => sceneIndex;
        #endregion
    }
}