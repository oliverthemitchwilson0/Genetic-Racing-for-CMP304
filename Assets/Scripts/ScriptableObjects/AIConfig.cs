using UnityEngine;

[CreateAssetMenu(fileName = "AIConfig", menuName = "Scriptable Objects/AIConfig")]
public class AIConfig : ScriptableObject
{
    [Header("Ray Config")]
    public LayerMask trackLayer;
    public float[] rayAngles;
    public float[] rayDistances;
}
