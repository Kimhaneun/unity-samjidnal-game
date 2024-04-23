using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MonoBehaviour
{
    #region COMPONENTS
    public Rigidbody RB { get; protected set; }
    public Animator Animator { get; protected set; }
    public SpriteRenderer SpriteRenderer { get; protected set; }
    public Collider Collider { get; protected set; }
    #endregion

    protected virtual void Awake()
    {
        RB = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();

        Transform visualTransform = transform.Find("Visual");
        Animator = visualTransform.GetComponent<Animator>();
        SpriteRenderer = visualTransform.GetComponent<SpriteRenderer>();
    }

    // Attack, Die, Delay 등을 구현 하기도 하는데 아직은 구현하지 말자
}
