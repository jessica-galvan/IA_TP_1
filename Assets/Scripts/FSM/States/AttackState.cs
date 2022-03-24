using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState<T> : CooldownState<T>
{
    private Actor _actor;
    private T _inputWin;
    public AttackState(Actor actor, float timeAttack, FSM<T> fsm, T input, T inputWin) : base(timeAttack, fsm, input)
    {
        _actor = actor;
        _inputWin = inputWin;
    }

    public override void Init()
    {
        base.Init();
        Debug.Log(_actor.name + " attack");
        //TODO: CALL TO ATTACK ANIMATION
        //_actor.OnAttack();
        //_anim.Play("Capoeira");
    }

    public override void Execute()
    {
        Debug.Log(_actor.name + " execute attack");
        //TODO: CHECK ENEMY (Is in front of you? is in range? raycast or do if 
        //var objs = _batman.CheckEnemies();
        //if (objs != null && objs.Length > 0)
        //{
        //    for (int i = objs.Length - 1; i >= 0; i--)
        //    {
        //        _batman.Attack(objs[i]);
        //    }
        //    _fsm.Transition(_inputWin);
        //}

        _fsm.Transition(_inputWin);
        base.Execute();
    }
}
