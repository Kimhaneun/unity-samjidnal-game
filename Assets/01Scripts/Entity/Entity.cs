using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    #region COMPONENTS
    public Rigidbody RB { get; protected set; }
    public Animator Animator { get; protected set; }
    public SpriteRenderer SpriteRenderer { get; protected set; }
    public Collider Collider { get; protected set; }
    #endregion

    public int FacingDiraction { get; protected set; }

    public bool CanStateChangeable { get; protected set; } = true;

    protected virtual void Awake()
    {
        RB = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();

        Transform visualTransform = transform.Find("Visual");
        Animator = visualTransform.GetComponent<Animator>();
        SpriteRenderer = visualTransform.GetComponent<SpriteRenderer>();
    }

    // Attack, Die, Delay 등을 구현 하기도 하는데 아직은 구현하지 말자
    #region DELAY CALLBACK COROUTINE
    public Coroutine StartDelayCallback(float delayTime, Action Callback)
    {
        return StartCoroutine(DelayCoroutine(delayTime, Callback));
    }
    #endregion

    // 여기에 속도를 제어하는 코드를 만들자.
    #region VELOCITY CONTROL
    public void SetVelocity(float x, float y, float z, bool doNotTurn = false)
    {
        RB.velocity = new Vector3(x, y, z);
    }

    public void StopImmediately(bool withAxisY, bool withAxisZ)
    {
        if (withAxisY && withAxisZ)
            RB.velocity = Vector3.zero;
        else if (withAxisY)
            RB.velocity = new Vector3(0, 0, RB.velocity.z);
        else if (withAxisZ)
            RB.velocity = new Vector3(0, RB.velocity.y, 0);
        else
            RB.velocity = new Vector3(RB.velocity.x, 0, 0);
    }
    #endregion

    protected IEnumerator DelayCoroutine(float delayTime, Action callback)
    {
        yield return new WaitForSeconds(delayTime);
        callback?.Invoke();
    }

    public abstract void Attack();
}
