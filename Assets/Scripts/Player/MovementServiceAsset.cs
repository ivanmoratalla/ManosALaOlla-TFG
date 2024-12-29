using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "MovementServiceAsset", menuName = "ServiceAssets/MovementServiceAsset")]
public class MovementServiceAsset : ScriptableObject
{
    [Header("Movement Settings")]
    [Space(10)]

    [SerializeField] private float movementSpeed;

    [Header("Rotation Settings")]
    [Space(10)]

    [SerializeField] private float rotationSpeed;

    public void Initialize(Rigidbody rb)
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Move(Rigidbody rb, UnityEngine.Transform transform, Vector3 movementDirection)
    {
        rb.MovePosition(transform.position + movementDirection * movementSpeed * Time.fixedDeltaTime);

        if (movementDirection.magnitude >= 0.1f)                                                                // Se rota al personaje solo si hay movimiento. Si no lo hay no se rotará nada.
        {
            Rotate(rb, movementDirection);
        }
    }

    public void Rotate(Rigidbody rb, Vector3 movementDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
    }
}
