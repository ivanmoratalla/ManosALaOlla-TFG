using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fryer : KitchenAppliance
{
    public override FoodAction action
    {
        get { return FoodAction.Fry; }
    }

    // En el caso de la freidora, se quiere empezar a cocinar automáticamente tras meter la comida en la olla
    public override bool interactWithAppliance(GameObject food)
    {
        if (CanProcess(food))
        {
            placeFood(food);

            isProcessing = true;
            StartCoroutine(cookFood());

            return true;
        }
        return false;
    }

    private IEnumerator cookFood()
    {

        yield return new WaitForSeconds(3f); // Tiempo de cocción simulado

        storedFood.GetComponent<Food>().changeFoodState(action, out storedFood);
        Debug.Log("Comida cocinada");
        isProcessing = false;

        storedFood.GetComponent<Collider>().enabled = false;        // Se desactivan las colisiones y la gravedad del nuevo ingrediente instanciado para evitar que se caiga del electrodoméstico
        storedFood.GetComponent<Rigidbody>().useGravity = false;
    }
}
