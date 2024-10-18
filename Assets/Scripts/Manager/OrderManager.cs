using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }
    private Dictionary<int, string> activeOrders = new Dictionary<int, string>(); // Mesa y plato pedido.

    void Awake()
    {
        Instance = this;
    }

    public void CreateOrder(string dishRequest, int tableNumber)
    {
        activeOrders[tableNumber] = dishRequest;
        Debug.Log("Comanda creada para la mesa " + tableNumber + ": " + dishRequest);
    }

    public bool ServeDish(int tableNumber, string servedDish)
    {
        if (activeOrders.ContainsKey(tableNumber) && servedDish != null)
        {
            if (activeOrders[tableNumber] == servedDish)
            {
                Debug.Log("Plato correcto servido en la mesa " + tableNumber);
                activeOrders.Remove(tableNumber);                           // Elimino la comanda.
                Level.Instance.freeTable(tableNumber);               // LLamo al manager para liberar la mesa y que se pueda sentar otro cliente que llegue

                return true;
            }
            else
            {
                Debug.Log("Plato incorrecto servido en la mesa " + tableNumber);
                return false;
            }
        }
        else
        {
            Debug.Log("No hay comanda para la mesa " + tableNumber);
            return false;
        }
    }
}
