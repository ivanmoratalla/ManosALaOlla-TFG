using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;                                                             // Velocidad de movimiento coche
    private Vector3 destination;                                                                            // Punto al que tiene que llegar el coche

    public void Initialize(Vector3 target)
    {
        destination = target;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);  // Se mueve el coche hacia el destino

        if (Vector3.Distance(transform.position, destination) < 0.1f)                                       // Si el coche alcanza el destino, se devuelve a la pool
        {
            if (ObjectPool.Instance != null)
            {
                ObjectPool.Instance.ReturnObject(gameObject);
            }
            else
            {
                Destroy(gameObject);                                                                        // Backup por si no hay pool
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador atropellado!");
        }
    }
}
