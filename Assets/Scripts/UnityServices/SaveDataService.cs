using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;

public class SaveDataService: ISaveDataService
{
    public SaveDataService()
    {
        if(UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializeUnityServices();
        }  
    }

    // Método para inicializar los servicios de Unity si no lo están
    private async void InitializeUnityServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Se ha producido un error al inicializar los servicios de Unity: " + e.Message);
        }
    }

    // Método para guardar las estrellas de un nivel. Hay que comprobar si la puntuación que ya estaba guardada es menor para actualizarla, si no no tiene sentido
    public async Task SaveStarsForLevel(int level, int stars)
    {
        try
        {
            int savedStars = await LoadStarsForLevel(level);
            Debug.Log("Monedas guardadas previamente: " + savedStars);

            if (stars > savedStars)                                                                     // Solo se actualiza si la nueva puntuación es mayoe
            {
                string key = $"level_{level}_coins";

                var dataToSave = new Dictionary<string, object> { { key, stars } };

                await CloudSaveService.Instance.Data.Player.SaveAsync(dataToSave);

                Debug.Log("Monedas guardadas para el nivel " + level + ": " + stars);
            }
        }  
        catch (Exception e)
        {
            Debug.LogError("Error al guardar las estrellas del nivel " + level + ": " + e.Message);
        }
    }

    // Método para obtener las estrellas de un nivel
    public async Task<int> LoadStarsForLevel(int level)
    {
        try
        {
            string key = $"level_{level}_coins";

            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });

            if (playerData.TryGetValue(key, out var entry))
            {
                string value = entry.Value.GetAs<string>();
                Debug.Log("Monedas recuperadas para el nivel " + level + ": " + value);
                
                return Convert.ToInt32(value);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error al cargar las estrellas del nivel " + level + ": " + e.Message);
        }

        return 0;
    }

    /*// Método para cargar los datos de las estrellas que tiene cada nivel
    public async Task<Dictionary<int, int>> LoadStarsPerLevelAsync()
    {
        try
        {
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> {StarsDataKey });

            if (playerData.TryGetValue(StarsDataKey, out var starsPerLevelObject))
            {
                return (Dictionary<int, int>) starsPerLevelObject;
            }
            else
            {
                return new Dictionary<int, int>(); // Retornar un diccionario vacío si no hay datos
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load coins by level: {ex.Message}");
            return new Dictionary<int, int>(); // Retornar un diccionario vacío en caso de error
        }
    }*/

}
