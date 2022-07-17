using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorBehaviour : MonoBehaviour, IFlocking
{
    [SerializeField] private int capacity = 10;
    [SerializeField] private float _range;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float _multiplier;
    private Collider[] _colls;

    public float Multiplier { get => _multiplier; set => _multiplier = value; }

    private void Awake()
    {
        _colls = new Collider[capacity];
    }

    public Vector3 GetDir(List<Transform> boids, Transform self)
    {
        Vector3 dir = Vector3.zero;

        int countColl = Physics.OverlapSphereNonAlloc(self.position, _range, _colls, layerMask);

        for (int i = 0; i < countColl; i++)
        {
            Vector3 dirSeparation = self.position - _colls[i].transform.position;
            dir += dirSeparation;
        }

        if (countColl > 0)
            dir /= countColl;

        return dir * Multiplier;
    }
}
