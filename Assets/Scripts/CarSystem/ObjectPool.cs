using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;                                          // Instancia única (singleton)
    private List<GameObject> pooledObjects;                                     // Variable que guardará los distintos coches de la pool
    [SerializeField] private List<GameObject> objectsToPool;                    // Variable para indicar qué objeto es el que se utiliza en la pool
    [SerializeField] private int amountToPool;                                  // Variable que indica la cantidad de coches que habrá en la pool
    private int currentObjectIndex = 0;                                         // Índice cíclico para alternar entre los objetos que se pueden añadir a la pool
    private int lastReturnedIndex = -1;                                         // Índice del último objeto devuelto por GetPooledObject


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();                                     // Inicialización de la lista de objetos de la pool
        GameObject tmp;

        // Creación de tantos coches para la pool como diga la variable amountToPool
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectsToPool[currentObjectIndex]); 
            tmp.SetActive(false);
            pooledObjects.Add(tmp);

            currentObjectIndex = (currentObjectIndex + 1) % objectsToPool.Count;    // Se incrementa el índice de manera cíclica para que instancie otro coche distinto
        }
    }

    // Método que devuelve un objeto disponible de la pool
    public GameObject GetPooledObject()
    {
        int startIndex = (lastReturnedIndex + 1) % pooledObjects.Count;             // Comienza en el siguiente al último devuelto
        int index = startIndex;

        do
        {
            if (!pooledObjects[index].activeInHierarchy)                            // Si el objeto no está activo, se devuelve
            {
                pooledObjects[index].SetActive(true);
                lastReturnedIndex = index;                                          // Actualiza el índice del último devuelto
                return pooledObjects[index];
            }

            index = (index + 1) % pooledObjects.Count;                              // Avanzar al siguiente índice, volviendo al principio si se llega al final

        } while (index != startIndex);                                              // Bucle hasta recorrer toda la lista

        return null;                                                                // No hay objetos disponibles
    }

    // Metodo que devuelve un objeto a la pool
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
