using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLightingManager : MonoBehaviour
{
    [SerializeField] private List<Light> lights;       // Luces que quieres alternar (p.ej., direccionales, puntos)
    [SerializeField] private float onDuration = 10f;  // Tiempo encendido
    [SerializeField] private float offDuration = 5f;  // Tiempo apagado

    private Color originalAmbientColor;               // Guardar� el color original
    private float originalAmbientIntensity;           // Guardar� la intensidad original
    private float originalReflectionIntensity;        // Guardar� la intensidad de reflexi�n original

    private void Start()
    {
        // Guarda las configuraciones originales de iluminaci�n
        originalAmbientColor = RenderSettings.ambientLight;
        originalAmbientIntensity = RenderSettings.ambientIntensity;
        originalReflectionIntensity = RenderSettings.reflectionIntensity;

        // Comienza la alternancia de luces
        StartCoroutine(ToggleDarkness());
    }

    private IEnumerator ToggleDarkness()
    {
        while (true)
        {
            // ENCENDER luces y restaurar iluminaci�n ambiental
            SetLightsActive(true);
            RenderSettings.ambientLight = originalAmbientColor;
            RenderSettings.ambientIntensity = originalAmbientIntensity;
            RenderSettings.reflectionIntensity = originalReflectionIntensity;
            yield return new WaitForSeconds(onDuration);

            // APAGAR luces y reducir la iluminaci�n ambiental
            SetLightsActive(false);
            RenderSettings.ambientLight = Color.black;
            RenderSettings.ambientIntensity = 0f;
            RenderSettings.reflectionIntensity = 0f;
            yield return new WaitForSeconds(offDuration);
        }
    }

    private void SetLightsActive(bool isActive)
    {
        foreach (Light light in lights)
        {
            if (light != null)
            {
                light.enabled = isActive; // Activar o desactivar luces
            }
        }
    }

    private void OnDestroy()
    {
        // Restaura las configuraciones originales al salir del nivel
        RenderSettings.ambientLight = originalAmbientColor;
        RenderSettings.ambientIntensity = originalAmbientIntensity;
        RenderSettings.reflectionIntensity = originalReflectionIntensity;
    }
}
