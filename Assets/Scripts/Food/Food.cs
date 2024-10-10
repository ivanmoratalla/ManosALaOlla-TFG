using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Esta clase representa a una comida, es decir, a un estado del autómata. Por tanto, incluye info del estado, junto a las transiciones que se pueden hacer desde este estado
public class Food : MonoBehaviour
{
    [SerializeField] private FoodStateData stateData; // Al serializarlo puedo ver el atributo en el inspector aunque sea privado. Así, desde otras clases se mantiene privado (buenas prácticas) pero desde el inspector se puede modificar
    [SerializeField] private List<FoodTransition> transitions; // Esto son las transiciones entre estados que se puede hacer en función de la acción que se haga

    public FoodStateData getStateData()
    {
        return stateData;
    }

    // Método para obtener el estado resultante basado en una acción específica
    public FoodStateData GetNextState(FoodAction action)
    {

        foreach (FoodTransition transition in transitions)
        {
            if (transition.getAction() == action)
            {
                return transition.getNextState();
            }
        }

        Debug.LogWarning($"No se encontró transición para la acción: {action}");
        return null;
    }

}

