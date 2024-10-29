using UnityEngine;
using System.Collections.Generic;
using UnityEditor.PackageManager;


[CreateAssetMenu(fileName = "NewLevelData", menuName = "LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField] private int levelNumber;               // Número del nivel
    [SerializeField] private List<CustomerData> customers;  // Lista de clientes que va a tener el nivel

    public List<CustomerData> getCustomers()
    {
        return customers;
    }
    
    public int getLevelNumber()
    {
        return levelNumber;
    }
}
