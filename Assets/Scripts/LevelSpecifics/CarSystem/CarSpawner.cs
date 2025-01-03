using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;                     // Punto donde aparecen los coches
    [SerializeField] private GameObject destinationPoint;               // Punto donde llegan los coches
    [SerializeField] private float spawninterval = 10f;                 // Intervarlo entre que aparece un coche      

    private void Start()
    {
        InvokeRepeating(nameof(SpawnCar), 0f, spawninterval);
    }

    private void SpawnCar()
    {
        GameObject car = ObjectPool.Instance.GetPooledObject();

        if(car != null)
        {
            car.transform.position = spawnPoint.transform.position;
            

            car.GetComponent<CarMovement>().Initialize(destinationPoint.transform.position);
        }
    }
}
