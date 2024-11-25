using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;                      // Instancia �nica (singleton)
    private List<GameObject> pooledObjects;                 // Variable que guardar� los distintos coches de la pool
    [SerializeField] private GameObject objectToPool;       // Variable para indicar qu� objeto es el que se utiliza en la pool
    [SerializeField] private int amountToPool;              // Variable que indica la cantidad de coches que habr� en la pool

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();             // Inicializaci�n de la lista de objetos de la pool
        GameObject tmp;

        // Creaci�n de tantos coches para la pool como diga la variable amountToPool
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    // M�todo que devuelve un coche de la pool disponible para usarse
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                pooledObjects[i].SetActive(true);
                return pooledObjects[i];
            }
        }
        return null;
    }

    // Metodo que devuelve un objeto a la pool
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
