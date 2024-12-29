using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInteraction : MonoBehaviour
{

    public GameObject hand;                                                         // Punto donde el objeto que el jugador tiene en la mano va a estar (posición)

    [SerializeField] private GameObject respawnPoint = null;                        // Punto donde reaparece el jugador (solo si hay coches en los niveles)
    [SerializeField] private int playerNumber;
    private bool isPlayerInteractingWithCar = false;                                // Para evitar interaccion repetida con el coche mientras esté desactivado

    public static event Action<int, float, Vector3> OnPlayerDisappear;              // Evento para notificar que el jugador ha colisionado con un coche

    private void OnTriggerEnter(Collider other)
    {
        // Se comprueba si se ha colisionado con un coche
        if (other.GetComponent<CarMovement>() != null && !isPlayerInteractingWithCar)
        {
            Debug.Log("Jugador atropellado");
            Vector3 collisionPoint = other.ClosestPoint(transform.position);
            HandleCarInteraction(collisionPoint);
        }
    }

    private void HandleCarInteraction(Vector3 collisionPoint)
    {
        isPlayerInteractingWithCar = true;

        OnPlayerDisappear?.Invoke(playerNumber, 3f, collisionPoint);

        // Se destruye el objeto que tenía en la mano el jugador (si tenía uno)
        if (hand.transform.childCount > 0)
        {
            GameObject childObject = hand.transform.GetChild(0).gameObject;
            Destroy(childObject);
        }

        // Se desactiva el jugador
        this.gameObject.SetActive(false);

        // Se programa la reaparición
        Invoke(nameof(RespawnPlayer), 3f);
    }

    private void RespawnPlayer()
    {
        this.gameObject.SetActive(true);
        this.transform.position = respawnPoint.transform.position;
        isPlayerInteractingWithCar = false;
    }
}