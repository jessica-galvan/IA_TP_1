using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseView : EntityView
{
    //rigidbody  get component.
    //me suscribo a los eventos del model. 
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();

        if(_model is IAttack)
            (_model as IAttack).OnAttack += AttackAnimation;
    }

    private void AttackAnimation()
    {

    }

}
