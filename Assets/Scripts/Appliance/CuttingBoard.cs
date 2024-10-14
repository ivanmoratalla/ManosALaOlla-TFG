using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : KitchenAppliance
{
    //private bool isCutting = false;                 // Variable que indica si el jugador está cortando el objeto
    private bool isPlayerNearby = false;            // Variable que indica si el jugador está colisionando con la tabla (al actualizarse el timer en el Update necesito esta variable)
    [SerializeField] private float timeToCut = 3f;  // Variable que indica el tiempo que se tiene que mantener pulsado para cortar con éxito
    private float holdTime = 0f;                    // VAriable que indica el tiempo que se lleva cortando

    public override FoodAction action
    {
        get { return FoodAction.Cut; }              // Variable que indica que la acción que se hace en la tabla es "cortar"
    }


    // En el caso de la tabla de cortar, no se quiere cortar al poner el objeto en la tabla, si no solo colocarlo
    public override bool interactWithAppliance(GameObject food)
    {
        if(CanProcess(food))
        {
            placeFood(food);
            Debug.Log("Se ha colocado la comida en la tabla de cortar");
            return true;
        }
        return false;
    }

    private void Update()
    {
        if (storedFood != null && isPlayerNearby)      // Solo se puede cortar si hay un objeto en la tabla y si el jugador está colisionando con la tabla
        {
            if (Input.GetKey(KeyCode.C))
            {
                if (!isProcessing)
                {
                    // El jugador ha comenzado a mantener la tecla presionada
                    isProcessing = true;
                    holdTime = 0f;
                    //storedFood.GetComponent<Collider>().enabled = false;    // Para evitar que se pueda coger mientras se cocina
                }

                // Aumentar el contador del tiempo que se mantiene la tecla presionada
                holdTime += Time.deltaTime;

                if (holdTime >= timeToCut)
                {
                    // Si se ha mantenido presionada la tecla durante el tiempo requerido, empezar a cortar
                    cookFood();
                }
            }
            else if (isProcessing)
            {
                // Si el jugador suelta la tecla antes de llegar al tiempo requerido, cancelar el proceso
                isProcessing = false;
                holdTime = 0f;

                Debug.Log("Has soltado la tecla antes de tiempo o no estás cerca. Intenta de nuevo.");
            }
        }
    }


    private void cookFood()
    {
        storedFood.GetComponent<Food>().changeFoodState(action, out storedFood);
        Debug.Log("Comida cocinada");
        
        isProcessing = false;
        holdTime = 0f;

        storedFood.GetComponent<Collider>().enabled = false;        // Se desactivan las colisiones y la gravedad del nuevo ingrediente instanciado para evitar que se caiga del electrodoméstico
        storedFood.GetComponent<Rigidbody>().useGravity = false;

    }



    private void OnTriggerEnter(Collider other)
    {
        PlayerInteraction player = other.GetComponent<PlayerInteraction>();
        if (player != null)
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInteraction player = other.GetComponent<PlayerInteraction>();
        if (player != null)
        {
            isPlayerNearby = false;
            isProcessing = false;
            //storedFood.GetComponent<Collider>().enabled = true;
        }
    }
}
