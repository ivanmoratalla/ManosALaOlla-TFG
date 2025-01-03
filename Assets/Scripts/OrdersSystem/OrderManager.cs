using System;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager
{
    public static EventHandler<int> OnServedDish;                                       // Evento que se va a invocar para notificar a la UI que tiene que actualizar la puntuación
    public static EventHandler<KeyValuePair<int, Recipe>> OnOrderCreate;                // Evento para notificar a la UI cuando se crea un nuevo pedido
    public static EventHandler<int> OnOrderSuccess;                                     // Evento para notificar cuando un pedido ha sido completado (se envía la mesa)
    public static EventHandler<int> OnOrderFailed;                                      // Evento para notificar cuando un pedido ha fracasado (se envía la mesa)

    public static EventHandler<KeyValuePair<int,float>> OnOrderTimeChange;              // Evento para notificar que hay que cambiar la barra de tiempo del pedido de una mesa


    private Level level;                                                                // Nivel sobre el que maneja las órdenes    
    private Dictionary<int, string> activeOrders;                                       // Pedidos por mesa activos
    private Dictionary<int, float> orderTimers;                                         // Variable para guardar el tiempo que lleva activo cada pedido

    private int score;                                                                  // Puntuación del nivel

    private float orderTimeLimit = 40f;                                                 // Duración máxima que puede esperar un cliente
    private const int maxStars = 5;                                                     // Puntos máximos que se pueden obtener por un pedido (5 estrellas)

    public int pedidosEntregados = 0;
    public int pedidosPerdidos = 0;

    public OrderManager(Level level)
    {
        activeOrders = new Dictionary<int, string>();
        orderTimers = new Dictionary<int, float>();
        score = 0;

        this.level = level;

        PlayerInteraction.OnTryToServeDish += ServeDish;                                // Suscribirse al evento

    }

    public void CreateOrder(Recipe dishRequest, int tableNumber)
    {
        activeOrders[tableNumber] = dishRequest.GetRecipeName();
        orderTimers[tableNumber] = 0f;                                                  // Tiempo del pedido a 0

        OnOrderCreate?.Invoke(this, new KeyValuePair<int, Recipe>(tableNumber, dishRequest));

        Debug.Log("Comanda creada para la mesa " + tableNumber + ": " + dishRequest);
    }

    private void ServeDish(int tableNumber, string servedDish, Action<bool> callback)
    {
        bool res = false;

        if (activeOrders.ContainsKey(tableNumber) && servedDish != null)
        {
            if (activeOrders[tableNumber] == servedDish)
            {
                float orderTime = orderTimers[tableNumber];                         // Tomamos el tiempo acumulado en el pedido.
                score += CalculateStars(orderTime);                                 // Sumar los puntos al total del nivel.

                OnServedDish?.Invoke(this, score);        // Se invoca el evento al servir correctamente el plato para actualizar la UI con la nueva puntuación del nivel
                OnOrderSuccess?.Invoke(this, tableNumber);
                pedidosEntregados++;

                Debug.Log("Plato correcto servido en la mesa " + tableNumber);
                Debug.Log("Total de puntos del nivel: " + score);

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

    // Método para calcular los puntos en función del tiempo que se ha tardado
    private int CalculateStars(float timeTaken)
    {
        float percentage = timeTaken / orderTimeLimit;

        int stars;
        if (percentage < 0.4f)                
        {
            stars = 5;
        }
        else if (percentage < 0.5)          
        {
            stars = 4;
        }
        else if (percentage < 0.6f)           
        {
            stars = 3;
        }
        else if (percentage < 0.8f)           
        {
            stars = 2;
        }
        else                                  
        {
            stars = 1;
        }
        Debug.Log("Puntos del pedido: " + stars);

        return stars;
    }

    // Método para revisar y eliminar pedidos que hayan expirado
    public void UpdateOrderTimes(float deltaTime)
    {
        List<int> expiredOrders = new List<int>();

        foreach (var order in activeOrders)
        {
            int tableNumber = order.Key;

            // Acumulamos el tiempo que ha pasado desde que se creó el pedido
            orderTimers[tableNumber] += deltaTime;

            if (orderTimers[tableNumber] >= orderTimeLimit)
            {
                Debug.Log("El pedido en la mesa " + tableNumber + " ha expirado y se ha perdido.");
                expiredOrders.Add(tableNumber);
                score -= 2; // Restamos 2 puntos por pedido perdido.
                OnServedDish?.Invoke(this, score);

                pedidosPerdidos++;
            }
            else
            {
                OnOrderTimeChange?.Invoke(this, new KeyValuePair<int, float>(tableNumber, (orderTimeLimit - orderTimers[tableNumber]) / orderTimeLimit));
            }
        }

        // Eliminamos los pedidos expirados de los diccionarios y liberamos las mesas.
        foreach (int tableNumber in expiredOrders)
        {
            OnOrderFailed?.Invoke(this, tableNumber);

            activeOrders.Remove(tableNumber);
            orderTimers.Remove(tableNumber);
            level.freeTable(tableNumber);
        }
    }

    public int getScore()
    {
        return score;
    }
}
