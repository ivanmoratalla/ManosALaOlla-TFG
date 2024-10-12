using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private CustomerData data;
    private LevelManager levelManager;
    private Table assignedTable;


    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        StartCoroutine(handleCustomerArrival());
    }

    private IEnumerator handleCustomerArrival()
    {
        Debug.Log("Esperando a que le toque llegar");
        yield return new WaitForSeconds(data.getArrivalTime());     // Con esto se espera a que le llegue el turno de llegar al restaurante

        Debug.Log("Comienza a buscar mesa disponible");
        while (assignedTable == null)                               // El cliente espera hasta que tenga una mesa disponible para sentarse
        {
            assignedTable = levelManager.getAvailableTable();

            if (assignedTable != null)      
            {
                // Mesa libre encontrada
                assignedTable.seatCustomer(this);
                goToTable(assignedTable);
                createOrder();
            }
            else
            {
                // No hay mesas libres, esperar un segundo antes de volver a comprobar.
                yield return new WaitForSeconds(1f);
            }
        }
    }

    private void goToTable(Table table)
    {
        transform.position = table.transform.position;
    }

    private void createOrder()
    {
        OrderManager.Instance.CreateOrder(data.getDish(), assignedTable.getTableNumber());
    }

    public void setData(CustomerData data)
    {
        this.data = data;
    }

    public CustomerData getData()
    {
        return this.data;
    }
}
