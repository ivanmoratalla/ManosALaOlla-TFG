using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    public GameObject hand;                                                 // Punto donde el objeto que el jugador tiene en la mano va a estar (posición)
    private GameObject pickedObject = null;                                 // Objeto que el jugador tiene en la mano
    private Counter collidingCounter = null;                                // Esta variable índica si tengo una encimera con la que el personaje está colisionando, para poder coger/soltar objetos en ella
    private KitchenAppliance collidingAppliance = null;                     // Esta variable índica si tengo un electrodoméstico con el que el personaje está colisionando, para poder interactuar o no con él


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

        // Se comprueba si se ha entrado en contacto con un electrodoméstico
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
            if (collidingCounter != null && collidingCounter.hasObject())   // Se comprueba si hay cerca una estantería y si tiene un objeto para coger 
            {
                GameObject counterObject = collidingCounter.pickUpObject();

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
            Table table = other.GetComponent<Table>();                      // Se comprueba si la colisión es con una mesa
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
            collidingCounter = null;                                        // Si se ha dejado de estar en contacto con una encimera, se indica en la variable correspondiente que ya no se tiene una encimera cerca dónde dejar el objeto
        }

        KitchenAppliance appliance = other.GetComponent<KitchenAppliance>();
        if (appliance != null)
        {
            collidingAppliance = null;
        }
    }


    // Este método es el encargado de coger el objeto que se pasa como parámetro
    private void handlePickObject(GameObject other)
    {
        other.GetComponent<Rigidbody>().useGravity = false;                 
        other.GetComponent<Rigidbody>().isKinematic = true;                

        other.transform.position = hand.transform.position;
        other.gameObject.transform.SetParent(hand.gameObject.transform);

        pickedObject = other.gameObject;
    }

    /* Este método es el encargado de manejar cuando el usuario quiere soltar un objeto. Lo puede hacer por cuatro motivos:
     * - Dejar un objeto en una encimera
     * - Dejar un ingrediente en un plato
     * - Dejar un objeto en el suelo
     * - Dejar un objeto, tipo comida, en un electrodoméstico para cocinarlo (el electrodoméstico se encargará de mirar si se puede)
     */
    private void handleReleaseObject()
    {
        // INTERACTUAR CON UN ELECTRODOMÉSTICO
        if(pickedObject.GetComponent<Food>() != null && collidingAppliance != null && collidingAppliance.interactWithAppliance(pickedObject))
        {
            Debug.Log("Se ha interactuado con el electrodoméstico");
            pickedObject = null;                                               

        }
        // DEJAR EN UNA ENCIMERA
        else if (collidingCounter != null && !collidingCounter.hasObject())      // Si hay una encimera cerca, se deja en ella el objeto
        {
            collidingCounter.placeObject(pickedObject);
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
        Food dish;
        if (pickedObject != null && (dish = pickedObject.GetComponent<Food>()) != null && Input.GetKey("q"))    // Se comprueba si se tiene en la mano un plato y se pulsa el botón de entregar
        {
            deliverOrder(table.getTableNumber(), dish.getStateData().getName());
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
            // Aquí incluiré las penalizaciones que sean 
        }

    }
}