using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputServiceAsset", menuName = "ServiceAssets/InputServiceAsset")]
public class InputServiceAsset : ScriptableObject
{

    [SerializeField] private string horizontalAxes;
    [SerializeField] private string verticalAxes;

    [SerializeField] private KeyCode pickObject;
    [SerializeField] private KeyCode releaseObject;
    [SerializeField] private KeyCode cutFood;
    [SerializeField] private KeyCode serveDish;

    private const string PickObjectKey = "PickObjectKey";
    private const string ReleaseObjectKey = "ReleaseObjectKey";
    private const string CutFoodKey = "CutFoodKey";
    private const string ServeDishKey = "ServeDishKey";

    private void OnEnable()
    {
        // Cargar teclas personalizadas (si están guardadas)
        pickObject = (KeyCode)PlayerPrefs.GetInt(PickObjectKey, (int)pickObject);
        releaseObject = (KeyCode)PlayerPrefs.GetInt(ReleaseObjectKey, (int)releaseObject);
        cutFood = (KeyCode)PlayerPrefs.GetInt(CutFoodKey, (int)cutFood);
        serveDish = (KeyCode)PlayerPrefs.GetInt(ServeDishKey, (int)serveDish);
    }

    public void SetPickObjectKey(KeyCode newKey)
    {
        pickObject = newKey;
        PlayerPrefs.SetInt(PickObjectKey, (int)newKey);
        PlayerPrefs.Save();
    }

    public void SetReleaseObjectKey(KeyCode newKey)
    {
        releaseObject = newKey;
        PlayerPrefs.SetInt(ReleaseObjectKey, (int)newKey);
        PlayerPrefs.Save();
    }

    public void SetCutFoodKey(KeyCode newKey)
    {
        cutFood = newKey;
        PlayerPrefs.SetInt(CutFoodKey, (int)newKey);
        PlayerPrefs.Save();
    }

    public void SetServeDishKey(KeyCode newKey)
    {
        serveDish = newKey;
        PlayerPrefs.SetInt(ServeDishKey, (int)newKey);
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
}
