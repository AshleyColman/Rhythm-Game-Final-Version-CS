using UnityEngine;

public sealed class PlayerSettings : MonoBehaviour 
{
    #region Private Fields
    private double approachTime = 1;
    #endregion

    #region Properties
    public double ApproachTime => approachTime;
	#endregion
}
