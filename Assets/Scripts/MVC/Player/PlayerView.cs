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
        _controller.OnMove += RunAnimation;
    }

    protected virtual void RunAnimation(float x, float y)
    {
        _animator?.SetBool("Run", x > 0 || y > 0);
    }

    protected virtual void AttackAnimation()
    {
        print("ANIMATION");
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
        base.OnDestroy();
    }
}
