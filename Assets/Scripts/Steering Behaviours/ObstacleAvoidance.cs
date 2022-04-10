using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : ISteering
{
    IArtificialMovement _stats;
   
    public ObstacleAvoidance(IArtificialMovement stats)
    {
        _stats = stats;
    }

    public Vector3 GetDir()
    {
        //Primero hacemos un overlap sphere para sacar todos los posibles obstaculos que hay alrededor. 
        Collider[] obs = Physics.OverlapSphere(_stats.transform.position, _stats.IAStats.RangeAvoidance, _stats.IAStats.ObstacleList);
        Collider nearerObs = null;
        float nearDistance = 0;

        for (int i = 0; i < obs.Length; i++) //chequeamos cual es el más cercano 
        {
            Collider currObs = obs[i];
            Vector3 dir = currObs.transform.position - _stats.transform.position;
            float currAngle = Vector3.Angle(_stats.transform.forward, dir);

            if(currAngle < _stats.IAStats.AngleAvoidance / 2)
            {
                float currentDistance = Vector3.Distance(_stats.transform.position, currObs.transform.position); 
                if (nearerObs == null || nearDistance > currentDistance) //si no tenemos uno guardado o si la distancia es menor del nuevo, pisamos el existente.
                {
                    nearDistance = currentDistance;
                    nearerObs = currObs;
                }
            }
        }

        if(nearerObs != null)
        {
            var point = nearerObs.ClosestPoint(_stats.transform.position);

            Vector3 dir = _stats.transform.position + _stats.transform.right * 0.00001f - point; //la suma del _entitity right es por si JUSTO esta yendo de frente, el player salto la pared y el enemigo no puede hacer lo mismo, por lo que le da como que tiene que ir de frente. 
            
            if (nearDistance == _stats.IAStats.RangeAvoidance) //para evitar dividir a 0. 
                nearDistance = _stats.IAStats.RangeAvoidance - 0.01f;

            dir = dir * (_stats.IAStats.RangeAvoidance - nearDistance / _stats.IAStats.RangeAvoidance); //Con este paso nos fijamos CUAN cerca esta el objeto. Si esta a la misma distancia que el radius, esta lo más lejos de lo detectable. 
            return dir.normalized;
        }

        return Vector3.zero;
    }
}
