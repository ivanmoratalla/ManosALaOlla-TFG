using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputServiceAsset", menuName = "ServiceAssets/InputServiceAsset")]
public class InputServiceAsset : ScriptableObject
{
    [SerializeField] private int playerId;

    [SerializeField] private string horizontalAxes;
    [SerializeField] private string verticalAxes;


    [Header("Teclas Actuales")]
    [SerializeField] private KeyCode pickObject;
    [SerializeField] private KeyCode releaseObject;
    [SerializeField] private KeyCode cutFood;
    [SerializeField] private KeyCode serveDish;

    [Header("Teclas por Defecto")]
    [SerializeField] private KeyCode defaultPickObject = KeyCode.E;
    [SerializeField] private KeyCode defaultReleaseObject = KeyCode.F;
    [SerializeField] private KeyCode defaultCutFood = KeyCode.C;
    [SerializeField] private KeyCode defaultServeDish = KeyCode.Q;

    private const string PickObjectKey = "PickObjectKey";
    private const string ReleaseObjectKey = "ReleaseObjectKey";
    private const string CutFoodKey = "CutFoodKey";
    private const string ServeDishKey = "ServeDishKey";

    private void OnEnable()
    {
        pickObject = (KeyCode)PlayerPrefs.GetInt(GetPrefixedKey(PickObjectKey), (int)defaultPickObject);
        releaseObject = (KeyCode)PlayerPrefs.GetInt(GetPrefixedKey(ReleaseObjectKey), (int)defaultReleaseObject);
        cutFood = (KeyCode)PlayerPrefs.GetInt(GetPrefixedKey(CutFoodKey), (int)defaultCutFood);
        serveDish = (KeyCode)PlayerPrefs.GetInt(GetPrefixedKey(ServeDishKey), (int)defaultServeDish);
    }

    public void SetPickObjectKey(KeyCode newKey)
    {
        pickObject = newKey;
        PlayerPrefs.SetInt(GetPrefixedKey(PickObjectKey), (int)newKey);
        PlayerPrefs.Save();
    }

    public void SetReleaseObjectKey(KeyCode newKey)
    {
        releaseObject = newKey;
        PlayerPrefs.SetInt(GetPrefixedKey(ReleaseObjectKey), (int)newKey);
        PlayerPrefs.Save();
    }

    public void SetCutFoodKey(KeyCode newKey)
    {
        cutFood = newKey;
        PlayerPrefs.SetInt(GetPrefixedKey(CutFoodKey), (int)newKey);
        PlayerPrefs.Save();
    }

    public void SetServeDishKey(KeyCode newKey)
    {
        serveDish = newKey;
        PlayerPrefs.SetInt(GetPrefixedKey(ServeDishKey), (int)newKey);
        PlayerPrefs.Save();
    }


    public Vector3 Poll()
    {
        float horizontalInput = Input.GetAxis(horizontalAxes);
        float verticalInput = Input.GetAxis(verticalAxes);

        Vector3 movementDirection = new Vector3(-horizontalInput, 0, -verticalInput);
        movementDirection.Normalize();                                                  // Al normalizarlo consigo que en diagonal sea la misma velocidad que al moverse en un solo eje

        return movementDirection;
    }

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

    public KeyCode getPickObjectKey()
    {
        return pickObject;
    }

    public KeyCode getReleaseObjectKey()
    {
        return releaseObject;
    }

    public KeyCode getCutFoodKey()
    {
        return cutFood;
    }

    public KeyCode getServeDishKey()
    {
        return serveDish;
    }

    public int getPlayerId()
    {
        return playerId;
    }

    private string GetPrefixedKey(string key)
    {
        return $"Player{playerId}_{key}";
    }
}
