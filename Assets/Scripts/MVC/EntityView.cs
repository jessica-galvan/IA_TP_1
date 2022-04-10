using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class EntityView : MonoBehaviour
{
    protected IModel _model;
    protected Animator _animator;

    public Action OnDie;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected  virtual void Start()
    {
        SubscribeEvents();
    }

    protected virtual void SubscribeEvents()
    {
        //_model.LifeController.OnTakeDamage += OnTakeDamage;
        //_model.LifeController.OnDie += OnDeath;
    }

    protected virtual void Idle()
    {
        _animator?.Play("Idle");
    }

    protected virtual void OnTakeDamage()
    {
        _animator?.SetTrigger("TakeDamage");
    }

    protected virtual void OnDeath()
    {
        _animator?.SetTrigger("Die");
    }

    protected virtual void DeathAnimationOver()
    {
        OnDie?.Invoke();
    }

    protected virtual void OnDestroy()
    {
        _model.LifeController.OnTakeDamage -= OnTakeDamage;
        _model.LifeController.OnDie -= OnDeath;
    }
}
