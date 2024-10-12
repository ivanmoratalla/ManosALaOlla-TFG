using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    [SerializeField] private string recipeName;           // Variable para el nombre de la receta (ensalada, hamburguesa...)
    [SerializeField] private List<string> ingredients;    // Variable para guardar la lista de ingredientes de la receta


    public bool containsIngredient(string ingredient)
    {
        return ingredients.Contains(ingredient);
    }

    public string getRecipeName()
    {
        return recipeName;
    }
    public List<string> getIngredients()
    {
        return ingredients;
    }
}
