using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Table : MonoBehaviour
{
    [SerializeField] private int tableNumber;
    [SerializeField] private Text tableNumberText;

    [SerializeField] private bool isAvailable;
    private Customer currentCustomer;

    void Start()
    {
        isAvailable = true;
        currentCustomer = null;

        if (tableNumberText != null)
        {
            SetTableNumberVisible(false);
        }
    }

    public void seatCustomer(Customer customer)
    {
        if(isAvailable) {
            isAvailable = false;
            currentCustomer = customer;
        }
        else {
            Debug.Log("No se ha podido sentar al usuario en la mesa " + tableNumber + " porque está ocupada");
        }
    }

    public void removeCustomer()
    {
        if (!isAvailable)
        {
            isAvailable = true;
            currentCustomer.LeaveRestaurant();
            currentCustomer = null;
        }
        else
        {
            Debug.Log("No se ha podido liberar la mesa " + tableNumber + " porque ya estaba vacía");
        }
    }

    public void SetTableNumberVisible(bool visible)
    {
        if (tableNumberText != null)
        {
            tableNumberText.transform.parent.gameObject.SetActive(visible);

            if (visible)
            {
                Vector3 numberPosition = Camera.main.WorldToScreenPoint(transform.position);
                numberPosition = new Vector3(numberPosition.x, numberPosition.y - 45, numberPosition.z);

                tableNumberText.transform.parent.transform.position = numberPosition;
                tableNumberText.text = "Mesa " + tableNumber; // Actualizar el texto con el número de mesa

                LeanTween.scale(tableNumberText.transform.parent.gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutBack);
            }
        }
        else
        {
            Debug.LogWarning($"La mesa {tableNumber} no tiene asignado un componente de texto.");
        }
    }

    public int getTableNumber() { 
        return tableNumber; 
    }

    public bool IsAvailable()
    {
        return isAvailable;
    }
}
