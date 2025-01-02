using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] private GameObject ingredientInCrate;

    public GameObject PickUpFood()
    {
        GameObject ingredient = Instantiate(ingredientInCrate);

        return ingredient;
    }
}
