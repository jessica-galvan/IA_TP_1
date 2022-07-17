using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    [SerializeField] private int capacity = 10;
    [SerializeField] private float radius = 2f;
    [SerializeField] private LayerMask layerMask;

    List<IFlocking> _behaviours = new List<IFlocking>();
    IArtificialMovement _entity;
    Collider[] _colls;

    private void Awake()
    {
        _entity = GetComponent<IArtificialMovement>();
        _colls = new Collider[capacity];
        InitializedBehaviour();
    }

    private void InitializedBehaviour()
    {
        var behaviours = GetComponents<IFlocking>();
        _behaviours = new List<IFlocking>(behaviours);
    }

    private void Update()
    {
        List<Transform> boids = GetBoidsList();
        Vector3 dir = GetDir(boids);
        
        _entity.Move(transform.forward);
        
        if(dir != Vector3.zero)
            _entity.LookDir(dir);
    }

    private List<Transform> GetBoidsList()
    {
        int countColl = Physics.OverlapSphereNonAlloc(transform.position, radius, _colls, layerMask);
        var boids = new List<Transform>();

        for (int i = 0; i < countColl; i++)
        {
            if (_colls[i].transform == transform) continue; //let's skip myself
            boids.Add(_colls[i].transform);
        }

        return boids;
    }

    private Vector3 GetDir(List<Transform> boids)
    {
        Vector3 dir = Vector3.zero;
        for (int i = 0; i < _behaviours.Count; i++)
        {
            var curr = _behaviours[i].GetDir(boids, this.transform);
            dir += curr;
        }

        return dir;
    }

}
