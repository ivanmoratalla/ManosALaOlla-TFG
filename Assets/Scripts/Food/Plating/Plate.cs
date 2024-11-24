using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    private List<Recipe> validRecipes;                          // Esta lista guardara las recetas que se pueden hacer en funci�n de los ingredientes que haya en el plato
    private List<string> ingredientsInPlate;                    // Esta lista guarda los ingredientes que hay en el plato
    private string completedRecipeName;                         // Variable para guardar si una receta se ha completado (su nombre)
    private GameObject completedRecipePrefab;                   // Variable para guardar el prefab de una receta completada

    //[SerializeField] private Transform platePoint;              // Punto donde van a aparecer los ingredientes que se a�adan al plato
    private List<GameObject> instantiatedPrefabs;               // Lista con los ingredientes que se han instanciado (lo utilizar� para luego poder eliminarlos al instanciar el plato final)
    

    // Start is called before the first frame update
    void Start()
    {
        validRecipes = new List<Recipe>();
        ingredientsInPlate = new List<string>();
        completedRecipeName = null;
        completedRecipePrefab = null;
        instantiatedPrefabs = new List<GameObject>();
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
                addIngredientToPlate(food);
                updateCompletedRecipe();
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
                addIngredientToPlate(food);
                updateCompletedRecipe();

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
                    Debug.Log($"�Receta completada: {completedRecipeName}!");
                    instantiateRecipePrefab(recipe.getRecipePrefab());
                    return;  // Salimos una vez que encontramos una receta completa
                }

            }
        }
    }


    private void addIngredientToPlate(Food food)
    {
        food.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        food.GetComponent<Collider>().enabled = false;

        // Ajustar la posici�n del nuevo ingrediente justo encima de los ingredientes previos
        Vector3 newPosition = this.transform.position;  // Usar la posici�n del propio objeto Plate
        newPosition.y += calculatePosition(food);  // Colocar justo encima del �ltimo ingrediente o plato

        // Aplicar la nueva posici�n y escalar el ingrediente (por ejemplo, al 50% de su tama�o original)
        food.transform.position = newPosition;

        food.transform.SetParent(transform);

        instantiatedPrefabs.Add(food.gameObject);
    }

    // Este m�todo ser� llamado cuando una receta est� complets, con el objetivo de instanciar el prefab de la receta en el mundo, y no tener los ingredientes por separado
    private void instantiateRecipePrefab(GameObject prefab)
    {
        if(prefab != null)
        {
            foreach(GameObject ingredients in instantiatedPrefabs)
            {
                Destroy(ingredients.gameObject);
            }
            instantiatedPrefabs.Clear();

            Destroy(completedRecipePrefab);

            completedRecipePrefab = Instantiate(prefab, this.transform.position, Quaternion.identity);
            completedRecipePrefab.transform.SetParent(this.transform);
        }
    }

    // Este m�todo se encarga de calcular la posici�n en el plato d�nde poner el ingrediente, para que no se quede volando ni atraviese al plato u otros ingredientes
    private float calculatePosition(Food food)
    {
        float ingredientHeight = 0f;                                    // Altura del ingrediente
        float accumulatedHeight = 0f;                                   // Altura sumada de todos los ingredientes que haya ya en el plato

        Renderer ingredientRenderer = food.GetComponent<Renderer>();
        if(ingredientRenderer != null )
        {
            ingredientHeight = ingredientRenderer.bounds.size.y;
        }

        foreach (GameObject prefab in instantiatedPrefabs)
        {
            Renderer prefabRenderer = prefab.GetComponent<Renderer>();
            if (prefabRenderer != null)
            {
                accumulatedHeight += prefabRenderer.bounds.size.y;
            }
        }
        return accumulatedHeight /*+ (ingredientHeight / 2)*/;              // Con esto se consigue que el punto en el que se isntancie sea el exacto
    }


    public string getCompletedRecipeName()
    {
        return completedRecipeName;
    }
}
