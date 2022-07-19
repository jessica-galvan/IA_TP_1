using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteppedColors : MonoBehaviour
{
    public Shader shad;

    [Range(0, 1)]
    public float intensity;
    [Range(0,1)]
    public float step1, step2, step3;

    Material mat;


    // Start is called before the first frame update
    void Start()
    {
        mat = new Material(shad);
    }

    // Update is called once per frame
    void Update()
    {
        mat.SetFloat("_Intensity", intensity);
        mat.SetFloat("_Step1", step1);
        mat.SetFloat("_Step2", step2);
        mat.SetFloat("_Step3", step3);

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
    }
}
