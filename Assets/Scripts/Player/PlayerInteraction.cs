using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    public GameObject hand;                                                 // Punto donde el objeto que el jugador tiene en la mano va a estar (posici�n)
    private GameObject pickedObject = null;                                 // Objeto que el jugador tiene en la mano
    private Counter collidingCounter = null;                                // Esta variable �ndica si tengo una encimera con la que el personaje est� colisionando, para poder coger/soltar objetos en ella
    private KitchenAppliance collidingAppliance = null;                     // Esta variable �ndica si tengo un electrodom�stico con el que el personaje est� colisionando, para poder interactuar o no con �l


    void Update()
    {
        if (pickedObject != null && Input.GetKey("f"))                      // Si el usuario tiene un objeto en la mano y pulsa esta tecla quiere soltar el objeto
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
            collidingCounter = counter;                                     // Si se ha entrado en contacto con una encimera, la indico en la variable
        }

        // Se comprueba si se ha entrado en contacto con un electrodom�stico
        KitchenAppliance appliance = other.GetComponent<KitchenAppliance>();
        if (appliance != null)
        {
            collidingAppliance = appliance;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject applianceObject;
        if(pickedObject == null && Input.GetKey("e"))                       // Se comprueba si se ha pulsado la tecla y si se puede coger un objeto
        {
            GameObject counterObject;
            if (collidingCounter != null && collidingCounter.pickUpObject(out counterObject))   // Se comprueba si hay cerca una estanter�a y si tiene un objeto para coger 
            {

                handlePickObject(counterObject);
            }
            else if(collidingAppliance != null && (applianceObject = collidingAppliance.pickUpFood()) != null)
            {
                handlePickObject(applianceObject);
            }
            else if (other.gameObject.CompareTag("Objeto"))                 // Se comprueba si se tiene un objeto en el suelo
            {
                handlePickObject(other.gameObject);
            }
        }
        else
        {
            Table table = other.GetComponent<Table>();                      // Se comprueba si la colisi�n es con una mesa
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
        else if (collidingCounter != null && collidingCounter.interactWithCounter(pickedObject))      // Si hay una encimera cerca, se deja en ella el objeto
        {
            pickedObject = null;                                                
        }
        // DEJAR EN EL SUELO
        else                                                                // Si no hay encimera, se tira el objeto al suelo
        {
            pickedObject.GetComponent<Rigidbody>().useGravity = true;
            pickedObject.GetComponent<Rigidbody>().isKinematic = false;

            pickedObject.gameObject.transform.SetParent(null);

            pickedObject = null;                                               
        }
    }

    private void handleTableInteraction(Table table)
    {
        Plate plate;
        if (pickedObject != null && (plate = pickedObject.GetComponent<Plate>()) != null && Input.GetKey("q"))    // Se comprueba si se tiene en la mano un plato y se pulsa el bot�n de entregar
        {
            deliverOrder(table.getTableNumber(), plate.getCompletedRecipeName());
        }
    }

    private void deliverOrder(int tableNumber, string dish)
    {
        bool res = OrderManager.Instance.ServeDish(tableNumber, dish);

        if (res)
        {
            Destroy(pickedObject);
            pickedObject = null;
        }
        else
        {
            // Aqu� incluir� las penalizaciones que sean 
        }

    }
}