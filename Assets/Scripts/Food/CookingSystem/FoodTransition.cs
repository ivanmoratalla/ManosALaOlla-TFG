using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodAction
{
    Cut,
    Fry,
    Cook,
    Burn
}

[System.Serializable]
public class FoodTransition
{
    [SerializeField] private FoodAction action;             // Acci�n a realizar en esta transici�n
    [SerializeField] private GameObject nextStatePrefab;    // Prefab del estado al que se transiciona

    public FoodAction getAction()
    {
        return action; 
    }

    public GameObject getNextStatePrefab()
    {
        return nextStatePrefab;
    }
}
