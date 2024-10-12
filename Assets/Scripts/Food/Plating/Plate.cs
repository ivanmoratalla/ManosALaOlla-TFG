using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    private List<Recipe> validRecipes;                          // Esta lista guardara las recetas que se pueden hacer en función de los ingredientes que haya en el plato
    private List<string> ingredientsInPlate;                    // Esta lista guarda los ingredientes que hay en el plato
    private string completedRecipeName;                         // Variable para guardar si una receta se ha completado (su nombre)

    [SerializeField] private Transform platePoint;              // Punto donde van a aparecer los ingredientes que se añadan al plato
    private List<GameObject> instantiatedPrefabs;               // Lista con los ingredientes que se han instanciado (lo utilizaré para luego poder eliminarlos al instanciar el plato final)
    

    // Start is called before the first frame update
    void Start()
    {
        validRecipes = new List<Recipe>();
        ingredientsInPlate = new List<string>();
        completedRecipeName = null;
        instantiatedPrefabs = new List<GameObject>();
    }

    public bool addIngredient(Food food)
    {
        string ingredientToAdd = food.getStateData().getName();


        if(ingredientsInPlate.Count == 0)                       // Si no hay ingredientes añadidos, se puede añadir cualquier ingrediente que esté en al menos una receta
        {
            validRecipes = RecipeManager.Instance.getRecipesByIngredient(ingredientToAdd);

            if(validRecipes.Count > 0 )
            {
                ingredientsInPlate.Add(ingredientToAdd);        // Si se han encontrado recetas, se añade el ingrediente al plato
                updateCompletedRecipe();
                Destroy(food.gameObject);
                return true;
            }
            return false;                                       // Si no se han encontrado recetas con el ingrediente, no se añade al plato
        }
        else                                                    // Si se llega aquí, quiere decir que ya hay al menos un ingrediente en el plato
        {
            List<Recipe> newValidRecipes = new List<Recipe>();

            foreach (Recipe recipe in validRecipes)
            {
                if(recipe.containsIngredient(ingredientToAdd))
                {
                    newValidRecipes.Add(recipe);                // Hay que buscar, entre las listas que eran válidas, a las que contengan el nuevo ingrediente a añadir
                }
            }

            if(newValidRecipes.Count > 0 )                      // Si hay nuevas recetas válidas, quiere decir que existen recetas con los ingredientes que ya había en el plato y con el que se quiere añadir, por lo que se puede añador
            {
                validRecipes = newValidRecipes;                 // Se actualizan las recetas válidas con las que tienen al nuevo ingrediente
                ingredientsInPlate.Add(ingredientToAdd);
                updateCompletedRecipe();
                Destroy(food.gameObject);
                return true;
            }
            return false;
        }

    }

    private void updateCompletedRecipe()
    {
        foreach (Recipe recipe in validRecipes)
        {
            List<string> recipeIngredients = recipe.getIngredients();
            
            if(ingredientsInPlate.Count == recipeIngredients.Count)
            {
                bool match = true;

                foreach (string ingredient in recipeIngredients)
                {
                    if (!ingredientsInPlate.Contains(ingredient))      // Verificamos si el plato contiene todos los ingredientes de la receta
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    completedRecipeName = recipe.getRecipeName();       // Guardamos el nombre de la receta completada
                    Debug.Log($"¡Receta completada: {completedRecipeName}!");
                    instantiateDish();  // Mostramos el plato final
                    return;  // Salimos una vez que encontramos una receta completa
                }

            }
        }
    }

    private void instantiatePrefab(string ingredient)
    {
        string path = $"Prefabs/Ingredients/{ingredient}";

        GameObject prefab = Resources.Load<GameObject>(path);

        if (prefab != null)
        {
            // Instancia el ingrediente como hijo del objeto del plato
            GameObject instantiatedIngredient = Instantiate(prefab, platePoint.position, transform.rotation, this.transform);

            instantiatedIngredient.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f); // Ajusta estos valores según sea necesario

            // Ajusta la posición del ingrediente en el eje Y para apilar los ingredientes
            Vector3 newPosition = instantiatedIngredient.transform.localPosition;
            newPosition.y += ingredientsInPlate.Count * 0.1f; // Ajusta el multiplicador según el tamaño del ingrediente
            instantiatedIngredient.transform.localPosition = newPosition; // Establece la nueva posición local

            instantiatedPrefabs.Add(instantiatedIngredient); // Guarda el ingrediente instanciado
        }
    }

    private void instantiateDish()
    {
        foreach(GameObject ingredient in instantiatedPrefabs)
        {
            Destroy(ingredient);
        }

        instantiatedPrefabs.Clear();

        instantiatePrefab(completedRecipeName);
    }

    public string getCompletedRecipeName()
    {
        return completedRecipeName;
    }
}
