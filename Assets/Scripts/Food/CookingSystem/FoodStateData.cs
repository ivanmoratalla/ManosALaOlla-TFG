using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "NewFood", menuName = "Food")]
public class FoodStateData : ScriptableObject
{
    [SerializeField] private string foodName;       // Nombre del estado (una comida o ingredinte)
    //[SerializeField] private GameObject foodPrefab;
    [SerializeField] private List<FoodTransition> transitions; // Esto son las transiciones entre estados que se puede hacer en función de la acción que se haga

    //private int action; //Acción a realizar sobre la comida
    //private int timeToPrepare; // Tiempo en realizar la acción


    public string getName()
    {
        return foodName; 
    }

    public List<FoodTransition> getTransitions()
    {
        return transitions;
    }

}
