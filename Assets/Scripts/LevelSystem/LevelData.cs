using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField] private int levelNumber;               // Número del nivel
    [SerializeField] private List<CustomerData> customers;  // Lista de clientes que va a tener el nivel

    [SerializeField] private float preGameTime;             // Tiempo desde que se carga el nivel hasta que se inicia (para que el usuario pueda visualizar un poco el nivel)
    [SerializeField] private float gameTime;                // Tiempo de juego
    [SerializeField] private float timeBetweenCustomers;    // Tiempo que tiene que pasar entre clientes 

    [SerializeField] private int neededScore;               // Puntuación necesaria para conseguir completar un nivel

    public List<CustomerData> GetCustomers()
    {
        return customers;
    }
    
    public int GetLevelNumber()
    {
        return levelNumber;
    }

    public float GetPreGameTime()
    {
        return preGameTime;
    }

    public float GetGameTime()
    {
        return gameTime;
    }

    public int GetNeededScore()
    {
        return neededScore;
    }

    public float GetTimeBetweenCustomers()
    {
        return timeBetweenCustomers;
    }
}
