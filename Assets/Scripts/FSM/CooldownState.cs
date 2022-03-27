using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownState<T> : State<T>
{
    protected float _time;
    protected float _counter;
    protected FSM<T> _fsm;
    protected T _input;
    public CooldownState(float time, FSM<T> fsm, T input)
    {
        _fsm = fsm;
        _time = time;
        _input = input;
    }
    public override void Init()
    {
        _counter = _time;
    }
    public override void Execute()
    {
        _counter -= Time.deltaTime;
        if (_counter <= 0)
        {
            _fsm.Transition(_input);
        }
    }
}
