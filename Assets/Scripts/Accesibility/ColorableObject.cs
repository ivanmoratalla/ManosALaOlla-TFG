using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorableObject : MonoBehaviour
{
    private Material originalMaterial;
    private List<Renderer> renderers = new List<Renderer>();


    private void Awake()
    {
        if (this is Food)
        {
            // Si es de tipo Food, se busca los renderizadores en los hijos (porque algunas Comidas tienen varios GameObjects dentro al haberlas creado manualmente)
            Renderer[] foundRenderers = GetComponentsInChildren<Renderer>();
            if (foundRenderers.Length > 0)
            {
                renderers.AddRange(foundRenderers);
                originalMaterial = foundRenderers[0].material;                  // Se usa el material del primer renderer como material original por defecto
            }
        }
        else
        {
            // Se busca el Renderer del objeto actual si no es de tipo Food
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderers.Add(renderer);
                originalMaterial = renderer.material;
            }
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
        if (renderers.Count > 0)
        {
            // Se aplica el material alternativo o el original a todos los renderers
            foreach (Renderer renderer in renderers)
            {
                renderer.material = alternateMaterial != null ? alternateMaterial : originalMaterial;
            }
        }
    }
}
