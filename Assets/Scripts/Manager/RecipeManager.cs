using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance { get; private set; }

    private List<Recipe> recipes;        // Esta es la lista d�nde aparecer�n todas las recetas que se pueden cocinar

    void Awake()
    {
        Instance = this;
    }

    // Este m�todo se utilizar� para encontrar todas las recetas que incluyen el primer ingrediente que se a�ada a un plato
    public List<Recipe> getRecipesByIngredient(string ingredient)
    {
        List<Recipe> recipesWithIngredient = new List<Recipe>();

        foreach (Recipe recipe in recipes)
        {
            if (recipe.containsIngredient(ingredient))
            {
                recipesWithIngredient.Add(recipe);
            }
        }
        
        return recipesWithIngredient;
    }
}
