using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    private GameObject[] patrolNodes;
    public Transform[] PatrolPoints => patrolPoints;
    public GameObject[] PatrolNodes => patrolNodes;

    public void Initialize()
    {
        ConvertToArray();
        if (patrolPoints.Length > 1)
            ConvertToNodes();
    }

    private void ConvertToArray() //Toma del gameobject todos los transforms de los hijos y los pone en un array
    {
        patrolPoints = new Transform[this.transform.childCount + 1];
        patrolPoints[0] = this.transform; //Tomamos como 0 el punto de partida.
        for (int i = 0; i < this.transform.childCount; i++)
        {
            patrolPoints[i + 1] = this.transform.GetChild(i).GetComponent<Transform>();
        }
    }

    private void ConvertToNodes() //Y aca los instancia a objetos para que no se muevan con el enemigo. 
    {
        patrolNodes = new GameObject[patrolPoints.Length];

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            GameObject aux = new GameObject("PatrolNode "+ i);
            aux.transform.position = patrolPoints[i].transform.position;
            if (GameManager.instance.PatrolNodeParent != null)
                aux.transform.parent = GameManager.instance.PatrolNodeParent;
            patrolNodes[i] = aux;
        }

    }

    private void OnDestroy()
    {
        for (int i = patrolNodes.Length - 1; i >= 0; i--)
        {
            Destroy(patrolNodes[i]);
        }
    }
}
