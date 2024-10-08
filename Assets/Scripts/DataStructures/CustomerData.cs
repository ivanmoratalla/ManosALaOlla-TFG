using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomerData
{
    [SerializeField] private string name;           // Nombre del cliente.
    [SerializeField] private string dish;           // Plato que va a pedir.
    [SerializeField] private float arrivalTime;  // Tiempo que tarda en llegar al restaurante (para que se vayan uniendo progresivamente)

    public string getName()
    {
        return name;
    }

    public string getDish()
    {
        return dish;
    }

    public float getArrivalTime()
    {
        return arrivalTime;
    }
}
