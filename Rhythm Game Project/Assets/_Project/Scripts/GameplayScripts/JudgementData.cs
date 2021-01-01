using UnityEngine;

namespace Gameplay
{
    public sealed class JudgementData : MonoBehaviour
    {
        #region Constants
        public static readonly ushort OkayScore = 50;
        public static readonly ushort GreatScore = 100;
        public static readonly ushort PerfectScore = 250;

        public static readonly byte OkayHealth = 1;
        public static readonly byte GreatHealth = 2;
        public static readonly byte PerfectHealth = 5;
        #endregion
    }
}
