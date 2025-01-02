using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    [SerializeField] private GameObject hand;                                       // Punto donde el objeto que el jugador tiene en la mano va a estar (posición)
    private GameObject pickedObject = null;                                         // Objeto que el jugador tiene en la mano

    [SerializeField] private InputServiceAsset inputService;                        // Servicio al que se le llamará para saber qué tecla corresponde a cada acción

    public static event Action<int, string, Action<bool>> OnTryToServeDish;         // Evento para notificar cuando se quiere servir un pedido

    private List<GameObject> nearbyInteractables = new List<GameObject>();          // Lista con los objetos cercanos al jugador interactuables
    private GameObject closestInteractable = null;                                  // Objeto más cercano de todos

    private void OnEnable()
    {
        // Se suscribe a los eventos de acciones por comando de voz
        VoiceCommandService.OnPickUpObject += HandlePickUpObjectEvent;
        VoiceCommandService.OnReleaseObject += HandleReleaseObjectEvent;
        VoiceCommandService.OnServeDish += HandleServeDishEvent;
    }

    private void OnDisable()
    {
        // Se desuscribe de los eventos de acciones por comando de voz para evitar fallos y solapamientos.
        VoiceCommandService.OnPickUpObject -= HandlePickUpObjectEvent;
        VoiceCommandService.OnReleaseObject -= HandleReleaseObjectEvent;
        VoiceCommandService.OnServeDish -= HandleServeDishEvent;
    }

    void Update()
    {
        UpdateClosestInteractable();
        HandleInput();
    }

    // Método para actualizar el objeto interactuable más cercano al jugador. Se llama en cada frame
    private void UpdateClosestInteractable()
    {
        Highlighter highliter = new Highlighter();

        if (nearbyInteractables.Count <= 0)
        {
            if (closestInteractable != null)
            {
                highliter.StopHighlight(closestInteractable);
            }
            closestInteractable = null;
            return;
        }

        float minDistance = Mathf.Infinity;
        GameObject closest = null;

        foreach (var interactable in nearbyInteractables)
        {
            if (interactable != null && interactable != pickedObject)                                                        // El objeto en la mano no se considera, ya que si no sería siempre el más cercano
            {
                if (interactable.transform.parent != null && (interactable.transform.parent.GetComponent<Counter>() != null
                    || interactable.transform.parent.GetComponent<KitchenAppliance>() != null)) // Si un objeto está sobre una encimera no se considera para ser el closest tile, ya que para quitarlo de la encimera
                {                                                                                                           // quiero usar el método de la misma, ya que si no seguiría apareciendo que hay un objeto cuando no
                    continue; // Saltar objetos que están en una encimera
                }

                float distance = Vector3.Distance(transform.position, interactable.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = interactable;
                }
            }
        }

        if (closestInteractable != closest)
        {
            if (closestInteractable != null)
            {
                highliter.StopHighlight(closestInteractable);
            }

            closestInteractable = closest;

            if (closestInteractable != null)
            {
                highliter.StartHighlight(closestInteractable);
            }
        }
    }

    // Método para manejar la acción de coger objeto mediante comandos de voz
    private void HandlePickUpObjectEvent(object sender, int playerId)
    {
        if (inputService.GetPlayerId() == playerId)
        {
            HandlePickUpObject();
        }
    }

    // Método para manejar la acción de soltar objeto mediante comandos de voz
    private void HandleReleaseObjectEvent(object sender, int playerId)
    {
        if (inputService.GetPlayerId() == playerId)
        {
            HandleReleaseObject();
        }
    }

    // Método para manejar la acción de servir plato mediante comandos de voz
    private void HandleServeDishEvent(object sender, int playerId)
    {
        if (inputService.GetPlayerId() == playerId)
        {
            HandleServeDish();
        }
    }

    // Método para comprobar si el usuario está pulsando alguna de las teclas asociadas a acciones
    private void HandleInput()
    {
        if (Input.GetKeyDown(inputService.GetPickObjectKey()))
        {
            HandlePickUpObject();
        }
        else if (Input.GetKeyDown(inputService.GetReleaseObjectKey()))
        {
            HandleReleaseObject();
        }
        else if (Input.GetKeyDown(inputService.GetServeDishKey()))
        {
            HandleServeDish();
        }
    }

    // Método encargado de coger un objeto
    private void HandlePickUpObject()
    {
        if (pickedObject != null || closestInteractable == null)
        {
            return;
        }
        
        GameObject objectToPick = null;
        if ((closestInteractable.TryGetComponent<Counter>(out Counter counter) && counter.pickUpObject(out objectToPick))
            || (closestInteractable.TryGetComponent<KitchenAppliance>(out KitchenAppliance appliance) && (objectToPick = appliance.PickUpFood()) != null)
            || (closestInteractable.TryGetComponent<Crate>(out Crate crate) && (objectToPick = crate.PickUpFood()) != null))
        {
            PickObject(objectToPick);
        }
        else if (closestInteractable.gameObject.CompareTag("Objeto"))
        {
            PickObject(closestInteractable.gameObject);
        }
    }

    /* Este método es el encargado de manejar cuando el usuario quiere soltar un objeto. Lo puede hacer por cuatro motivos:
     * - Dejar un objeto en una encimera
     * - Dejar un ingrediente en un plato
     * - Dejar un objeto en el suelo
     * - Dejar un objeto, tipo comida, en un electrodoméstico para cocinarlo (el electrodoméstico se encargará de mirar si se puede)
     */
    private void HandleReleaseObject()
    {
        if (pickedObject != null && closestInteractable != null)
        {
            // INTERACTUAR CON UN ELECTRODOMÉSTICO
            if (pickedObject.GetComponent<Food>() != null && closestInteractable.TryGetComponent<KitchenAppliance>(out KitchenAppliance appliance) && appliance.InteractWithAppliance(pickedObject))
            {
                Debug.Log("Se ha interactuado con el electrodoméstico");
                pickedObject.GetComponent<Collider>().enabled = true;
                pickedObject = null;

            }
            // INTERACTUAR CON UNA ENCIMERA, TANTO PARA DEJAR OBJETO COMO PARA EMPLATAR
            else if (closestInteractable.TryGetComponent<Counter>(out Counter counter) && counter.interactWithCounter(pickedObject))
            {
                Debug.Log("Se ha interactuado con una encimera");
                pickedObject.GetComponent<Collider>().enabled = true;

                pickedObject = null;
            }
        }
        // DEJAR EN EL SUELO
        else if (pickedObject != null && pickedObject.GetComponent<Plate>() == null)   // Se deja en el suelo si no se está cerca de un electrodoméstico o encimera, para evitar colisiones erróneas con lo que hay en ellos.                                                                                            
        {                                                                                                               // Además, se impide que los platos se puedan dejar en el suelo (no se podrían coger luego porque no se detectaría colisión al ser tan bajos)
            pickedObject.GetComponent<Collider>().enabled = true;
            DropObjectOnGround();
        }
    }

    // Método encargado de servir un plato
    private void HandleServeDish()
    {
        if (pickedObject != null && closestInteractable != null && closestInteractable.TryGetComponent<Table>(out Table table))
        {
            Plate plate;
            if ((plate = pickedObject.GetComponent<Plate>()) != null)          // Se comprueba si se tiene en la mano un plato y se pulsa el botón de entregar
            {
                OnTryToServeDish?.Invoke(table.getTableNumber(), plate.GetCompletedRecipeName(), res =>
                {
                    if (res)
                    {
                        Destroy(pickedObject);
                        pickedObject = null;
                    }
                    else
                    {
                        // Aquí incluiré las penalizaciones que sean 
                    }
                });
            }
        }
    }

    // Método para soltar un objeto en el suelo
    private void DropObjectOnGround()
    {
        Debug.Log("Se deja en el suelo");

        var rb = pickedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        pickedObject.transform.SetParent(null);
        pickedObject = null;
    }

    // Este metodo es llamado cuando el jugador entra en contacto con otro objeto
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Counter>() != null || other.GetComponent<KitchenAppliance>() || other.GetComponent<Crate>() != null || other.GetComponent<Table>() != null || other.CompareTag("Objeto"))
        {
            if (!nearbyInteractables.Contains(other.gameObject))
            {
                nearbyInteractables.Add(other.gameObject);
            }
        }
    }

    // Este metodo es llamado cuando el jugador deja de estar en contacto con otro objeto
    private void OnTriggerExit(Collider other)
    {
        if (nearbyInteractables.Contains(other.gameObject))
        {
            nearbyInteractables.Remove(other.gameObject);
            if (closestInteractable == other.gameObject)
            {
                UpdateClosestInteractable();
            }
        }
    }

    // Este método es el encargado de coger el objeto que se pasa como parámetro
    private void PickObject(GameObject other)
    {
        other.GetComponent<Rigidbody>().useGravity = false;
        other.GetComponent<Rigidbody>().isKinematic = true;

        other.transform.position = hand.transform.position;
        other.gameObject.transform.SetParent(hand.gameObject.transform);
        other.GetComponent<Collider>().enabled = false;

        pickedObject = other.gameObject;
    }

    public InputServiceAsset GetInputServiceAsset()
    {
        return this.inputService;
    }
}