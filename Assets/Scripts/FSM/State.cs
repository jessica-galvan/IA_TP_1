using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State<T> : IState<T>
{

    Dictionary<T, IState<T>> _transitions = new Dictionary<T, IState<T>>();


    public void AddTransition(T input, IState<T> state)
    {
        //if (!_transitions.ContainsKey(input))  //NO CONVIENE
        //    _transitions.Add(input, state); //porque si quiere sobreesribirlo, no puedo

        _transitions[input] = state; //En cambio acá, si no existe, lo crea. Y si existe, lo sobreescribe.
    }

    public void RemoveTransition(T input)
    {
        if (_transitions.ContainsKey(input))
            _transitions.Remove(input);
    }

    public void RemoveTransition(IState<T> state)
    {
        foreach (var item in _transitions)
        {
            if (item.Value == state)
                _transitions.Remove(item.Key);
        }
    }

    public IState<T> GetTransition(T input)
    {
        if (_transitions.ContainsKey(input))
            return _transitions[input];
        else 
            return null;
    }

    public virtual void Init()
    {
       
    }

    public virtual void Execute()
    {
        
    }

    public virtual void Exit()
    {

    }
}
