using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private CustomerData data;
    private Table assignedTable;
    private OrderManager orderManager;


    void Start()
    {
        handleCustomerArrival();
    }

    private void handleCustomerArrival()
    {
        // AQUI TENDRE QUE HACER LAS ANIMACIONES O LO QUE SEA, EL CAMINO PARA IR A LA MESA Y SENTARSE
        goToTable(assignedTable);
        createOrder();
    }

    private void goToTable(Table table)
    {
        transform.position = table.transform.position;
    }

    private void createOrder()
    {
        orderManager.CreateOrder(data.getDish(), assignedTable.getTableNumber());
    }

    public void setData(CustomerData data, Table assignedTable, OrderManager od)
    {
        this.data = data;
        this.assignedTable = assignedTable;
        this.orderManager = od; 
    }

    public CustomerData getData()
    {
        return this.data;
    }
}
