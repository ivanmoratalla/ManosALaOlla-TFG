using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputServiceAsset", menuName = "ServiceAssets/InputServiceAsset")]
public class InputServiceAsset : ScriptableObject
{
    /*[Header("Key Mappings")]
    [Space(10)]

    [SerializeField] private KeyCode forward;
    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode backward;
    [SerializeField] private KeyCode right;*/

    [SerializeField] private string horizontalAxes;
    [SerializeField] private string verticalAxes;

    [SerializeField] private KeyCode pickObject;
    [SerializeField] private KeyCode releaseObject;
    [SerializeField] private KeyCode cutFood;
    [SerializeField] private KeyCode serveDish;

    public Vector3 Poll()
    {
        /*
        var direction = Vector3.zero;

        if (Input.GetKey(forward)) direction -= Vector3.forward;
        if (Input.GetKey(left)) direction -= Vector3.left;
        if (Input.GetKey(backward)) direction -= Vector3.back;
        if (Input.GetKey(right)) direction -= Vector3.right;

        direction.Normalize();

        return direction;*/
        float horizontalInput = Input.GetAxis(horizontalAxes);
        float verticalInput = Input.GetAxis(verticalAxes);

        Vector3 movementDirection = new Vector3(-horizontalInput, 0, -verticalInput);
        movementDirection.Normalize();                                                  // Al normalizarlo consigo que en diagonal sea la misma velocidad que al moverse en un solo eje

        return movementDirection;
    }
}
