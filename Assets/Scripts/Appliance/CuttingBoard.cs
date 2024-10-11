using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : KitchenAppliance
{
    public override FoodAction action
    {
        get { return FoodAction.Cut; }
    }

    // En el caso de la olla, se quiere empezar a cocinar automáticamente tras meter la comida en la olla
    public override bool interactWithAppliance(GameObject food)
    {
        if(CanProcess(food))
        {
            placeFood(food);

            Debug.Log("Comida cocinándose en ");
            isProcessing = true;
            StartCoroutine(cookFood());

            return true;
        }
        return false;
    }

    private IEnumerator cookFood()
    {
        storedFood.GetComponent<Collider>().enabled = false;    // Para evitar que se pueda coger mientras se cocina

        yield return new WaitForSeconds(3f); // Tiempo de cocción simulado

        storedFood.GetComponent<Food>().changeFoodState(action, out storedFood);
        Debug.Log("Comida cocinada");
        isProcessing = false;
    }
}
