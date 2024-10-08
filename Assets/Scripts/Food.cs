using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    [SerializeField] private FoodData data; // Al serializarlo puedo ver el atributo en el inspector aunque sea privado. As�, desde otras clases se mantiene privado (buenas pr�cticas) pero desde el inspector se puede modificar
    [SerializeField] private FoodData nextFoodData;

    public FoodData getFoodData()
    {
        return data;
    }

}

