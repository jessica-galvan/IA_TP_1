using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState<T> : CooldownState<T>
{
    private IAttack _model;

    public AttackState(IAttack model, float timeAttack, INode root) : base(timeAttack, root)
    {
        _model = model;
    }

    public override void Init()
    {
        base.Init();
        Debug.Log("attack");
        //TODO: CALL TO ATTACK ANIMATION
        //_actor.OnAttack();
        //_anim.Play("Capoeira");
    }

    public override void Execute()
    {
        Debug.Log( " execute attack");

        var objs = _model.CheckTargetsInRadious();
        if(objs != null && objs.Length > 0)
        {
            for (int i = objs.Length - 1; i >= 0; i--)
            {
                Debug.Log(objs[i].name + " was attacked");
                _model.Attack();
            }

            _root.Execute();
        }
        base.Execute();
    }
}
