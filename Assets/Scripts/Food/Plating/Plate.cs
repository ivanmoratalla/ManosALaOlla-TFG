using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public List<Recipe> validRecipes;                           // Esta lista guardara las recetas que se pueden hacer en funci�n de los ingredientes que haya en el plato
    public List<string> ingredientsInPlate;                     // Esta lista guarda los ingredientes que hay en el plato

    // Start is called before the first frame update
    void Start()
    {
        validRecipes = new List<Recipe>();
        ingredientsInPlate = new List<string>();
    }

    public bool addIngredient(Food food)
    {
        string ingredientToAdd = food.getStateData().getName();


        if(ingredientsInPlate.Count == 0)                       // Si no hay ingredientes a�adidos, se puede a�adir cualquier ingrediente que est� en al menos una receta
        {
            validRecipes = RecipeManager.Instance.getRecipesByIngredient(ingredientToAdd);

            if(validRecipes.Count > 0 )
            {
                ingredientsInPlate.Add(ingredientToAdd);        // Si se han encontrado recetas, se a�ade el ingrediente al plato
                return true;
            }
            return false;                                       // Si no se han encontrado recetas con el ingrediente, no se a�ade al plato
        }
        else                                                    // Si se llega aqu�, quiere decir que ya hay al menos un ingrediente en el plato
        {
            List<Recipe> newValidRecipes = new List<Recipe>();

            foreach (Recipe recipe in validRecipes)
            {
                if(recipe.containsIngredient(ingredientToAdd))
                {
                    newValidRecipes.Add(recipe);                // Hay que buscar, entre las listas que eran v�lidas, a las que contengan el nuevo ingrediente a a�adir
                }
            }

            if(newValidRecipes.Count > 0 )                      // Si hay nuevas recetas v�lidas, quiere decir que existen recetas con los ingredientes que ya hab�a en el plato y con el que se quiere a�adir, por lo que se puede a�ador
            {
                validRecipes = newValidRecipes;                 // Se actualizan las recetas v�lidas con las que tienen al nuevo ingrediente
                ingredientsInPlate.Add(ingredientToAdd);
                return true;
            }
            return false;
        }

    }
}
