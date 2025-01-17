using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    private enum GameState
    {
        PreGame,
        InGame,
        GameOver
    }

    public static EventHandler<float> OnTimeChange;                         // Evento que se va a invocar para notificar a la UI del cambio en el tiempo restante del nivel
    public static EventHandler<KeyValuePair<int,int>> OnGameOver;           // Evento para notificar que aparezca la UI de finalizar nivel. El booleano indica si se debe activar o no el bot�n de siguiente nivel

    [SerializeField] private LevelData levelData;                           // Datos del nivel (clientes junto a los platos que piden)
    private List<Table> tables;                                             // Lista con las distintas mesas que tiene el nivel
    [SerializeField] private GameObject clientPrefab;                       // Prefab de los clientes (para poder instanciarlos al llegar al restaurante)
    [SerializeField] private Transform clientSpawnPoint;                    // Punto donde se instancian los clientes (fuera de lo visible por el jugador)

    private List<PlayerController> players;                                 // Lista para almacenar a los jugadores

    private GameState gameState;                                            // Estado del nivel (Antes de comenzar el nivel, en el nivel o tras finalizar el nivel)
    
    private float actualTimer;                                              // El temporizador que se est� utilizando en el momento actual de la partida, tanto para previo al inicio como para el juego en si
    private float uiTimer;                                                  // Temporizador para saber cuando llamar a que se actualice el tiempo de la UI (para evitar notificar el evento muchas veces y solo cada segundo aprox)
    private float customersTimer;                                           // Temporizador para que los clientes lleguen de manera intercalada. 

    private Queue<CustomerData> customersQueue;                             // Cola de clientes que quieren llegar al restaurante
    private Queue<CustomerData> customersWaiting;                           // Clientes aue ya les ha tocado llegar al restaurante pero est�n esperando por una mesa libre
    private OrderManager orderManager;                                      // Variable para la clase encargada de manejar las comandas de este nivel


    void Awake()
    {
        orderManager = new OrderManager(this);
    }

    void Start()
    {
        SetTables();                                                        // Encontrar las mesas del nivel
        SetTableNumbersVisibility(true);
        SetPlayerMovementEnabled(false);

        gameState = GameState.PreGame;                                      // Se establece que el estado del nivel sea el previo al comienzo
        actualTimer = levelData.GetPreGameTime();                           // Se comienza a contar el tiempo previo al comienzo del nivel
        customersTimer = 0;                                                 // Se inicializa a 0 para que el primer cliente llegue instantaneamente

        customersQueue = new Queue<CustomerData>(levelData.GetCustomers()); // Se obtienen los datos de los clientes del nivel
        customersWaiting = new Queue<CustomerData>();                       // Se inicializa la cola de los clientes en espera para llegar

        OnTimeChange?.Invoke(this, levelData.GetGameTime());                // Se notifica que se actualice la UI con el tiempo total del nivel
    }

    private void Update()
    {
        actualTimer -= Time.deltaTime;
        uiTimer -= Time.deltaTime;
        customersTimer -= Time.deltaTime;

        switch (gameState)
        {
            case GameState.PreGame:
                if (actualTimer <= 0)                                       // Se comprueba si estando en el estado previo, ya se ha pasado su tiempo correspondiente
                {
                    SetTableNumbersVisibility(false);
                    SetPlayerMovementEnabled(true);

                    actualTimer = levelData.GetGameTime();                  // En ese caso, se comienza a contar el tiempo de juego normal
                    gameState = GameState.InGame;                           // Se cambia el estdo a "En partida"
                }
                break;
            case GameState.InGame:
                HandleCustomerSeating();
                if (uiTimer <= 0)
                {
                    OnTimeChange?.Invoke(this, actualTimer);                // Se invoca el evento para actualizar el tiempo restante en el UI
                    uiTimer = 1;                                            // Se resetea el temporizador de actualizaci�n de la UI
                }
                if (actualTimer <= 0)
                {
                    SetGameOver();
                }
                break;
        }

        orderManager.UpdateOrderTimes(Time.deltaTime);
    }

    private void HandleCustomerSeating()
    {
        if (customersQueue.Count > 0 && customersTimer <= 0)
        {
            customersWaiting.Enqueue(customersQueue.Dequeue());     // Mueve al cliente de la cola de nivel a la cola de espera
            customersTimer = levelData.GetTimeBetweenCustomers();   // Reinicia el temporizador de llegada
        }

        if (customersWaiting.Count > 0)
        {
            Table availableTable = GetAvailableTable();

            if (availableTable != null)
            {
                CustomerData customerData = customersWaiting.Dequeue(); // Saca al cliente en espera
                SeatCustomer(customerData, availableTable);            // Asignar al cliente a la mesa
            }
        }
    }

    private void SeatCustomer(CustomerData customerData, Table assignedTable)
    {
        Debug.Log("Sentando cliente en una mesa disponible");

        Customer newCustomer = Instantiate(clientPrefab, clientSpawnPoint.position, Quaternion.identity).GetComponent<Customer>();
        newCustomer.SetData(customerData, assignedTable, orderManager, clientSpawnPoint); // Configurar los datos del cliente
        assignedTable.SeatCustomer(newCustomer); // Asignar el cliente a la mesa

        Debug.Log("Cliente sentado en la mesa " + assignedTable.GetTableNumber());
    }

    private void SetTables()
    {
        tables = new List<Table>();

        Table[] foundTables = FindObjectsOfType<Table>();

        foreach (Table table in foundTables)
        {
            tables.Add(table);
        }
    }

    private void SetTableNumbersVisibility(bool visible)
    {
        foreach (Table table in tables)
        {
            table.SetTableNumberVisible(visible);
        }
    }

    // M�todo para activar o desactivar el movimiento de los jugadores (para el estado previo al comienzo del nivel)
    private void SetPlayerMovementEnabled(bool enabled)
    {
        if (players == null || players.Count == 0)
        {
            players = new List<PlayerController>(FindObjectsOfType<PlayerController>()); // Encuentra a todos los jugadores
        }

        foreach (var player in players)
        {
            player.enabled = enabled; // Habilita o deshabilita el componente de movimiento
        }
    }

    private async void SetGameOver()
    {
        gameState = GameState.GameOver;
        int finalStars = orderManager.GetScore();
        Debug.Log("Final stars: " + finalStars);
        //Debug.Log("Pedidos entregados:" + orderManager.pedidosEntregados);
        //Debug.Log("Pedidos perdidos:" + orderManager.pedidosPerdidos);

        ICloudDataService saveDataService = new CloudDataService();

        await saveDataService.SaveStarsForLevelIfHigher(levelData.GetLevelNumber(), finalStars);    // Se guarda en la base de datos la puntuaci�n obtenida (solo si es mayor que la previa)

        bool levelPassed = finalStars >= levelData.GetNeededScore();

        if (levelPassed)
        {
            await saveDataService.UpdateMaxLevelIfNeeded(levelData.GetLevelNumber());               // Si el nivel se ha completado, se comprueba si hay que actualizar el nivel m�ximo desbloqueado
        }

        OnGameOver?.Invoke(this, new KeyValuePair<int, int>(finalStars, levelData.GetNeededScore()));                                                      // Se notifica el evento para que aparezca la UI de fin de nivel                                                          
    }

    private Table GetAvailableTable()
    {
        foreach (Table table in tables)
        {
            if (table.IsAvailable())
            {
                return table;
            }
        }
        return null; // Si no hay mesas disponibles.
    }

    public void FreeTable(int tableNumber)
    {
        Debug.Log("Liberando mesa " + tableNumber);

        foreach (Table table in tables)
        {
            if (table.GetTableNumber() == tableNumber)
            {
                table.RemoveCustomer();
                Debug.Log("Se ha liberado la mesa " + tableNumber);
                return;
            }
        }
        Debug.LogWarning("No existe la mesa " + tableNumber);
    }
}
