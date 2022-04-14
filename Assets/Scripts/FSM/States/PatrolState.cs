using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState<T> : CooldownState<T>
{
    private IModel _model;

    public PatrolState(IModel model, float runTime, INode root = null) : base(runTime, root)
    {
        _model = model;
    }

    public override void Init()
    {
        base.Init();
        //TODO: implementar sistema de waypoints de progra 2. Incluir bool para "revertir" o no. en caso de que sea una linea recta en vez de un area
    }
}
