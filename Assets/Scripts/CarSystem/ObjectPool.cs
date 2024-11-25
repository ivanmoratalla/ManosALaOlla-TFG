using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;                      // Instancia única (singleton)
    private List<GameObject> pooledObjects;                 // Variable que guardará los distintos coches de la pool
    [SerializeField] private GameObject objectToPool;       // Variable para indicar qué objeto es el que se utiliza en la pool
    [SerializeField] private int amountToPool;              // Variable que indica la cantidad de coches que habrá en la pool

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();             // Inicialización de la lista de objetos de la pool
        GameObject tmp;

        // Creación de tantos coches para la pool como diga la variable amountToPool
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    // Método que devuelve un coche de la pool disponible para usarse
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
