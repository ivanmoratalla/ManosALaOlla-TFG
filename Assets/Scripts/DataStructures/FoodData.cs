using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodType
{
    Raw,
    Processed,
    Cooked
}

[CreateAssetMenu(fileName = "New Food", menuName = "New Food")]
public class FoodData : ScriptableObject
{
    [SerializeField] private string foodName;       // Nombre de la comida o el ingrediente
    [SerializeField] private FoodType foodType;     // Tipo de comida
    [SerializeField] private GameObject foodPrefab; 

    //private int action; //Acción a realizar sobre la comida
    //private int timeToPrepare; // Tiempo en realizar la acción


    public string getName()
    {
        return foodName; 
    }

    public FoodType getType()
    {
        return foodType;
    }

}
