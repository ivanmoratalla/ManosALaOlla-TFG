using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorableObject : MonoBehaviour
{
    public Material originalMaterial;
    private Renderer foodRenderer;

    private void Awake()
    {
        foodRenderer = GetComponent<Renderer>();

        if (foodRenderer != null)
        {
            originalMaterial = foodRenderer.material;
        }

    }

    private void OnEnable()
    {
        // Registra este objeto en el ColorManager
        ColorManager manager = FindObjectOfType<ColorManager>();
        if (manager != null)
        {
            manager.RegisterObject(this);
        }
    }

    private void OnDisable()
    {
        // Desregistra este objeto del ColorManager
        ColorManager manager = FindObjectOfType<ColorManager>();
        if (manager != null)
        {
            manager.UnregisterObject(this);
        }
    }

    public void ApplyMaterial(Material alternateMaterial)
    {
        if (foodRenderer != null)
        {
            // Si no hay material alternativo, vuelve al original
            foodRenderer.material = alternateMaterial != null ? alternateMaterial : originalMaterial;
        }
    }
}
