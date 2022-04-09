using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArtificial
{
    IAStats IAStats { get; }

    Transform transform { get;}
}
