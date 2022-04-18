using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    protected IModel _model;

    public Action OnDie;
    public Action OnGetHit;

    protected virtual void Awake()
    {
        _model = GetComponent<EntityModel>();

    }

    protected virtual void Start()
    {
        SubscribeEvents();
    }

    protected virtual void SubscribeEvents() //El input se recibe en el controller
    {
        _model.LifeController.OnTakeDamage += GetHit;
        _model.LifeController.OnDie += Die;
    }
    protected virtual void GetHit()
    {
        OnDie?.Invoke();
    }

    protected virtual void Die()
    {
        OnGetHit?.Invoke();
    }

    protected virtual bool IsDead()
    {
        return _model.LifeController.IsDead;
    }

    protected virtual void OnDestroy()
    {
        _model.LifeController.OnTakeDamage -= GetHit;
        _model.LifeController.OnDie -= Die;
    }

}
