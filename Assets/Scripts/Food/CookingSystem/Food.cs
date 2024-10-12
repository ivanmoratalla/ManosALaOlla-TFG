using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Esta clase representa a una comida, es decir, a un estado del aut�mata. Por tanto, incluye info del estado, junto a las transiciones que se pueden hacer desde este estado
public class Food : MonoBehaviour
{
    [SerializeField] private FoodStateData stateData; // Al serializarlo puedo ver el atributo en el inspector aunque sea privado. As�, desde otras clases se mantiene privado (buenas pr�cticas) pero desde el inspector se puede modificar
    

    public FoodStateData getStateData()
    {
        return stateData;
    }

    // M�todo para obtener el estado siguiente (si lo hay) en funci�n de la acci�n
    private GameObject getNextState(FoodAction action)
    {

        foreach (FoodTransition transition in stateData.getTransitions())
        {
            if (transition.getAction() == action)
            {
                return transition.getNextStatePrefab();
            }
        }

        Debug.LogWarning($"No se encontr� transici�n para la acci�n: {action}");
        return null;
    }

    public void changeFoodState(FoodAction action, out GameObject go)
    {
        go = null;

        GameObject nextStatePrefab = getNextState(action);
        if (nextStatePrefab != null)
        {
            go = Instantiate(nextStatePrefab, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }

    public bool canTransition(FoodAction action)
    {

        foreach (FoodTransition transition in stateData.getTransitions())
        {
            if (transition.getAction() == action)
            {
                return true;
            }
        }

        Debug.LogWarning($"No se encontr� transici�n para la acci�n: {action}");
        return false;
    }
}

