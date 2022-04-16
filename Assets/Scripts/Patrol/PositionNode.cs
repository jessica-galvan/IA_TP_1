using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionNode : MonoBehaviour
{
    private PositionNode _nextNode;
    private PositionNode _previousNode;
    private PositionNode _rootNode;

    public PositionNode NextNode => _nextNode;
    public PositionNode PreviousNode => _previousNode;

    //public void InitializeNode(PositionNode previousNode, PositionNode nextNode) // Para inicializar todos los datos del nodo
    //{
    //    _previousNode = previousNode;
    //    _nextNode = nextNode;
    //}
}
