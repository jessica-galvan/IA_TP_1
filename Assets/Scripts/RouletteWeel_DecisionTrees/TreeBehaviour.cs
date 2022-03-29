using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    public bool shootRange;
    public bool lineOfSight;
    public bool ammoQuantity;
    public bool enemyList;

    private INode _root; //Este es el nodo que inicia el arbol

    private void InitializedTree()
    {



        //ALL ACTION NODES
        //INode shoot = new ActionNode(ShootAction); //Si es una funcion de más de una linea, este es el mdoo clasico
        INode shoot = new ActionNode(()=>print("Shoot")); //lambda: para ahorrar lineas, usamos el base. 
        INode rechargeAmmo = new ActionNode(()=>print("Recharge AMMO"));
        INode patrol = new ActionNode(()=>print("Patrol"));
        INode chase = new ActionNode(() => print("Chase Enemy"));


        Dictionary<INode, int> itemsRandom = new Dictionary<INode, int>();
        itemsRandom[shoot] = 25;
        itemsRandom[chase] = 15;

        INode random = new RandomNode(itemsRandom);

        //AL QUESTIONS NODE
        INode qShootRange = new QuestionNode(ShootRange, shoot, chase);
        INode qLineOfSight = new QuestionNode(CheckLineOfSight, qShootRange, patrol);
        INode qAmmo = new QuestionNode(CheckAmmo, qLineOfSight, rechargeAmmo);
        INode qEnemyList = new QuestionNode(()=> enemyList, qAmmo, patrol); //Version lambda

        _root = qEnemyList; //Declaramos el nodo incial
    }

    void ShootAction()
    {
        print("Shoot");
    }

    bool ShootRange()
    {
        return shootRange;
    }

    bool CheckLineOfSight()
    {
        return lineOfSight;
    }

    bool CheckAmmo()
    {
        return ammoQuantity;
    }

    private void Awake()
    {
        InitializedTree();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //Esto es para reiniciar el arbol.
        {
            _root.Execute();
        }
    }
}
