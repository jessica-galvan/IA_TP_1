using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownState<T> : State<T>
{
    protected float _time;
    protected float _counter;
    protected INode _root;

    public CooldownState(float time, INode root = null)
    {
        _time = time;
        _root = root;
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
            _counter = _time;
            if (_root != null)
                _root.Execute();

        }
    }
}
