using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodAction
{
    Cut,
    Fry,
    Cook,
    Burn,
    Roast
}

[System.Serializable]
public class FoodTransition
{
    [SerializeField] private FoodAction action;             // Acción a realizar en esta transición
    [SerializeField] private GameObject nextStatePrefab;    // Prefab del estado al que se transiciona

    public FoodAction GetAction()
    {
        return action; 
    }

    public GameObject GetNextStatePrefab()
    {
        return nextStatePrefab;
    }
}
