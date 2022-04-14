using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState<T> : CooldownState<T>
{
    private IModel _model;

    public RunState(IModel model, float runTime, INode root) : base(runTime, root)
    {
        _model = model;
    }

    public override void Init()
    {
        base.Init();
        //Me suscribo al move y al attack
    }

    public override void Execute() 
    {
        base.Execute();
        //Llamo al playupdate del input controller.
        //llamo al move del player mientras me este moviendo. 

    }

    //public void Move(); //Se suscribe al move - ahce logica movimiento o le dice mov al player. 
    //public void Attack();//se suscribe al attack y llama a la transcion

}
