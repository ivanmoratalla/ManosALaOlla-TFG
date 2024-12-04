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


    public KeyCode PickObject
    {
        get => pickObject;
        set => pickObject = value;
    }

    public KeyCode ReleaseObject
    {
        get => releaseObject;
        set => releaseObject = value;
    }

    public KeyCode CutFood
    {
        get => cutFood;
        set => cutFood = value;
    }

    public KeyCode ServeDish
    {
        get => serveDish;
        set => serveDish = value;
    }

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

    public void RebindKey(string action, KeyCode newKey)
    {
        switch (action)
        {
            case nameof(PickObject):
                pickObject = newKey;
                PlayerPrefs.SetInt(PickObjectKey, (int)newKey);
                Debug.Log(pickObject.ToString());
                break;
            case nameof(ReleaseObject):
                releaseObject = newKey;
                PlayerPrefs.SetInt(ReleaseObjectKey, (int)newKey);
                break;
            case nameof(CutFood):
                cutFood = newKey;
                PlayerPrefs.SetInt(CutFoodKey, (int)newKey);
                break;
            case nameof(ServeDish):
                serveDish = newKey;
                PlayerPrefs.SetInt(ServeDishKey, (int)newKey);
                break;
        }
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
