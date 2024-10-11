using System.Collections;
using UnityEngine;

public abstract class KitchenAppliance : MonoBehaviour
{
    public GameObject storedFood = null;         // Comida que se va a cocinar / se est� cocinando
    protected bool isProcessing = false;            // Variable para indicar si se est� cocinando algo
    public abstract FoodAction action { get; }



    // Este m�todo se utilizar� desde el jugador cuando quiera interactuar con un electrodom�stico, ya sea simplemente para colocar el objeto a cocinar como para comenzar a cocinar
    public abstract bool interactWithAppliance(GameObject food);


    // M�todo para colocar la comida en el electrodom�stico
    public void placeFood(GameObject food)
    {
        storedFood = food;
        storedFood.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        storedFood.transform.SetParent(this.transform);

        Rigidbody rb = storedFood.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    // M�todo para recoger el objeto del electrodom�stico
    public GameObject pickUpFood()
    {
        GameObject foodAux = null;
        if(canPickUpFood())
        {
            foodAux = storedFood;
            storedFood = null;

        }
        return foodAux;
    }

    /* M�todo para verificar si el electrodom�stico est� listo para procesar la comida. Para poder interactuar con el electrodom�tico:
     * - No tiene que haber un objeto en �l (para poder dejar el ingrediente)
     * - No tiene que haber ning�n elemento proces�ndose
     * - Se tiene que poder hacer la transici�n de estado del ingrediente en el electrodom�stico
     */
    protected bool CanProcess(GameObject food)
    {
        Debug.Log("Devuelvo: " + storedFood != null && !isProcessing && food.GetComponent<Food>().canTransition(action));
        return !isProcessing && food.GetComponent<Food>().canTransition(action);
    }

    protected bool canPickUpFood()
    {
        return storedFood != null && !isProcessing;
    }

    // M�todo para verificar si ya tiene comida
    public bool HasFood()
    {
        return storedFood != null;
    }
}