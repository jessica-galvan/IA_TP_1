using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardView : EnemyBaseView
{
    private WizardController _controller;

    protected override void Awake()
    {
        base.Awake();
        _model = GetComponent<WizardModel>();
        _controller = GetComponent<WizardController>();
    }

    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();

        if (_model is IArtificialMovement)
            (_model as IArtificialMovement).OnMove += RunAnimation;
    }

    protected virtual void RunAnimation(bool value)
    {
        _animator?.SetBool("Run", value);
    }
}
