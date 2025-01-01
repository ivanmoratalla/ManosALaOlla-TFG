using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingAppliance : KitchenAppliance
{

    [SerializeField] private FoodAction actionType;
    [SerializeField] private float cookTime = 7f;                                                           // Tiempo para que la comida esté lista
    [SerializeField] private float burnTime = 15f;                                                          // Tiempo para que la comida se queme

    protected override FoodAction Action
    {
        get { return actionType; }
    }

    public override bool InteractWithAppliance(GameObject food)
    {
        if (CanProcess(food))
        {
            PlaceFood(food);

            isProcessing = true;            
            StartCoroutine(CookFood());
            
            return true;
        }
        return false;
    }

    private IEnumerator CookFood()
    {
        float elapsedTime = 0f;                                                                     // Variable para medir el tiempo que pasa
        Boolean cooked = false;

        StartProgressUI();

        while (elapsedTime < burnTime)                                                                                      
        {
            yield return null;

            if (storedFood == null)
            {
                Debug.LogWarning("La comida ha sido retirada.Deteniendo cocción.");
                isProcessing = false;
                
                yield break;
            }

            elapsedTime += Time.deltaTime;                                                          // Se aumenta el tiempo transcurrido
            OnProgressChange?.Invoke(this, elapsedTime / cookTime);

            if (!cooked && elapsedTime >= cookTime && elapsedTime < burnTime)                       // Se comprueba si ha transcurrido el tiempo necesario pero menos del necesario para quemar la comida
            {
                // Comida cocinada correctamente
                cooked = true;
                storedFood.GetComponent<Food>().ChangeFoodState(actionType, out storedFood);
                storedFood.GetComponent<Collider>().enabled = false;                                // Se desactivan las colisiones y la gravedad del nuevo ingrediente instanciado para evitar que se caiga del electrodoméstico
                storedFood.GetComponent<Rigidbody>().useGravity = false;

                Debug.Log("Comida cocinada");
                isProcessing = false;                                                               // Se indica que ya no se está procesando para que el jugador la pueda coger
            }
            else if (elapsedTime >= burnTime)                                                       // Se comprueba si se ha pasado del tiempo máximo
            {
                // Comida quemada
                storedFood.GetComponent<Food>().ChangeFoodState(FoodAction.Burn, out storedFood);
                storedFood.GetComponent<Collider>().enabled = false;                                // Se desactivan las colisiones y la gravedad del nuevo ingrediente instanciado para evitar que se caiga del electrodoméstico
                storedFood.GetComponent<Rigidbody>().useGravity = false;

                Debug.Log("Comida quemada");
                isProcessing = false; // Finaliza el proceso.
            }
        }
    }
}
