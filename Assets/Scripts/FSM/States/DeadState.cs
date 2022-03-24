using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState<T> : State<T>
{
    private float _timeToDead;
    private float _counter;
    private Actor _actor;

    public DeadState(Actor entity, float timeToDead)
    {
        _actor = entity;
        _timeToDead = timeToDead;
    }

    public override void Init()
    {
        _counter = _timeToDead;
        _actor?.LifeController.TakeDamage(_actor.LifeController.CurrentLife); //Por ahora lo mata sacandole toda la vida.
    }

    public override void Execute()
    {
        _counter -= Time.deltaTime; //TODO: CHECK si es necesario si pongo un evento que se incie cuando termina la animación?
        if (_counter <= 0)
        {
            GameObject.Destroy(_actor);
        }
    }
}
