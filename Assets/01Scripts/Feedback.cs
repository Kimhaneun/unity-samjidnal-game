using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Feedback : MonoBehaviour
{
    public abstract void CreateFeedback();
    public abstract void FinishFeedback();

    protected Entity _entity;

    protected virtual void Awake()
    {
        _entity = GetComponent<Entity>();
    }

    private void OnDisable()
    {
        FinishFeedback();
    }
}
