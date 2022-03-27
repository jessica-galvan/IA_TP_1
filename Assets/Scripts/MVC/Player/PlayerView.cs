using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : EntityView
{

    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();

    }

    protected virtual void OnRun()
    {
        _animator?.SetTrigger("RunFoward");
    }

    protected virtual void OnAttack()
    {
        _animator?.SetTrigger("Attack01");
    }

    protected virtual void OnDefend()
    {
        _animator?.SetTrigger("Defend");

    }

    protected virtual void OnGetHit()
    {
        _animator?.SetTrigger("GetHit");
    }
}
