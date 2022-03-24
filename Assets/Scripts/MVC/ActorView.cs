using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ActorView : MonoBehaviour
{
    protected ActorModel _model;
    protected Animator _animator;

    public Action OnDie;

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected virtual void SubscribeEvents()
    {
        _model.LifeController.OnTakeDamage += OnTakeDamage;
        _model.LifeController.OnDie += OnDeath;
    }

    protected virtual void OnIdle()
    {
        _animator?.SetTrigger("Idle");
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
}
