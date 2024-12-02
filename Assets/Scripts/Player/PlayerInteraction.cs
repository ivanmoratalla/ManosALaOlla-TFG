using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    public GameObject hand;                                                         // Punto donde el objeto que el jugador tiene en la mano va a estar (posici�n)
    private GameObject pickedObject = null;                                         // Objeto que el jugador tiene en la mano
    private Counter collidingCounter = null;                                        // Esta variable �ndica si tengo una encimera con la que el personaje est� colisionando, para poder coger/soltar objetos en ella
    private KitchenAppliance collidingAppliance = null;                             // Esta variable �ndica si tengo un electrodom�stico con el que el personaje est� colisionando, para poder interactuar o no con �l

    [SerializeField] private GameObject respawnPoint = null;                        // Punto donde reaparece el jugador (solo si hay coches en los niveles)
    private bool isPlayerInteractingWithCar = false;                                // Para evitar interaccion repetida con el coche mientras est� desactivado

    private OrderManager orderManager;                                              // Manejador de pedidos con el que el jugador debe interactuar al completar uno

    [SerializeField] private InputServiceAsset inputService;                        // Servicio al que se le llamar� para saber qu� tecla corresponde a cada acci�n

    public static event Action<int, string, Action<bool>> OnTryToServeDish;         // Evento para notificar cuando se quiere servir un pedido
    public static event Action<float, Vector3> OnPlayerDisappear;                            // Evento para notificar que el jugador ha colisionado con un coche

    void Update()
    {
        if (pickedObject != null && Input.GetKey(inputService.getReleaseObjectKey()))                        // Si el usuario tiene un objeto en la mano y pulsa esta tecla quiere soltar el objeto
        {
            handleReleaseObject();
        }
    }

    // Este metodo es llamado cuando el jugador entra en contacto con otro objeto
    private void OnTriggerEnter(Collider other)
    {
        // Se comprueba si se ha entrado en contacto con una encimera
        Counter counter = other.GetComponent<Counter>();
        if (counter != null)                                
        {
            collidingCounter = counter;                                             // Si se ha entrado en contacto con una encimera, la indico en la variable
        }

        // Se comprueba si se ha entrado en contacto con un electrodom�stico
        KitchenAppliance appliance = other.GetComponent<KitchenAppliance>();
        if (appliance != null)
        {
            collidingAppliance = appliance;
        }

        if(other.GetComponent<CarMovement>() != null && !isPlayerInteractingWithCar)
        {
            Debug.Log("Jugador atropellado");
            Vector3 collisionPoint = other.ClosestPoint(transform.position);
            HandleCarInteraction(collisionPoint);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject objectToPick;
        if(pickedObject == null && Input.GetKey(inputService.getPickObjectKey()))                       // Se comprueba si se ha pulsado la tecla y si se puede coger un objeto
        {
            GameObject counterObject;
            if (collidingCounter != null && collidingCounter.pickUpObject(out counterObject))   // Se comprueba si hay cerca una estanter�a y si tiene un objeto para coger 
            {

                handlePickObject(counterObject);
            }
            else if (collidingAppliance != null && (objectToPick = collidingAppliance.pickUpFood()) != null)
            {
                handlePickObject(objectToPick);
            }
            else if (other.GetComponent<Crate>() != null && (objectToPick = other.GetComponent<Crate>().pickUpFood()))
            {
                handlePickObject(objectToPick);
            }
            else if (other.gameObject.CompareTag("Objeto"))                     // Se comprueba si se tiene un objeto en el suelo
            {
                handlePickObject(other.gameObject);
            }
        }
        else
        {
            Table table = other.GetComponent<Table>();                          // Se comprueba si la colisi�n es con una mesa
            if (table != null)
            {
                handleTableInteraction(table);
            }
        }
    }

    // Este metodo es llamado cuando el jugador deja de estar en contacto con otro objeto
    private void OnTriggerExit(Collider other)
    {
        Counter counter = other.GetComponent<Counter>();
        if (counter != null)                                
        {
            collidingCounter = null;                                        // Si se ha dejado de estar en contacto con una encimera, se indica en la variable correspondiente que ya no se tiene una encimera cerca d�nde dejar el objeto
        }

        KitchenAppliance appliance = other.GetComponent<KitchenAppliance>();
        if (appliance != null)
        {
            collidingAppliance = null;
        }
    }


    // Este m�todo es el encargado de coger el objeto que se pasa como par�metro
    private void handlePickObject(GameObject other)
    {
        other.GetComponent<Rigidbody>().useGravity = false;                 
        other.GetComponent<Rigidbody>().isKinematic = true;                

        other.transform.position = hand.transform.position;
        other.gameObject.transform.SetParent(hand.gameObject.transform);

        pickedObject = other.gameObject;
    }

    /* Este m�todo es el encargado de manejar cuando el usuario quiere soltar un objeto. Lo puede hacer por cuatro motivos:
     * - Dejar un objeto en una encimera
     * - Dejar un ingrediente en un plato
     * - Dejar un objeto en el suelo
     * - Dejar un objeto, tipo comida, en un electrodom�stico para cocinarlo (el electrodom�stico se encargar� de mirar si se puede)
     */
    private void handleReleaseObject()
    {
        // INTERACTUAR CON UN ELECTRODOM�STICO
        if(pickedObject.GetComponent<Food>() != null && collidingAppliance != null && collidingAppliance.interactWithAppliance(pickedObject))
        {
            Debug.Log("Se ha interactuado con el electrodom�stico");
            pickedObject = null;                                               

        }
        // INTERACTUAR CON UNA ENCIMERA, TANTO PARA DEJAR OBJETO COMO PARA EMPLATAR
        else if (collidingCounter != null && collidingCounter.interactWithCounter(pickedObject))                        // Si hay una encimera cerca, se deja en ella el objeto
        {
            pickedObject = null;                                                
        }
        // DEJAR EN EL SUELO
        else if(collidingAppliance == null && collidingCounter == null && pickedObject.GetComponent<Plate>() == null)   // Se deja en el suelo si no se est� cerca de un electrodom�stico o encimera, para evitar colisiones err�neas con lo que hay en ellos.                                                                                            
        {                                                                                                               // Adem�s, se impide que los platos se puedan dejar en el suelo (no se podr�an coger luego porque no se detectar�a colisi�n al ser tan bajos)
            pickedObject.GetComponent<Rigidbody>().useGravity = true;
            pickedObject.GetComponent<Rigidbody>().isKinematic = false;

            pickedObject.gameObject.transform.SetParent(null);

            pickedObject = null;                                               
        }
    }

    private void handleTableInteraction(Table table)
    {
        Plate plate;
        if (pickedObject != null && (plate = pickedObject.GetComponent<Plate>()) != null && Input.GetKey(inputService.getServeDishKey()))          // Se comprueba si se tiene en la mano un plato y se pulsa el bot�n de entregar
        {
            deliverOrder(table.getTableNumber(), plate.getCompletedRecipeName());
        }
    }

    private void HandleCarInteraction(Vector3 collisionPoint)
    {
        isPlayerInteractingWithCar = true;

        OnPlayerDisappear?.Invoke(3f, collisionPoint);

        // Desactivar el objeto padre
        this.transform.parent.gameObject.SetActive(false);

        // Programar la reaparici�n
        Invoke(nameof(ReappearPlayer), 3f);
    }

    private void ReappearPlayer()
    {
        this.transform.parent.gameObject.SetActive(true);
        this.transform.parent.transform.position = respawnPoint.transform.position;
        isPlayerInteractingWithCar = false;
    }


    private void deliverOrder(int tableNumber, string dish)
    {
        OnTryToServeDish?.Invoke(tableNumber, dish, res =>
        {
            if (res)
            {
                Destroy(pickedObject);
                pickedObject = null;
            }
            else
            {
                // Aqu� incluir� las penalizaciones que sean 
            }
        });
    }
}