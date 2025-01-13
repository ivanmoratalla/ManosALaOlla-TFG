using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInteraction : MonoBehaviour
{
    [SerializeField] private GameObject hand;                                   // Punto donde el objeto que el jugador tiene en la mano va a estar (posici�n)

    [SerializeField] private GameObject respawnPoint = null;                    // Punto donde reaparece el jugador (solo si hay coches en los niveles)
    [SerializeField] private int playerNumber;

    public static event Action<int, float> OnPlayerDisappear;                   // Evento para notificar que el jugador ha colisionado con un coche

    private void OnTriggerEnter(Collider other)
    {
        // Se comprueba si se ha colisionado con un coche
        if (other.GetComponent<CarMovement>() != null)
        {
            Debug.Log("Jugador atropellado");
            HandleCarInteraction();
        }
    }

    private void HandleCarInteraction()
    {
        OnPlayerDisappear?.Invoke(playerNumber, 3f);                            // Se notifica a StatsUI que debe poner el tiempo para la reaparici�n

        // Se destruye el objeto que ten�a en la mano el jugador (si ten�a uno)
        if (hand.transform.childCount > 0)
        {
            GameObject childObject = hand.transform.GetChild(0).gameObject;
            Destroy(childObject);
        }

        // Se desactiva el jugador
        this.gameObject.SetActive(false);

        // Se programa la reaparici�n
        Invoke(nameof(RespawnPlayer), 3f);
    }

    private void RespawnPlayer()
    {
        this.gameObject.SetActive(true);
        this.transform.position = respawnPoint.transform.position;
    }
}