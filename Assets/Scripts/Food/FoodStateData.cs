using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Food", menuName = "New Food")]
public class FoodStateData : ScriptableObject
{
    [SerializeField] private string foodName;       // Nombre del estado (una comida o ingredinte)
    [SerializeField] private GameObject foodPrefab; 

    //private int action; //Acción a realizar sobre la comida
    //private int timeToPrepare; // Tiempo en realizar la acción


    public string getName()
    {
        return foodName; 
    }

}
