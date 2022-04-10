using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : EntityView
{
    private PlayerController _controller;

    protected override void Awake()
    {
        base.Awake();
        _model = GetComponent<PlayerModel>();
        _controller = GetComponent<PlayerController>();
    }

    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();
        _controller.OnAttack += AttackAnimation;

        if (_model is ITarget)
            (_model as ITarget).OnMove += RunAnimation;
    }

    protected virtual void RunAnimation(bool value)
    {
        _animator?.SetBool("Run", value);
    }

    protected virtual void AttackAnimation()
    {
        _animator?.SetTrigger("Attack");
    }

    protected virtual void OnDefend()
    {
        _animator?.SetTrigger("Defend");

    }

    protected virtual void OnGetHit()
    {
        _animator?.SetTrigger("GetHit");
    }

    protected override void OnDestroy()
    {
        if (_model is ITarget)
            (_model as ITarget).OnMove -= RunAnimation;

        base.OnDestroy();
    }
}
