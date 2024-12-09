using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ColorManager : MonoBehaviour
{
    private List<Type> types;                        // Lista con los tipos cuyos objetos se pueden cambiar de oclor
    private Dictionary<Type, List<ColorableObject>> levelObjectsByType; // Por cada tipo de objeto coloreable, una lista con las instancias de ese tipo en el nivel
    private Dictionary<Type, Material> alternativeMaterials;            // Material alternativo por cada tipo de objeto coloreable
    private Dictionary<Type, bool> useAlternativeMaterial;              // Por cada tipo de objeto coloreable, si se usa o no su material alternativo

    private void Awake()
    {
        types = new List<Type>();
        levelObjectsByType = new Dictionary<Type, List<ColorableObject>>();
        alternativeMaterials = new Dictionary<Type, Material>();
        useAlternativeMaterial = new Dictionary<Type, bool>();

        types.Add(typeof(Food));
        types.Add(typeof(Counter));
        types.Add(typeof(Plate));
        types.Add(typeof(KitchenAppliance));


        for(int i = 0; i < types.Count; i++)
        {
            Type type = types[i];

            levelObjectsByType.Add(type, new List<ColorableObject>());
            alternativeMaterials.Add(type, Resources.Load<Material>($"Materials/{type.Name}_Material"));
            useAlternativeMaterial.Add(type, false);
        }
    }

    private void Start()
    {
        LoadSavedData();
    }

    // Método para registrar un objeto que puede cambiar de color
    public void RegisterObject(ColorableObject obj)
    {
        Type objType = obj.GetType();

        if (!types.Contains(objType))                                   // Si no existe una lista para ese tipo, se crea
        {
            return;
        }

        if (!levelObjectsByType[objType].Contains(obj))                                 // Si la lista no contiene el objeto, se añade y se le aplica el material que sea
        {
            levelObjectsByType[objType].Add(obj);

            bool useAlternate = useAlternativeMaterial[objType];
            obj.ApplyMaterial(useAlternate ? GetAlternateMaterialFor(objType) : null);
        }

        
    }

    // Método para elimiar de las listas un objeto que puede cambiar de color
    public void UnregisterObject(ColorableObject obj)
    {
        Type objType = obj.GetType();

        // Remueve el objeto de la lista correspondiente
        if (levelObjectsByType.ContainsKey(objType))
        {
            levelObjectsByType[objType].Remove(obj);
        }
    }

    // Método para cambiar el color del material alternativo
    public void SetAlternativeColorForType(Type type, Color newColor)
    {
        if (types.Contains(type))
        {
            alternativeMaterials[type].color = newColor;
            SaveColor(type, newColor);
            
            if (useAlternativeMaterial[type])
            {
                foreach (var obj in levelObjectsByType[type])
                {
                    obj.ApplyMaterial(alternativeMaterials[type]);
                }
            }
        }
    }

    public Color GetAlternativeColorForType(Type type)
    {
        if(types.Contains(type))
        {
            return alternativeMaterials[type].color;
        }
        return Color.white;
    }

    // Método para activar el color alternativo de los objetos de un determinado tipo
    public void ActivateAlternateMaterialsForType(Type type)
    {
        useAlternativeMaterial[type] = true;

        ApplyMaterialForType(type, GetAlternateMaterialFor(type));

        SaveState(type, true);
    }

    // Método para desactivar el color alternativo de los objetos de un determinado tipo
    public void DisableAlternateMaterialsForType(Type type)
    {
        useAlternativeMaterial[type] = false;

        ApplyMaterialForType(type, null); 

        SaveState(type, false);
    }

    // Método privado para aplicar un material a todos los objetos de un tipo
    private void ApplyMaterialForType(Type type, Material material)
    {
        if(types.Contains(type))
        {
            foreach (ColorableObject obj in levelObjectsByType[type])
            {
                obj.ApplyMaterial(material);
            }
        }
    }

    // Método privado para obtener el material alternativo de un tipo de objetos
    private Material GetAlternateMaterialFor(Type type)
    {
        alternativeMaterials.TryGetValue(type, out Material material);
        return material;
    }

    // Método para guardar el color del material alternativo (Persistencia de datos)
    private void SaveColor(Type type, Color color)
    {
        string key = $"{type.Name}_Color";
        PlayerPrefs.SetString(key, "#" + ColorUtility.ToHtmlStringRGBA(color));
    }

    // Método para guardar el estado del material alternativo (si está activo o no) (Persistencia de datos)
    private void SaveState(Type type, bool state)
    {
        string key = $"{type.Name}_State";
        PlayerPrefs.SetInt(key, state ? 1 : 0);
    }

    // Método para cargar los datos guardados
    private void LoadSavedData()
    {
        foreach (var type in alternativeMaterials.Keys)
        {
            string colorKey = $"{type.Name}_Color";
            string stateKey = $"{type.Name}_State";

            // Cargar el color guardado
            if (PlayerPrefs.HasKey(colorKey))
            {
                string colorString = PlayerPrefs.GetString(colorKey);
                if (ColorUtility.TryParseHtmlString(colorString, out Color loadedColor))
                {
                    Debug.Log("Color cargado para " + type + " : " + colorString);
                    alternativeMaterials[type].color = loadedColor;
                }
            }

            // Cargar el estado guardado (si se usa material alternativo)
            if (PlayerPrefs.HasKey(stateKey))
            {
                bool useAltMaterial = PlayerPrefs.GetInt(stateKey) == 1;

                // Si el estado es true, aplica el material alternativo
                if (useAltMaterial)
                {
                    Debug.Log("Estado true para " + type);
                    useAlternativeMaterial[type] = true;
                    ActivateAlternateMaterialsForType(type);
                }
                else
                {
                    useAlternativeMaterial[type] = false;
                    DisableAlternateMaterialsForType(type);
                }
            }
        }
    }
}

