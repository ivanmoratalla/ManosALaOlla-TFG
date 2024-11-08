using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersUI : MonoBehaviour
{
    [SerializeField] private OrderUI orderUIPrefab;
    
    //private List<OrderUI> activeUIOrders = new List<OrderUI>();

    private Dictionary<int, OrderUI> activeUIOrders = new Dictionary<int, OrderUI>();

    private void OnEnable()
    {
        OrderManager.OnOrderCreate += CreateUIOrder;
        OrderManager.OnOrderSuccess += OrderSuccess;
        OrderManager.OnOrderFailed += OrderFailed;
        OrderManager.OnOrderTimeChange += UpdateOrderBar;
        
    }

    private void OnDisable()
    {
        OrderManager.OnOrderCreate -= CreateUIOrder;
        OrderManager.OnOrderSuccess -= OrderSuccess;
        OrderManager.OnOrderFailed -= OrderFailed;
        OrderManager.OnOrderTimeChange -= UpdateOrderBar;
    }

    private void CreateUIOrder(object sender, KeyValuePair<int, Recipe> orderData)
    {
        OrderUI newOrder = Instantiate(orderUIPrefab, transform);
        newOrder.SetOrder(orderData);
        activeUIOrders.Add(orderData.Key, newOrder);
    }

    private void OrderSuccess(object sender, int tableNumber)
    {
        OrderUI orderUI = activeUIOrders[tableNumber];
        orderUI.SuccessAndDestroy();
        
        activeUIOrders.Remove(tableNumber);
    }

    private void OrderFailed(object sender, int tableNumber)
    {
        OrderUI orderUI = activeUIOrders[tableNumber];
        orderUI.FailAndDestroy();

        activeUIOrders.Remove(tableNumber);
    }

    private void UpdateOrderBar(object sender, KeyValuePair<int, float> data)
    {
        OrderUI orderUI = activeUIOrders[(int)data.Key];                                // Se obtiene el pedido del que cambiar el tiempo en función de la mesa recibida
        orderUI.UpdateBar(data.Value);
    }

}
