using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public GameObject storedObject = null;         // Objeto que hay encima de la encimera

    // M�todo que llamar� desde el jugador para colocar un objeto
    public void placeObject(GameObject obj)
    {
        obj.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);    // Se coloca el objeto encima de la encimera
        obj.transform.SetParent(this.transform);                                                                        // Se establece la encimera como padre del objeto

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;  // Se desactiva la gravedad y se activa el modo cinem�tico para evitar que el objeto se mueva por s� solo de la encimera
            rb.isKinematic = true;  
        }

        storedObject = obj;
    }

    // M�todo para recoger el objeto de la encimera
    public GameObject pickUpObject()
    {
        GameObject objectToPick = storedObject;
        if (objectToPick != null)
        {
            storedObject = null;
        }
        return objectToPick;
    }

    // M�todo que devuelve si hay alg�n objeto en la encimera
    public bool hasObject()
    {
        return storedObject != null;
    }
}

