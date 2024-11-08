using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private Text coinsText;
    [SerializeField] private Text timeText;

    // MonoBehavior Callbacks *****
    private void OnEnable()
    {
        OrderManager.OnServedDish += UpdateCoins;
        Level.OnTimeChange += UpdateRemainingTime;
    }

    private void OnDisable()
    {
        OrderManager.OnServedDish -= UpdateCoins;
        Level.OnTimeChange -= UpdateRemainingTime;
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
}
