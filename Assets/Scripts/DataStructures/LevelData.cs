using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField] private int levelNumber;               // Número del nivel
    [SerializeField] private List<CustomerData> customers;  // Lista de clientes que va a tener el nivel

    [SerializeField] private float preGameTime;             // Tiempo desde que se carga el nivel hasta que se inicia (para que el usuario pueda visualizar un poco el nivel)
    [SerializeField] private float gameTime;                // Tiempo de juego

    [SerializeField] private int neededScore;               // Puntuación necesaria para conseguir completar un nivel

    public List<CustomerData> getCustomers()
    {
        return customers;
    }
    
    public int getLevelNumber()
    {
        return levelNumber;
    }

    public float getPreGameTime()
    {
        return preGameTime;
    }

    public float getGameTime()
    {
        return gameTime;
    }

    public int getNeededScore()
    {
        return neededScore;
    }
}
