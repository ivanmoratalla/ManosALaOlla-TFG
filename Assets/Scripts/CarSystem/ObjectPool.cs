using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;                                          // Instancia �nica (singleton)
    private List<GameObject> pooledObjects;                                     // Variable que guardar� los distintos coches de la pool
    [SerializeField] private List<GameObject> objectsToPool;                    // Variable para indicar qu� objeto es el que se utiliza en la pool
    [SerializeField] private int amountToPool;                                  // Variable que indica la cantidad de coches que habr� en la pool
    private int currentObjectIndex = 0;                                         // �ndice c�clico para alternar entre los objetos que se pueden a�adir a la pool
    private int lastReturnedIndex = -1;                                         // �ndice del �ltimo objeto devuelto por GetPooledObject


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();                                     // Inicializaci�n de la lista de objetos de la pool
        GameObject tmp;

        // Creaci�n de tantos coches para la pool como diga la variable amountToPool
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectsToPool[currentObjectIndex]); 
            tmp.SetActive(false);
            pooledObjects.Add(tmp);

            currentObjectIndex = (currentObjectIndex + 1) % objectsToPool.Count;    // Se incrementa el �ndice de manera c�clica para que instancie otro coche distinto
        }
    }

    // M�todo que devuelve un objeto disponible de la pool
    public GameObject GetPooledObject()
    {
        int startIndex = (lastReturnedIndex + 1) % pooledObjects.Count;             // Comienza en el siguiente al �ltimo devuelto
        int index = startIndex;

        do
        {
            if (!pooledObjects[index].activeInHierarchy)                            // Si el objeto no est� activo, se devuelve
            {
                pooledObjects[index].SetActive(true);
                lastReturnedIndex = index;                                          // Actualiza el �ndice del �ltimo devuelto
                return pooledObjects[index];
            }

            index = (index + 1) % pooledObjects.Count;                              // Avanzar al siguiente �ndice, volviendo al principio si se llega al final

        } while (index != startIndex);                                              // Bucle hasta recorrer toda la lista

        return null;                                                                // No hay objetos disponibles
    }

    // Metodo que devuelve un objeto a la pool
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
