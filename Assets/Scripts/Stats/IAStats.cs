using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IAStats", menuName = "Stats/IAStats", order = 2)]
public class IAStats : ScriptableObject
{
    public LayerMask ObstacleList => _obstacleList;
    [SerializeField] private LayerMask _obstacleList;

    public float AngleAvoidance => _angleAvoidance;
    [SerializeField] private float _angleAvoidance;
    public float RangeAvoidance => _rangeAvoidance;
    [SerializeField] private float _rangeAvoidance;

}
