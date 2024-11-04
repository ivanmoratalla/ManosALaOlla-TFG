using System;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager
{
    //public static OrderManager Instance { get; private set; }

    private Level level;                                                                // Nivel sobre el que maneja las órdenes    
    private Dictionary<int, string> activeOrders;                                       // Pedidos por mesa activos

    public OrderManager(Level level)
    {
        activeOrders = new Dictionary<int, string>();
        this.level = level;
        PlayerInteraction.OnServeDish += ServeDish; // Suscribirse al evento

    }

    public void CreateOrder(string dishRequest, int tableNumber)
    {
        activeOrders[tableNumber] = dishRequest;
        Debug.Log("Comanda creada para la mesa " + tableNumber + ": " + dishRequest);
    }

    private void ServeDish(int tableNumber, string servedDish, Action<bool> callback)
    {
        bool res = false;

        if (activeOrders.ContainsKey(tableNumber) && servedDish != null)
        {
            if (activeOrders[tableNumber] == servedDish)
            {
                Debug.Log("Plato correcto servido en la mesa " + tableNumber);
                activeOrders.Remove(tableNumber);                                   // Elimino la comanda.
                level.freeTable(tableNumber);                                       // LLamo al manager para liberar la mesa y que se pueda sentar otro cliente que llegue

                res = true;
            }
            else
            {
                Debug.Log("Plato incorrecto servido en la mesa " + tableNumber);
            }
        }
        else
        {
            Debug.Log("No hay comanda para la mesa " + tableNumber);
        }

        callback?.Invoke(res);
    }
}
