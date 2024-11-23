using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : KitchenAppliance
{
    public override FoodAction action
    {
        get { return FoodAction.Cook; }
    }

    [SerializeField] float cookTime = 7f;                                                           // Tiempo para que la comida esté lista
    [SerializeField] float burnTime = 15f;                                                          // Tiempo para que la comida se queme


    public override bool interactWithAppliance(GameObject food)
    {
        if (CanProcess(food))
        {
            placeFood(food);
            //switchToPanWithFood();

            isProcessing = true;            
            StartCoroutine(cookFood());
            
            return true;
        }
        return false;
    }

    private IEnumerator cookFood()
    {
        float elapsedTime = 0f;                                                                     // Variable para medir el tiempo que pasa
        Boolean cooked = false;

        while (elapsedTime < burnTime)                                                                                      
        {
            yield return null;

            if (storedFood == null)
            {
                Debug.LogWarning("La comida ha sido retirada de la sartén.Deteniendo cocción.");
                isProcessing = false;
                
                yield break;
            }

            elapsedTime += Time.deltaTime;                                                          // Se aumenta el tiempo transcurrido

            if (!cooked && elapsedTime >= cookTime && elapsedTime < burnTime)                       // Se comprueba si ha transcurrido el tiempo necesario pero menos del necesario para quemar la comida
            {
                // Comida cocinada correctamente
                cooked = true;
                storedFood.GetComponent<Food>().changeFoodState(FoodAction.Cook, out storedFood);
                storedFood.GetComponent<Collider>().enabled = false;                                // Se desactivan las colisiones y la gravedad del nuevo ingrediente instanciado para evitar que se caiga del electrodoméstico
                storedFood.GetComponent<Rigidbody>().useGravity = false;

                Debug.Log("Comida cocinada");
                isProcessing = false;                                                               // Se indica que ya no se está procesando para que el jugador la pueda coger
            }
            else if (elapsedTime >= burnTime)                                                       // Se comprueba si se ha pasado del tiempo máximo
            {
                // Comida quemada
                storedFood.GetComponent<Food>().changeFoodState(FoodAction.Burn, out storedFood);
                storedFood.GetComponent<Collider>().enabled = false;                                // Se desactivan las colisiones y la gravedad del nuevo ingrediente instanciado para evitar que se caiga del electrodoméstico
                storedFood.GetComponent<Rigidbody>().useGravity = false;

                Debug.Log("Comida quemada");
                isProcessing = false; // Finaliza el proceso.
            }
        }
    }
}
