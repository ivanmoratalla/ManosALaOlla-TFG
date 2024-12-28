using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    // Servicios que utiliza el personaje
    [SerializeField] private MovementServiceAsset movementService;              // Servicio al que se le llamar� para mover al personaje
    [SerializeField] private InputServiceAsset inputService;                    // Servicio al que se le llamar� para recoger la entrada del usuario y conseguir la direcci�n de movimiento

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementService.Initialize(rb);                                                                         // Se inicializa el movimiento (controlo si hay que congelar la rotaci�n para que el personaje no se caiga por las f�sicas)
    }

    private void FixedUpdate()
    {
        Vector3 movementDirection = inputService.Poll();                                                        // Se consigue la direcci�n de movimiento llamando al servicio encargado de obtenerla

        movementService.Move(rb, this.transform, movementDirection);                                            // Se llama al m�todo del servicio que maneja el movimiento
    }

}
