using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeVisibility : MonoBehaviour
{
    [SerializeField] private float transparency = 0.1f;
    private Color textureColor;
    private MeshRenderer meshRend;
    private float timer = 5f;


    private void Awake()
    {
        meshRend = GetComponent<MeshRenderer>();
        //textureColor = meshRend.material.color;
    }

    public void MeshActive()
    {
        textureColor.a = transparency;  
        meshRend.material.color = textureColor;
        StartCoroutine(TurnOnAgain(timer));
    }

    private IEnumerator TurnOnAgain(float timer)
    {
        yield return new WaitForSeconds(timer);
        textureColor.a = 1f;
        meshRend.material.color = textureColor;
    }
}
