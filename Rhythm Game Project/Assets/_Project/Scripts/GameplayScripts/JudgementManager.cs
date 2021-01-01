namespace Gameplay
{
    using UnityEngine;

    public sealed class JudgementManager : MonoBehaviour
    {
        #region Private Fields
        private uint perfect = 0;
        private uint great = 0;
        private uint okay = 0;
        private uint miss = 0;
        #endregion

        #region Public Methods
        public void IncreaseOkayCount()
        {
            okay++;
        }

        public void IncreaseGreatCount()
        {
            great++;
        }

        public void IncreasePerfectCount()
        {
            perfect++;
        }

        public void IncreaseMissCount()
        {
            miss++;
        }
        #endregion
    }
}
