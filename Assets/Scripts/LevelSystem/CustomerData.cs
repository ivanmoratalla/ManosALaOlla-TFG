using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomerData
{
    [SerializeField] private string name;           // Nombre del cliente.
    [SerializeField] private Recipe dish;           // Plato que va a pedir.

    public string getName()
    {
        return name;
    }

    public Recipe getDish()
    {
        return dish;
    }
}
