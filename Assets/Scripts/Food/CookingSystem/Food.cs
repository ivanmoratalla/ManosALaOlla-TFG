using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Esta clase representa a una comida, es decir, a un estado del autómata. Por tanto, incluye info del estado, junto a las transiciones que se pueden hacer desde este estado
public class Food : ColorableObject
{
    [SerializeField] private FoodStateData stateData; // Al serializarlo puedo ver el atributo en el inspector aunque sea privado. Así, desde otras clases se mantiene privado (buenas prácticas) pero desde el inspector se puede modificar

    public bool CanTransition(FoodAction action)
    {

        foreach (FoodTransition transition in stateData.GetTransitions())
        {
            if (transition.GetAction() == action)
            {
                return true;
            }
        }

        Debug.LogWarning($"No se encontró transición para la acción: {action}");
        return false;
    }

    public void ChangeFoodState(FoodAction action, out GameObject go)
    {
        go = null;

        GameObject nextStatePrefab = GetNextState(action);
        if (nextStatePrefab != null)
        {
            go = Instantiate(nextStatePrefab, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }

    // Método para obtener el estado siguiente (si lo hay) en función de la acción
    private GameObject GetNextState(FoodAction action)
    {
        foreach (FoodTransition transition in stateData.GetTransitions())
        {
            if (transition.GetAction() == action)
            {
                return transition.GetNextStatePrefab();
            }
        }

        Debug.LogWarning($"No se encontró transición para la acción: {action}");
        return null;
    }

    public FoodStateData GetStateData()
    {
        return stateData;
    }
}

