using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 50f;
    
    private Rigidbody rb;

    [SerializeField] private string horizontalAxes;
    [SerializeField] private string verticalAxes; 

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;                                                   // Congelo la rotación en todos los ejes para evitar que el personaje se caiga hacia los lados y rote solo. La controlaré manualmente después
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis(horizontalAxes);
        float verticalInput = Input.GetAxis(verticalAxes);

        Vector3 movementDirection = new Vector3(-horizontalInput, 0, -verticalInput);
        movementDirection.Normalize();                                                                          // Con esto consigo que en diagonal sea la misma velocidad

        rb.MovePosition(transform.position + movementDirection * speed * Time.fixedDeltaTime);

        if (movementDirection.magnitude >= 0.1f)                                                                // Se rota al personaje solo si hay movimiento. Si no lo hay no se rotará nada.
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }

    }

}
