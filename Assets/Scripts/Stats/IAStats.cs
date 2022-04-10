using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IAStats", menuName = "Stats/IAStats", order = 2)]
public class IAStats : ScriptableObject
{
    public LayerMask ObstacleList => _obstacleList;
    [SerializeField] private LayerMask _obstacleList;

    public float AngleAvoidance => _angleAvoidance;
    [SerializeField] private float _angleAvoidance = 270f;
    public float RangeAvoidance => _rangeAvoidance;
    [SerializeField] private float _rangeAvoidance = 1f;

    public float SteeringWeight => _steeringWeight;
    [SerializeField] private float _steeringWeight = 1f;

    public float AvoidanceWeight => _avoidanceWeight;
    [SerializeField] private float _avoidanceWeight = 1f;

    public float TimePrediction => _timePrediction;
    [SerializeField] private float _timePrediction = 5f;

    public float MaxDistanceFromTarget => _maxDistanceFromTarget;
    [SerializeField] private float _maxDistanceFromTarget = 5f;    
}
