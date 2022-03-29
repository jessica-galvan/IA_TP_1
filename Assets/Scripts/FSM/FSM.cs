using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM<T>
{
    IState<T> _current;

    public IState<T> GetCurrentState => _current;

    public FSM()
    {
        //Constructor que inicia sin estado
    }

    public FSM(IState<T> init) //CONSTUCTOR con info
    {
        SetInit(init);
    }

    public void SetInit(IState<T> init)
    {
        _current = init;
        _current.Init();
    }

    public void OnUpdate() //para que llame al update del actual current
    {
        if(_current != null)
            _current.Execute();
    }

    public void Transition(T input)
    {
        var newState = _current.GetTransition(input); //llamamos al input actual a ver si contiene el nuevo
        if(newState != null) //nos fijamos que EXISTA antes de transicionar
        {
            _current.Exit();
            SetInit(newState);
        }
    }
}
