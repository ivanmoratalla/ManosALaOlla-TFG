using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipe")]
public class Recipe: ScriptableObject
{
    private string recipeName;           // Variable para el nombre de la receta (ensalada, hamburguesa...)
    private List<string> ingredients;    // Variable para guardar la lista de ingredientes de la receta


    public bool containsIngredient(string ingredient)
    {
        return ingredient.Contains(ingredient);
    }
}
