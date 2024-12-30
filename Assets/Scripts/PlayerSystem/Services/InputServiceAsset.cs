using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputServiceAsset", menuName = "ServiceAssets/InputServiceAsset")]
public class InputServiceAsset : ScriptableObject
{
    [SerializeField] private int playerId;                              // ID del jugador que usa el servicio

    [SerializeField] private string horizontalAxes;                     // Eje horizontal para el movimiento
    [SerializeField] private string verticalAxes;                       // Eje vertical para el movimiento

    // Teclas actuales para las acciones del jugador
    [Header("Teclas Actuales")]
    private KeyCode pickObject;
    private KeyCode releaseObject;
    private KeyCode cutFood;
    private KeyCode serveDish;

    // Teclas por defecto para las acciones del jugador
    [Header("Teclas por Defecto")]
    [SerializeField] private KeyCode defaultPickObject = KeyCode.E;
    [SerializeField] private KeyCode defaultReleaseObject = KeyCode.F;
    [SerializeField] private KeyCode defaultCutFood = KeyCode.C;
    [SerializeField] private KeyCode defaultServeDish = KeyCode.Q;

    // Strings a usar como clave para almacenar la info en PlayerPrefs
    private const string PickObjectKey = "PickObjectKey";
    private const string ReleaseObjectKey = "ReleaseObjectKey";
    private const string CutFoodKey = "CutFoodKey";
    private const string ServeDishKey = "ServeDishKey";

    private void OnEnable()
    {
        // Se cogen los valores guardados para cada acción, y si no se ponen las por defecto
        pickObject = (KeyCode)PlayerPrefs.GetInt(GetPrefixedKey(PickObjectKey), (int)defaultPickObject);
        releaseObject = (KeyCode)PlayerPrefs.GetInt(GetPrefixedKey(ReleaseObjectKey), (int)defaultReleaseObject);
        cutFood = (KeyCode)PlayerPrefs.GetInt(GetPrefixedKey(CutFoodKey), (int)defaultCutFood);
        serveDish = (KeyCode)PlayerPrefs.GetInt(GetPrefixedKey(ServeDishKey), (int)defaultServeDish);
    }

    // Método que devuelve el vector de movimiento en función de los valores de los ejes
    public Vector3 Poll()
    {
        float horizontalInput = Input.GetAxis(horizontalAxes);
        float verticalInput = Input.GetAxis(verticalAxes);

        Vector3 movementDirection = new Vector3(-horizontalInput, 0, -verticalInput);
        movementDirection.Normalize();                                                  // Al normalizarlo consigo que en diagonal sea la misma velocidad que al moverse en un solo eje

        return movementDirection;
    }

    public void SetKey(string action, KeyCode newKey)
    {
        bool actionExists = true;

        switch (action)
        {
            case "PickObject":
                pickObject = newKey;
                break;
            case "ReleaseObject":
                releaseObject = newKey; 
                break;
            case "CutFood":
                cutFood = newKey;
                break;
            case "ServeDish":
                serveDish = newKey;
                break;
            default:
                actionExists = false;
                break;
        }

        if(actionExists)
        {
            PlayerPrefs.SetInt(GetPrefixedKey(action + "Key"), (int)newKey);
            PlayerPrefs.Save();
        }
    }

    // Método para resetear las teclas de las acciones a los valores por defecto
    public void ResetToDefaults()
    {
        // Restablecer valores a los predeterminados
        pickObject = defaultPickObject;
        releaseObject = defaultReleaseObject;
        cutFood = defaultCutFood;
        serveDish = defaultServeDish;

        // Guardar los valores predeterminados en PlayerPrefs
        PlayerPrefs.SetInt(GetPrefixedKey(PickObjectKey), (int)defaultPickObject);
        PlayerPrefs.SetInt(GetPrefixedKey(ReleaseObjectKey), (int)defaultReleaseObject);
        PlayerPrefs.SetInt(GetPrefixedKey(CutFoodKey), (int)defaultCutFood);
        PlayerPrefs.SetInt(GetPrefixedKey(ServeDishKey), (int)defaultServeDish);

        PlayerPrefs.Save();
    }

    // Método que devuelve la tecla de coger objeto
    public KeyCode GetPickObjectKey()
    {
        return pickObject;
    }

    // Método que devuelve la tecla de soltar objeto
    public KeyCode GetReleaseObjectKey()
    {
        return releaseObject;
    }

    // Método que devuelve la tecla de cortar
    public KeyCode GetCutFoodKey()
    {
        return cutFood;
    }

    // Método que devuelve la tecla de servir plato
    public KeyCode GetServeDishKey()
    {
        return serveDish;
    }

    // Método que devuelve el id del jugador
    public int GetPlayerId()
    {
        return playerId;
    }

    // Método que devuelve la clave a usar para guardar o cargar la tecla de una acción jugador
    private string GetPrefixedKey(string key)
    {
        return $"Player{playerId}_{key}";
    }
}
