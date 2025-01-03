using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : ColorableObject
{
    public GameObject storedObject = null;         // Objeto que hay encima de la encimera

    /* Método para que el jugador interactúe con la encimera. Devuelve true en dos casos:
     * - Si no hay ningún objeto en la mesa y se deja el objeto
     * - Si hay un plato y el ingrediente se puede emplatar
     */
    public bool InteractWithCounter(GameObject obj)
    {
        // DEJAR OBJETO EN LA MESA
        if(storedObject == null)
        {
            PlaceObject(obj);
            return true;
        }
        
        // AÑADIR INGREDIENTE AL PLATO
        Plate plate = storedObject.GetComponent<Plate>();
        Food ingredient = obj.GetComponent<Food>();

        return plate != null && ingredient != null && plate.AddIngredient(ingredient);
    }




    // Método que llamaré desde el jugador para colocar un objeto
    public void PlaceObject(GameObject obj)
    {
        float placePosition = CalculatePosition(obj);
        obj.transform.position = new Vector3(transform.position.x, placePosition, transform.position.z);    // Se coloca el objeto encima de la encimera
        obj.transform.SetParent(this.transform);                                                                        // Se establece la encimera como padre del objeto

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;  // Se desactiva la gravedad y se activa el modo cinemático para evitar que el objeto se mueva por sí solo de la encimera
            rb.isKinematic = true;  
        }

        storedObject = obj;
    }

    // Método para recoger el objeto de la encimera
    public bool PickUpObject(out GameObject objectToPick)
    {
        objectToPick = null;
        if (storedObject != null)
        {
            objectToPick = storedObject;
            storedObject = null;
            return true;
        }
        return false;
    }

    private float CalculatePosition(GameObject obj)
    {
        // Calcula la posición de la encimera
        float counterHeight = transform.localScale.y; // Obtiene la altura de la encimera
        float objectHeight = 0f; // Inicializa la altura del objeto

        // Verifica si el objeto tiene un Collider para calcular su altura
        Collider objCollider = obj.GetComponent<Collider>();
        if (objCollider != null)
        {
            objectHeight = objCollider.bounds.size.y; // Obtiene la altura del objeto basado en su Collider
        }

        // Calcula la posición Y para colocar el objeto
        return  transform.position.y + (counterHeight/* / 2*/) /*+ (objectHeight / 2)*/;
    }
}

