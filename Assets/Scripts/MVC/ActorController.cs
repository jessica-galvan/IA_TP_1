using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    private ActorModel _model;

    protected virtual void Awake()
    {
        _model = GetComponent<ActorModel>();
    }

}
