using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private Text coinsText;
    [SerializeField] private Text timeText;
    
    [SerializeField] private Text countdownToReappearTextPlayer1;
    [SerializeField] private Text countdownToReappearTextPlayer2;

    [SerializeField] private GameObject reappearInfoAreaPlayer1;
    [SerializeField] private GameObject reappearInfoAreaPlayer2;


    // MonoBehavior Callbacks *****
    private void OnEnable()
    {
        OrderManager.OnServedDish += UpdateCoins;
        Level.OnTimeChange += UpdateRemainingTime;
        CarInteraction.OnPlayerDisappear += ShowCountdownToReappear;

        reappearInfoAreaPlayer1.SetActive(false);
        reappearInfoAreaPlayer2.SetActive(false);
    }

    private void OnDisable()
    {
        OrderManager.OnServedDish -= UpdateCoins;
        Level.OnTimeChange -= UpdateRemainingTime;
        CarInteraction.OnPlayerDisappear -= ShowCountdownToReappear;
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

    private void ShowCountdownToReappear(int playerID, float duration, Vector3 position)
    {
        if (playerID == 1)
        {
            StartCoroutine(HandleCountdown(duration, position, reappearInfoAreaPlayer1, countdownToReappearTextPlayer1));
        }
        else if (playerID == 2)
        {
            StartCoroutine(HandleCountdown(duration, position, reappearInfoAreaPlayer2, countdownToReappearTextPlayer2));
        }
    }

    private IEnumerator HandleCountdown(float duration, Vector3 position, GameObject infoArea, Text countdownText)
    {
        infoArea.SetActive(true);

        float remainingTime = duration;
        while (remainingTime > 0)
        {
            int roundedTime = Mathf.CeilToInt(remainingTime);

            countdownText.text = $"{roundedTime}s"; // Actualizar texto del contador

            yield return new WaitForSeconds(0.1f);
            remainingTime -= 0.1f;
        }

        infoArea.SetActive(false);
    }
}
