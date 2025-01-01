using System;
using System.Collections;
using UnityEngine;

public abstract class KitchenAppliance : ColorableObject
{
    protected GameObject storedFood = null;         // Comida que se va a cocinar / se est� cocinando
    protected bool isProcessing = false;            // Variable para indicar si se est� cocinando algo
    public abstract FoodAction action { get; }

    [SerializeField] private ProgressUI progressUIPrefab;
    public EventHandler<float> OnProgressChange;
    public EventHandler OnProgressCanceled;

    // M�todo para interactuar con el electrodom�stico. Debe ser implementado por el electrodom�stico concreto ya que hacen cosas distintas.
    // Se llamar� desde el jugador cuando quiera interactuar con un electrodom�stico
    public abstract bool InteractWithAppliance(GameObject food);

    public void StartProgressUI()
    {
        ProgressUI progressUI = Instantiate(progressUIPrefab);
        progressUI.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        progressUI.Set(this);
    }


    // M�todo para colocar la comida en el electrodom�stico
    protected void PlaceFood(GameObject food)
    {
        float newYPosition = CalculatePosition(food);

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

    // M�todo para recoger el objeto del electrodom�stico
    public GameObject PickUpFood()
    {
        GameObject foodAux = null;
        if(CanPickUpFood())
        {
            foodAux = storedFood;
            storedFood.GetComponent<Collider>().enabled = true;     // Al coger el objeto se le activa el collider para que se pueda coger del suelo. No se activa la gravedad porque si no se caer�a
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
        return !isProcessing && food.GetComponent<Food>().CanTransition(action);
    }

    protected bool CanPickUpFood()
    {
        return storedFood != null && !isProcessing;
    }

    private float CalculatePosition(GameObject obj)
    {
        // Calcula la altura del electrodom�stico
        float counterHeight = this.GetComponent<Collider>().bounds.size.y; // Obtiene la altura de la encimera
        float objectHeight = 0f; // Inicializa la altura del objeto

        // Verifica si el objeto tiene un Collider para calcular su altura
        Collider objCollider = obj.GetComponent<Collider>();
        if (objCollider != null)
        {
            objectHeight = objCollider.bounds.size.y; // Obtiene la altura del objeto basado en su Collider
        }

        // Calcula la posici�n Y para colocar el objeto
        return transform.position.y + (counterHeight) /*/ 2) + (objectHeight / 2)*/;
    }
}