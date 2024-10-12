using UnityEngine;
using System.Collections.Generic;
using UnityEditor.PackageManager;


[CreateAssetMenu(fileName = "NewLevelData", menuName = "LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField] private List<CustomerData> customers; // Lista de clientes que va a tener el nivel
    //[SerializeField] private List<Table> tables;           // Lista de mesas que va a haber en el nivel

    public List<CustomerData> getCustomers()
    {
        return customers;
    }
    /*
    public List<Table> getTables()
    {
        return tables;
    }*/
}
