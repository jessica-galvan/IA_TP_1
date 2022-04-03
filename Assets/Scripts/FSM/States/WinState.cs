using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinState<T> : State<T>
{
    Animator _anim;
    public WinState(Animator anim)
    {
        _anim = anim;
    }
    public override void Init()
    {
        base.Init();
        //TODO: change this one for real win?
    }
}
