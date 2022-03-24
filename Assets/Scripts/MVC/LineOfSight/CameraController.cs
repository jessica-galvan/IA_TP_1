using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CameraModel _model;

    void Start()
    {
        _model = GetComponent<CameraModel>();
    }

    void Update()
    {
        Transform[] targets = _model.CheckTargets();

        for (int i = 0; i < targets.Length; i++)
        {
            if (_model.LineOfSight(targets[i]))
            {
                _model.IsDetectedTargets = true;
                break;
            }
            else
            {
                _model.IsDetectedTargets = false;
            }
        }

    }
}
