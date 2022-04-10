using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PidgeonController : EntityController
{

    protected override void Awake()
    {
        base.Awake();
        _model = GetComponent<PidgeonModel>();
    }

    private void Update()
    {

    }

}
