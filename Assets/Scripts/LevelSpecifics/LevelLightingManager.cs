using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLightingManager : MonoBehaviour
{
    [SerializeField] private List<Light> lights;        // Luces a encender y apagar
    [SerializeField] private float onDuration = 10f;    // Tiempo encendido
    [SerializeField] private float offDuration = 5f;    // Tiempo apagado

    private Color originalAmbientColor;                 // Variable para guardar el color original
    private float originalAmbientIntensity;             // Variable para guardar la intensidad original
    private float originalReflectionIntensity;          // Variable para guardar la intensidad de reflexi�n original

    private void Start()
    {
        // Se guardan las configuraciones originales de iluminaci�n
        originalAmbientColor = RenderSettings.ambientLight;
        originalAmbientIntensity = RenderSettings.ambientIntensity;
        originalReflectionIntensity = RenderSettings.reflectionIntensity;

        StartCoroutine(ToggleDarkness());               // Se comienza la alternancia de luces
    }

    // M�todo para alternar entre luces encendidas y oscuridad
    private IEnumerator ToggleDarkness()
    {
        while (true)
        {
            // ENCENDER luces y restaurar iluminaci�n ambiental
            SetLightsActive(true);
            RenderSettings.ambientLight = originalAmbientColor;
            RenderSettings.ambientIntensity = originalAmbientIntensity;
            RenderSettings.reflectionIntensity = originalReflectionIntensity;
            yield return new WaitForSeconds(onDuration);        // Se dejan activas durante onDuration segundos

            // APAGAR luces y reducir la iluminaci�n ambiental
            SetLightsActive(false);
            RenderSettings.ambientLight = Color.black;
            RenderSettings.ambientIntensity = 0f;
            RenderSettings.reflectionIntensity = 0f;
            yield return new WaitForSeconds(offDuration);       // Se desactivan durante onDuration segundos
        }
    }

    // M�todo para activar o desactivar las luces del nivel
    private void SetLightsActive(bool isActive)
    {
        foreach (Light light in lights)
        {
            if (light != null)
            {
                light.enabled = isActive;                       // Activar o desactivar luces
            }
        }
    }

    private void OnDestroy()
    {
        // Se restaunran las configuraciones originales al salir del nivel
        RenderSettings.ambientLight = originalAmbientColor;
        RenderSettings.ambientIntensity = originalAmbientIntensity;
        RenderSettings.reflectionIntensity = originalReflectionIntensity;
    }
}
