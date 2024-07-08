using System;
using System.Collections;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    #region COMPONENTS
    public Animator Animator { get; protected set; }
    public SpriteRenderer SpriteRenderer { get; protected set; }
    public DamageCaster DamageCaster { get; protected set; }
    public Health Health { get; protected set; }
    public IMovement Movement { get; protected set; }
    #endregion

    public bool CanStateChangeable { get; protected set; } = true;
    public bool IsDead { get; protected set; } 

    [SerializeField] protected EntityStat _entityStat;
    public EntityStat EntityStat => _entityStat;

    protected virtual void Awake()
    {
        Movement = GetComponent<IMovement>();

        Movement.Initialize(this);

        Transform damageCasterTransform = transform.Find("DamageCaster");
        if(damageCasterTransform != null)
        {
            DamageCaster = damageCasterTransform.GetComponent<DamageCaster>();
            DamageCaster.Initialize(this);
        }

        Transform visualTransform = transform.Find("Visual");
        Animator = visualTransform.GetComponent<Animator>();
        SpriteRenderer = visualTransform.GetComponent<SpriteRenderer>();

        _entityStat = Instantiate(_entityStat);
        _entityStat.SetEntity(this);

        Health = GetComponent<Health>();
        Health?.Initialize(this);
    }

    protected virtual void Start() { }

    #region DELAY CALLBACK COROUTINE
    public Coroutine StartDelayCallback(float delayTime, Action Callback)
    {
        return StartCoroutine(DelayCoroutine(delayTime, Callback));
    }
    #endregion

    #region VELOCITY CONTROL
   

    public void StopImmediately(bool withAxisY, bool withAxisZ)
    {
        //if (withAxisY && withAxisZ)
        //    RB.velocity = Vector3.zero;
        //else if (withAxisY)
        //    RB.velocity = new Vector3(0, 0, RB.velocity.z);
        //else if (withAxisZ)
        //    RB.velocity = new Vector3(0, RB.velocity.y, 0);
        //else
        //    RB.velocity = new Vector3(RB.velocity.x, 0, 0);
    }
    #endregion

    protected IEnumerator DelayCoroutine(float delayTime, Action callback)
    {
        yield return new WaitForSeconds(delayTime);
        callback?.Invoke();
    }

    public abstract void Attack();

    public abstract void Death();
}
