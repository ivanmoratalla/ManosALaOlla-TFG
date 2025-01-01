using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    [SerializeField] private string recipeName;                         // Variable para el nombre de la receta (ensalada, hamburguesa...)
    [SerializeField] private List<FoodStateData> ingredientsList;       // Variable para guardar la lista de ingredientes de la receta

    [SerializeField] private GameObject recipePrefab;                   // Variable que guarda el prefab de la receta
    [SerializeField] private Sprite recipeSprite;


    public bool containsIngredient(string ingredient)
    {
        foreach(FoodStateData foodData in ingredientsList)
        {
            if(foodData.GetName() == ingredient)
            {
                return true;
            }
        }
        return false;
    }

    public string getRecipeName()
    {
        return recipeName;
    }

    public List<FoodStateData> getIngredients()
    {
        return ingredientsList;
    }

    public GameObject getRecipePrefab()
    {
        return recipePrefab;
    }

    public Sprite getRecipeSprite()
    {
        return recipeSprite;
    }
}
