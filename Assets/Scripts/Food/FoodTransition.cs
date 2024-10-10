using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodAction
{
    Cut,
    Fry
}

[System.Serializable]
public class FoodTransition
{
    [SerializeField] private FoodAction action; // Acci�n a realizar en esta transici�n
    [SerializeField] private FoodStateData nextState; // Siguiente estado al que se transiciona

    public FoodAction getAction()
    {
        return action; 
    }

    public FoodStateData getNextState()
    {
        return nextState;
    }
}
