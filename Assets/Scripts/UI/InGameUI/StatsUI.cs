using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private Text coinsText;
    [SerializeField] private Text timeText;
    
    [SerializeField] private Text countdownToReappearText;
    [SerializeField] private GameObject reappearInfoArea;

    // MonoBehavior Callbacks *****
    private void OnEnable()
    {
        OrderManager.OnServedDish += UpdateCoins;
        Level.OnTimeChange += UpdateRemainingTime;
        PlayerInteraction.OnPlayerDisappear += ShowCountdownToReappear;

        reappearInfoArea.SetActive(false);
    }

    private void OnDisable()
    {
        OrderManager.OnServedDish -= UpdateCoins;
        Level.OnTimeChange -= UpdateRemainingTime;
        PlayerInteraction.OnPlayerDisappear -= ShowCountdownToReappear;

    }

    // Private Methods *****
    private void UpdateRemainingTime(object sender, float newTime)
    {
        timeText.text = ToClock(newTime);
    }

    private void UpdateCoins(object sender, int updatedScore)
    {
        coinsText.text = updatedScore.ToString();
    }

    private string ToClock(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60); // Convierte segundos a minutos
        int remainingSeconds = Mathf.FloorToInt(time % 60); // Obtiene los segundos restantes

        return string.Format("{0:00}:{1:00}", minutes, remainingSeconds); // Formatea como "mm:ss"
    }

    private void ShowCountdownToReappear(float duration, Vector3 position)
    {
        StartCoroutine(HandleCountdown(duration, position));
    }

    private IEnumerator HandleCountdown(float duration, Vector3 position)
    {
        reappearInfoArea.SetActive(true);

        float remainingTime = duration;
        while (remainingTime > 0)
        {
            int roundedTime = Mathf.CeilToInt(remainingTime);

            countdownToReappearText.text = $"{roundedTime}s"; // Actualizar texto del contador

            yield return new WaitForSeconds(0.1f);
            remainingTime -= 0.1f;
        }

        reappearInfoArea.SetActive(false);

    }
}
