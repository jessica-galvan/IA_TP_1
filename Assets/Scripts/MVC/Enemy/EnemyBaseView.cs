using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseView : EntityView
{
    protected EnemyBaseModel _model;

    //rigidbody  get component.

    //me suscribo a los eventos del model. 
    protected override void Awake()
    {
        base.Awake();
        _model.OnActtack += AttackAnimation;
    }

    private void AttackAnimation()
    {

    }

}
