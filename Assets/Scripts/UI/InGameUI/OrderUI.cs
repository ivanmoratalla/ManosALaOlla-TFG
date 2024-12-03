using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    [SerializeField] private Image timerBar;

    [SerializeField] private Image recipeImage;
    [SerializeField] private Text assignedTable;

    [SerializeField] private Transform ingredientContainer;         // Contenedor para las imágenes de los ingredientes
    [SerializeField] private GameObject ingredientImagePrefab;      // Prefab para las imágenes de los ingredientes


    private void OnEnable()
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.localScale = Vector3.zero;
        LeanTween.scale(rect, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutSine);
    }


    // Public Methods
    public void SetOrder(KeyValuePair<int, Recipe> orderData)
    {
        recipeImage.sprite = orderData.Value.getRecipeSprite();     // Se establece la imagen de la receta
        assignedTable.text = "Mesa: " + orderData.Key.ToString();   // Se establece la mesa en la que va el pedido

        // Se crean las imágenes de los ingredientes 
        foreach (FoodStateData ingredient in orderData.Value.getIngredients())
        {
            GameObject newIngredientImage = Instantiate(ingredientImagePrefab, ingredientContainer);
            Image ingredientImage = newIngredientImage.GetComponent<Image>();

            // Aquí necesitas un sistema para mapear el nombre del ingrediente a su sprite
            Sprite ingredientSprite = ingredient.getIngredientSprite();
            if (ingredientSprite != null)
            {
                ingredientImage.sprite = ingredientSprite;
            }
            else
            {
                Debug.LogWarning($"No se encontró sprite para el ingrediente: {ingredient}");
            }
        }
    }

    public void UpdateBar(float newTime)
    {
        timerBar.fillAmount = newTime;

        if (newTime <= 0)
        {
            LeanTween.color(gameObject.GetComponent<RectTransform>(), Color.red, 0.5f).setDestroyOnComplete(true);
        }

    }

    public void SuccessAndDestroy()
    {
        LeanTween.color(gameObject.GetComponent<RectTransform>(), Color.green, 0.5f).setDestroyOnComplete(true);
    }

    public void FailAndDestroy()
    {
        LeanTween.color(gameObject.GetComponent<RectTransform>(), Color.red, 0.5f).setDestroyOnComplete(true);
    }
}
