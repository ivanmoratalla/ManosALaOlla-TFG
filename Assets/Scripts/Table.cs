using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private int tableNumber;
    [SerializeField] private bool isAvailable;
    private Customer currentCustomer;

    void Start()
    {
        isAvailable = true;
        currentCustomer = null;
    }

    public void seatCustomer(Customer customer)
    {
        if(isAvailable) {
            isAvailable = false;
            currentCustomer = customer;
            Debug.Log("Se ha sentado al cliente " + customer.getData().getName() + " en la mesa " + tableNumber);
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
            Destroy(currentCustomer.gameObject);
            currentCustomer = null;
        }
        else
        {
            Debug.Log("No se ha podido liberar la mesa " + tableNumber + " porque ya estaba vacía");
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
