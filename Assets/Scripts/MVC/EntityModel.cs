using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LifeController))]
public abstract class EntityModel : MonoBehaviour, IDamagable, IModel
{
    [Header("Stats")]
    [SerializeField] protected ActorStats _actorStats;
    [SerializeField] protected AttackStats _attackStats;
    [SerializeField] protected float deathTimer = 4f;

    //PROPIEDADES
    public ActorStats ActorStats => _actorStats;
    public AttackStats AttackStats => _attackStats;
    public LifeController LifeController { get; private set; }

    public Action OnIdle { get => _onIdle; set => _onIdle = value; }
    protected Action _onIdle = delegate { };

    public Action OnHit{ get => _onHit; set => _onHit = value; }
    protected Action _onHit = delegate { };

    public Action OnDie { get => _onDie; set => _onDie = value; }
    protected Action _onDie = delegate { };


    protected virtual void Awake()
    {
        LifeController = GetComponent<LifeController>();
    }

    protected virtual void Start()
    {
        InitStats();
    }

    protected virtual void InitStats()
    {
        LifeController.SetStats(_actorStats);
    }

    protected IEnumerator DieTimer(float time)
    {
        yield return new WaitForSeconds(time);
        OnDeath();
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    public virtual void IdleAnimation()
    {
        OnIdle?.Invoke();
    }

    public virtual void GetHitAnimation()
    {
        OnHit?.Invoke();
    }

    public virtual void DieAnimation()
    {
        OnDie?.Invoke();
        StartCoroutine(DieTimer(deathTimer));
    }



}
