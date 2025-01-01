using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : KitchenAppliance
{
    [SerializeField] private float timeToCut = 3f;  // Variable que indica el tiempo que se tiene que mantener pulsado para cortar con éxito
    private float holdTime = 0f;                    // VAriable que indica el tiempo que se lleva cortando

    private PlayerInteraction playerNearby = null;  // Variable que indica que jugador hay cerca de la tabla de cortar (null si no hay ninguno)
    private bool voiceCommandActive = false;        // Variable para indicar si la acción fue activada por comando de voz

    protected override FoodAction Action
    {
        get { return FoodAction.Cut; }              // Variable que indica que la acción que se hace en la tabla es "cortar"
    }

    private void Start()
    {
        VoiceCommandService.OnCutIngredient += HandleCutEvent;
    }

    private void OnDestroy()
    {
        VoiceCommandService.OnCutIngredient -= HandleCutEvent;

    }

    // En el caso de la tabla de cortar, no se quiere cortar al poner el objeto en la tabla, si no solo colocarlo
    public override bool InteractWithAppliance(GameObject food)
    {
        if (CanProcess(food))
        {
            PlaceFood(food);
            Debug.Log("Se ha colocado la comida en la tabla de cortar");
            return true;
        }
        return false;
    }

    private void Update()
    {
        if(storedFood != null && storedFood.GetComponent<Food>().CanTransition(Action))              // Se comprueba si hay un ingrediente en la tabla de cortar y si se puede transicionar (Pq al cortar
                                                                                                     // el ingrediente cortado después ya no se puede cortar otra vez)
        {
            if (playerNearby != null)       // Solo se puede cortar si el jugador está colisionando con la tabla
            {
                if (Input.GetKey(playerNearby.GetInputServiceAsset().GetCutFoodKey()) || voiceCommandActive)
                {
                    if (!isProcessing)
                    {
                        // El jugador ha comenzado a mantener la tecla presionada
                        isProcessing = true;
                        holdTime = 0f;
                        //storedFood.GetComponent<Collider>().enabled = false;    // Para evitar que se pueda coger mientras se cocina
                        StartProgressUI();
                    }

                    // Aumentar el contador del tiempo que se mantiene la tecla presionada
                    holdTime += Time.deltaTime;
                    OnProgressChange?.Invoke(this, holdTime / timeToCut);

                    if (holdTime >= timeToCut)
                    {
                        // Si se ha mantenido presionada la tecla durante el tiempo requerido, empezar a cortar
                        CookFood();
                    }
                }
                else if (isProcessing)
                {
                    CancelCut();

                    Debug.Log("Se ha soltado la tecla antes de tiempo. Intentar de nuevo.");
                }
            }
            else if (isProcessing)
            {
                CancelCut();

                Debug.Log("El jugador se ha alejado de la tabla de cortar.");
            }
        }
    }

    private void CancelCut()
    {
        OnProgressCanceled?.Invoke(this, null);
        isProcessing = false;
        holdTime = 0f;
        voiceCommandActive = false; // Resetear el estado del comando de voz
    }


    private void CookFood()
    {
        storedFood.GetComponent<Food>().ChangeFoodState(Action, out storedFood);
        Debug.Log("Comida cocinada");

        isProcessing = false;
        holdTime = 0f;

        storedFood.GetComponent<Collider>().enabled = false;        // Se desactivan las colisiones y la gravedad del nuevo ingrediente instanciado para evitar que se caiga del electrodoméstico
        storedFood.GetComponent<Rigidbody>().useGravity = false;

    }



    private void OnTriggerEnter(Collider other)
    {
        PlayerInteraction player = other.transform.parent.GetComponent<PlayerInteraction>();
        if (player != null)
        {
            playerNearby = player;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInteraction player = other.transform.parent.GetComponent<PlayerInteraction>();
        if (player != null)
        {
            playerNearby = null;
        }
    }

    private void HandleCutEvent(object sender, int playerId)
    {
        if (playerNearby != null && playerNearby.GetInputServiceAsset().GetPlayerId() == playerId)
        {
            voiceCommandActive = true;
        }
    }
}