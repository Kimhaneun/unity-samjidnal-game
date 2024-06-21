using UnityEngine;

public enum HitTypeEnum
{

}

public struct ImpactInfo
{
    public Vector3 hitPoint;
    public Vector3 hitDirection;
    public HitTypeEnum hitTypeEnum;
}