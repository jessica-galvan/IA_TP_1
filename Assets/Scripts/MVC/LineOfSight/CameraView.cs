using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraView : MonoBehaviour
{
    public Image iconSnakeSpiderman;

    private CameraModel _model;

    void Start()
    {
        _model = GetComponent<CameraModel>();
    }

    void Update()
    {
        iconSnakeSpiderman.gameObject.SetActive(_model.IsDetectedTargets);
    }
}
