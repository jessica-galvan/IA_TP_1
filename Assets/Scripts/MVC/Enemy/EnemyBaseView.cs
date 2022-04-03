using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseView : EntityView
{
    public Action OnRealAttackMoment = delegate { };
    //rigidbody  get component.
    
    //me suscribo a los eventos del model. 
    protected override void Awake()
    {
        base.Awake();
        _model = GetComponent<EnemyBaseModel>();
    }

    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();

        if(_model is IAttack)
            (_model as IAttack).OnAttack += AttackAnimation;
    }

    private void AttackAnimation()
    {
        if (!_animator.GetAnimatorTransitionInfo(0).IsName("attack01"))
        {
            print("recibi orden ataque");
            _animator.SetTrigger("Attack");
        }

    }

    private void OnRealAttackPoint()
    {
        OnRealAttackMoment?.Invoke();
    }
}
