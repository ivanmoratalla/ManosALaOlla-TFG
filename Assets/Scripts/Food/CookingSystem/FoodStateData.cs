using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "NewFoodState", menuName = "FoodState")]
public class FoodStateData : ScriptableObject
{
    [SerializeField] private string foodName;                       // Nombre del estado (una comida o ingredinte)
    [SerializeField] private List<FoodTransition> transitions;      // Esto son las transiciones entre estados que se puede hacer en función de la acción que se haga
    [SerializeField] private Sprite ingredientSpriteForUI;          // Sprite del ingrediente para añadirlo a la UI de la comanda
    
    public string GetName()
    {
        return foodName; 
    }

    public List<FoodTransition> GetTransitions()
    {
        return transitions;
    }

    public Sprite GetIngredientSprite()
    {
        return ingredientSpriteForUI;
    }
}
