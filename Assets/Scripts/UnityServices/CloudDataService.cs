using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;

public class CloudDataService: ICloudDataService
{
    public CloudDataService()
    {
        if(UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializeUnityServices();
        }  
    }

    // M�todo para inicializar los servicios de Unity si no lo est�n
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

    // M�todo para guardar las estrellas de un nivel. Hay que comprobar si la puntuaci�n que ya estaba guardada es menor para actualizarla, si no no tiene sentido
    public async Task SaveStarsForLevelIfHigher(int level, int stars)
    {
        try
        {
            int savedStars = await LoadStarsForLevel(level);

            if (stars > savedStars)                                                                             // Solo se actualiza si la nueva puntuaci�n es mayoe
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

    // M�todo para obtener las estrellas de un nivel
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

    public async Task UpdateMaxLevelIfNeeded(int completedLevel)
    {
        try
        {
            int currentMaxLevel = await LoadMaxUnlockedLevel();

            if (completedLevel == currentMaxLevel)                                                                      // Si el nivel que se ha completado es el m�ximo, hay que indicar que se desbloquea el siguiente
            {
                string key = "max_unlocked_level";

                int newMaxUnlockedLevel = completedLevel + 1;                                                           // El nuevo nivel m�ximo desbloqueado es el siguiente al que se ha completado

                var dataToSave = new Dictionary<string, object> { { key, newMaxUnlockedLevel } };

                await CloudSaveService.Instance.Data.Player.SaveAsync(dataToSave);

                Debug.Log("Nuevo nivel m�ximo guardado: " + newMaxUnlockedLevel);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error al guardar el nivel m�ximo desbloqueado: " + e.Message);
        }
    }

    public async Task<int> LoadMaxUnlockedLevel()
    {
        try
        {
            string key = "max_unlocked_level";

            var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });

            if (playerData.TryGetValue(key, out var entry))
            {
                string value = entry.Value.GetAs<string>();
                Debug.Log("Nivel m�ximo desbloqueado: " + value);

                return Convert.ToInt32(value);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error al cargar el nivel m�ximo desbloqueado: " + e.Message);
        }

        return 1;                                                                                                   // Siempre debe estar desbloqueado el primer nivel, por lo que se devuelve este n�mero si no se ha encontrado la entrada
    }

}
