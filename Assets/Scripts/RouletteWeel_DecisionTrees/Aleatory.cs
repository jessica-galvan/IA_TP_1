using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyEngine
{    public class Aleatory
    {
        public static float Range(float min, float max)
        {
            return (max - min) * Random.value + min;
        }

        public static T Roulette<T>(Dictionary<T, float> items)
        {
            float total = 0;
            foreach (var item in items)
            {
                total += item.Value;
            }

            float random = Random.value;

            foreach (var item in items)
            {
                float currentValue = item.Value / total;
                if(currentValue >= random) //Si es mayor, estamos en rango
                    return item.Key;
                else
                    random -= currentValue; //Si es menor, le restamos eso y volvemos a hacer la vuelta.
            }
            return default(T);
        }

        public static T[] Shuffle<T>(T[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                var aux = items[i];
                int indexRand = (int)Range(0, items.Length);
                items[i] = items[indexRand];
                items[indexRand] = aux;
            }

            return items;
        }

        public static List<T> Shuffle<T>(List<T> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var aux = items[i];
                int indexRand = (int)Range(0, items.Count);
                items[i] = items[indexRand];
                items[indexRand] = aux;
            }

            return items;
        }
    }
}

