using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// Este script se encargar� de gestionar los niveles del juego, estableciendo los que est�n disponibles para jugar y los que no
public class LevelsManager : MonoBehaviour
{
    public static LevelsManager Instance;                   // Patr�n Singleton (instancia �nica)

    private const string key = "UnlockedLevels";            // Esta es la clave que voy a usar en PlayerPrefs y cuyo valor es el n�mero de niveles desbloqueados que tiene el jugador (equivalente al n�mero de nivel m�s alto que se haya desbloqueado)

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if(!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetInt(key, 1);                     // De manera predeterminada solo est� desbloqueado el primer nivel
            PlayerPrefs.Save();
        }
    }

    public void levelCompleted(int levelNumber)
    {
        int maxUnlockedLevels = PlayerPrefs.GetInt(key,1);  // Se obtiene el n�mero del nivel m�s alto que est� desbloqueado
        Debug.Log("nivel pasado: " + levelNumber);
        Debug.Log("nivel max desbloqueado: " + maxUnlockedLevels);

        if(levelNumber == maxUnlockedLevels)                // Si justo se ha pasado el nivel m�s alto, hay que desbloquear uno nueov. Esta comprobaci�n se hace para evitar que si, por ejemplo, tienes 3 niveles desbloqueados, si te vuelves a pasar
        {                                                   // el primer nivel no hay que desbloquear el cuarto nivel, si no solo cuando acabas el tercero
            PlayerPrefs.SetInt(key, maxUnlockedLevels + 1);
            PlayerPrefs.Save();

            Debug.Log($"Nivel {levelNumber} completado. Nivel {maxUnlockedLevels++} ahora est� desbloqueado.");

            SceneManager.LoadScene("LevelsMenu");
        }
        else
        {
            Debug.Log($"El nivel completado ({levelNumber}) no es el �ltimo ({maxUnlockedLevels++}), por lo que no se desbloquea uno nuevo");
        }
    }

    public bool isLevelUnlocked(int levelNumber)
    {
        return levelNumber <= PlayerPrefs.GetInt(key);
    }
}
