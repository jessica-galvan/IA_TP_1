using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreView : EnemyBaseView
{
    private OgreController _controller;

    protected override void Awake()
    {
        base.Awake();
        _model = GetComponent<OgreModel>();
        _controller = GetComponent<OgreController>();
    }

    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();

        if (_model is IArtificialMovement)
            (_model as IArtificialMovement).OnMove += RunAnimation;
    }

    protected override void Idle()
    {
        _animator?.Play("Idle");
    }

    protected override void AttackAnimation()
    {
        base.AttackAnimation();
        RunAnimation(false);
    }


    protected virtual void RunAnimation(bool value)
    {
        _animator?.SetBool("Movement", value);
    }
}
