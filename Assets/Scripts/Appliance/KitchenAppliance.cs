using System.Collections;
using UnityEngine;

public abstract class KitchenAppliance : MonoBehaviour
{
    public GameObject storedFood = null;         // Comida que se va a cocinar / se está cocinando
    protected bool isProcessing = false;            // Variable para indicar si se está cocinando algo
    public abstract FoodAction action { get; }



    // Este método se utilizará desde el jugador cuando quiera interactuar con un electrodoméstico, ya sea simplemente para colocar el objeto a cocinar como para comenzar a cocinar
    public abstract bool interactWithAppliance(GameObject food);


    // Método para colocar la comida en el electrodoméstico
    public void placeFood(GameObject food)
    {
        float newYPosition = calculatePosition(food);

        storedFood = food;
        storedFood.transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
        storedFood.transform.SetParent(this.transform);

        Rigidbody rb = storedFood.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    // Método para recoger el objeto del electrodoméstico
    public GameObject pickUpFood()
    {
        GameObject foodAux = null;
        if(canPickUpFood())
        {
            foodAux = storedFood;
            storedFood.GetComponent<Collider>().enabled = true;     // Al coger el objeto se le activa el collider para que se pueda coger del suelo. No se activa la gravedad porque si no se caería
            storedFood = null;

        }
        return foodAux;
    }

    /* Método para verificar si el electrodoméstico está listo para procesar la comida. Para poder interactuar con el electrodomético:
     * - No tiene que haber un objeto en él (para poder dejar el ingrediente)
     * - No tiene que haber ningún elemento procesándose
     * - Se tiene que poder hacer la transición de estado del ingrediente en el electrodoméstico
     */
    protected bool CanProcess(GameObject food)
    {
        return !isProcessing && food.GetComponent<Food>().canTransition(action);
    }

    protected bool canPickUpFood()
    {
        return storedFood != null && !isProcessing;
    }

    private float calculatePosition(GameObject obj)
    {
        // Calcula la altura del electrodoméstico
        float counterHeight = this.GetComponent<Collider>().bounds.size.y; // Obtiene la altura de la encimera
        float objectHeight = 0f; // Inicializa la altura del objeto

        // Verifica si el objeto tiene un Collider para calcular su altura
        Collider objCollider = obj.GetComponent<Collider>();
        if (objCollider != null)
        {
            objectHeight = objCollider.bounds.size.y; // Obtiene la altura del objeto basado en su Collider
        }

        // Calcula la posición Y para colocar el objeto
        return transform.position.y + (counterHeight) /*/ 2) + (objectHeight / 2)*/;
    }
}