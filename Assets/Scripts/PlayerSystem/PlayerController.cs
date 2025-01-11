using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    // Servicios que utiliza el personaje
    [SerializeField] private MovementServiceAsset movementService;              // Servicio al que se le llamará para mover al personaje
    [SerializeField] private InputServiceAsset inputService;                    // Servicio al que se le llamará para recoger la entrada del usuario y conseguir la dirección de movimiento
    
    private Animator animator;
    private bool isWalking = false;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        movementService.Initialize(rb);                                                                         // Se inicializa el movimiento (controlo si hay que congelar la rotación para que el personaje no se caiga por las físicas)
    }

    private void FixedUpdate()
    {
        Vector3 movementDirection = inputService.Poll();                                                        // Se consigue la dirección de movimiento llamando al servicio encargado de obtenerla

        movementService.Move(rb, this.transform, movementDirection);                                            // Se llama al método del servicio que maneja el movimiento

        bool newState = movementDirection.magnitude > 0.1f;

        if(isWalking != newState)                                                                               // Esta comprobación se hace para evitar que se ejecute todo el rato SetBool
        {
            isWalking = newState;
            animator.SetBool("isWalking", isWalking);
        }
    }

}
