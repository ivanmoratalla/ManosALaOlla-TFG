using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;                                   // Variable que indica cómo es el botón (forma, tamaño, color etc)
    public RectTransform container;                                                     // Variable que es el contenedor dónde van a estar los botones que se creen
    private int totalLevels = 0;                                                        // Variable que incluirá el total de niveles que tiene mi juego
    private CloudDataService saveDataService;
    
    // Start is called before the first frame update
    void Start()
    {
        saveDataService = new CloudDataService();                                        // Clase para cargar datos de la nube de Unity

        generateLevelButtons();
    }

    private async void generateLevelButtons()
    {

        foreach (Transform child in container)                                          // Si hubiera algún botón creado, se elimian                               
        {
            Destroy(child.gameObject);
        }

        totalLevels = calculateNumberOfLevels();                                        // Se calcula el número de niveles que tiene mi juego
        int maxUnlockedLevel = await saveDataService.LoadMaxUnlockedLevel();            // Se obtiene el máximo nivel desbloqueado

        Debug.Log("Total de niveles: " + totalLevels);
        Debug.Log("Máximo nivel desbloqueado: " + maxUnlockedLevel);

        for (int i = 1; i <= totalLevels; i++)
        {
            // Crear un nuevo botón a partir del prefab
            GameObject newButton = Instantiate(buttonPrefab, container);

            // Configurar el texto del botón
            Text buttonText = newButton.GetComponentInChildren<Text>();
            buttonText.text = "Nivel " + i;

            // Configurar la interacción del botón
            int levelNumber = i;
            Button button = newButton.GetComponent<Button>();
            button.onClick.AddListener(() => loadLevel(levelNumber));

            // Comprobar si el nivel está desbloqueado
            button.interactable = i <= maxUnlockedLevel;
            Debug.Log("Nivel " + i + " " + button.interactable);
        }
    }

    private void loadLevel(int levelNumber)
    {
        SceneManager.LoadScene("Level_" + levelNumber); 
        Debug.Log($"Cargando nivel: {levelNumber}");
    }

    private int calculateNumberOfLevels()
    {
        int total = 0;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (sceneName.StartsWith("Level_"))                                         // Las escenas correspondientes a niveles comienzan por Level_
            {
                total++;
            }
        }

        return total; 
    }
}
