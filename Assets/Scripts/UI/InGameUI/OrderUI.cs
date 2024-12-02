using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    [SerializeField] private Image timerBar;

    [SerializeField] private Text recipeName;
    [SerializeField] private Image recipeImage;
    [SerializeField] private Text assignedTable;


    private void OnEnable()
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.localScale = Vector3.zero;
        LeanTween.scale(rect, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutSine);
    }


    // Public Methods
    public void SetOrder(KeyValuePair<int, Recipe> orderData)
    {
        recipeName.text = orderData.Value.getRecipeName();
        assignedTable.text = "Mesa: " + orderData.Key.ToString();
    }

    public void UpdateBar(float newTime)
    {
        timerBar.fillAmount = newTime;

        if (newTime <= 0)
        {
            LeanTween.color(gameObject.GetComponent<RectTransform>(), Color.red, 0.5f).setDestroyOnComplete(true);
        }

    }

    public void SuccessAndDestroy()
    {
        LeanTween.color(gameObject.GetComponent<RectTransform>(), Color.green, 0.5f).setDestroyOnComplete(true);
    }

    public void FailAndDestroy()
    {
        LeanTween.color(gameObject.GetComponent<RectTransform>(), Color.red, 0.5f).setDestroyOnComplete(true);
    }
}
