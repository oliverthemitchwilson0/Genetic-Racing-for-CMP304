using UnityEngine;

[CreateAssetMenu(fileName = "CarConfig", menuName = "Scriptable Objects/CarConfig")]
public class CarConfig : ScriptableObject
{
    public float acceleration;
    public float friction;
    public float maxSpeed;
    public float steeringStrength;
    public AnimationCurve steeringCurve;
}
