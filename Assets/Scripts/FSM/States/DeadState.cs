using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState<T> : State<T>
{
    //private float _timeToDead;
    //private float _counter;
    private IModel _model;

    public DeadState(IModel entity, float timeToDead)
    {
        _model = entity;
        //_timeToDead = timeToDead;
    }

    public override void Init()
    {
        _model.DieAnimation();
        //_counter = _timeToDead;
        //_model?.LifeController.TakeDamage(_model.LifeController.CurrentLife); //Por ahora lo mata sacandole toda la vida.
    }

    public override void Execute()
    {
        //_counter -= Time.deltaTime; //TODO: CHECK si es necesario si pongo un evento que se incie cuando termina la animación?
        //if (_counter <= 0)
        //{
        //    //Debug.Log(_model.gameObject.name + " DIE EXECUTE");
        //    //GameObject.Destroy(_model.gameObject);
        //}
    }

    //clase nueva que se dedica a hacer un cooldown una que chequee cuando hago el punto medio, otra para el final del ataque. para transicionar. 
}
