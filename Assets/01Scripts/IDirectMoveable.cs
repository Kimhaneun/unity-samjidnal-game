using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDirectMoveable
{
    public void SetVelocity(float x, float y, float z, bool doNotTurn = false);
}
