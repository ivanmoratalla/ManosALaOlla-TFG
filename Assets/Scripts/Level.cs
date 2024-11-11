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
    public static EventHandler<Boolean> OnGameOver;                         // Evento para notificar que aparezca la UI de finalizar nivel. El booleano indica si se debe activar o no el botón de siguiente nivel

    [SerializeField] private LevelData levelData;                           // Datos del nivel (clientes junto a los platos que piden)
    [SerializeField] private List<Table> tables;                            // Lista con las distintas mesas que tiene el nivel
    [SerializeField] private GameObject clientPrefab;                       // Prefab de los clientes (para poder instanciarlos al llegar al restaurante)


    private GameState gameState;                                            // Estado del nivel (Antes de comenzar el nivel, en el nivel o tras finalizar el nivel)
    
    private float actualTimer;                                              // El temporizador que se está utilizando en el momento actual de la partida, tanto para previo al inicio como para el juego en si
    private float uiTimer;                                                  // Temporizador para saber cuando llamar a que se actualice el tiempo de la UI (para evitar notificar el evento muchas veces y solo cada segundo aprox)
    private float customersTimer;                                           // Temporizador para que los clientes lleguen de manera intercalada. 

    private Queue<CustomerData> customersQueue;                             // Cola de clientes que quieren llegar al restaurante
    private OrderManager orderManager;                                      // Variable para la clase encargada de manejar las comandas de este nivel


    void Awake()
    {
        orderManager = new OrderManager(this);
    }

    void Start()
    {
        initializeLevel();
    }


    // Método para la inicialización de un nivel. Hay que encontrar las mesas que haya en el nivel y comenzar a sentar a los clientes en las mesas cuando les sea posible
    private void initializeLevel()
    {
        setTables();                                                        // Encontrar las mesas del nivel
        
        gameState = GameState.PreGame;                                      // Se establece que el estado del nivel sea el previo al comienzo
        actualTimer = levelData.getPreGameTime();                           // Se comienza a contar el tiempo previo al comienzo del nivel
        customersTimer = 0;                                                 // Se inicializa a 0 para que el primer cliente llegue instantaneamente
        customersQueue = new Queue<CustomerData>(levelData.getCustomers()); // Se obtienen los datos de los clientes del nivel

        OnTimeChange?.Invoke(this, levelData.getGameTime());                // Se notifica que se actualice la UI con el tiempo total del nivel

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
                    actualTimer = levelData.getGameTime();                  // En ese caso, se comienza a contar el tiempo de juego normal
                    gameState = GameState.InGame;                           // Se cambia el estdo a "En partida"
                }
                break;
            case GameState.InGame:
                if(customersQueue.Count > 0 && customersTimer <= 0)
                {
                    StartCoroutine(seatCustomer());
                    customersTimer = levelData.getTimeBetweenCustomers();   // Se reinicia el tiempo para que llegue otro cliente
                }
                if (uiTimer <= 0)
                {
                    OnTimeChange?.Invoke(this, actualTimer);                // Se invoca el evento para actualizar el tiempo restante en el UI
                    uiTimer = 1;                                            // Se resetea el temporizador de actualización de la UI
                }
                if (actualTimer <= 0)
                {
                    SetGameOver();
                }
                break;
        }

        orderManager.UpdateOrderTimes(Time.deltaTime);
    }


    private void setTables()
    {
        tables = new List<Table>();

        Table[] foundTables = FindObjectsOfType<Table>();

        foreach (Table table in foundTables)
        {
            tables.Add(table);
        }
    }

    private IEnumerator seatCustomer()
    {
        Debug.Log("Se intenta sentar a un cliente");
        Table assignedTable = null;

        while (assignedTable == null)
        {
            assignedTable = getAvailableTable();

            if (assignedTable != null)                                                          // Mesa libre encontrada
            {
                Debug.Log("Se ha encontrado una mesa libre para el cliente");
            }
            else
            {
                yield return new WaitForSeconds(1f);                                            // Si no hay mesas libres, esperar un segundo antes de volver a comprobar.
            }
        }

        // Aquí ya se ha encontrado una mesa libre para el cliente, por lo que se le sien0ta
        var customerData = customersQueue.Dequeue();
        Customer newCustomer = Instantiate(clientPrefab).GetComponent<Customer>();              // clientPrefab es el prefab de un cliente.
        newCustomer.setData(customerData, assignedTable, orderManager);             // Asigna los datos de cliente

        yield return null;                                                                      // Introduzco un retraso para asegurarme que el cliente está completamente instanciado
        
        assignedTable.seatCustomer(newCustomer);

        Debug.Log("Cliente creado"); 
    }

    private async void SetGameOver()
    {
        gameState = GameState.GameOver;
        int finalStars = orderManager.getScore();
        Debug.Log("Final stars: " + finalStars);

        ICloudDataService saveDataService = new CloudDataService();

        await saveDataService.SaveStarsForLevelIfHigher(levelData.getLevelNumber(), finalStars);    // Se guarda en la base de datos la puntuación obtenida (solo si es mayor que la previa)

        Boolean levelPassed = finalStars >= levelData.getNeededScore();

        if (levelPassed)
        {
            await saveDataService.UpdateMaxLevelIfNeeded(levelData.getLevelNumber());               // Si el nivel se ha completado, se comprueba si hay que actualizar el nivel máximo desbloqueado
        }

        OnGameOver?.Invoke(this, levelPassed);                                                      // Se notifica el evento para que aparezca la UI de fin de nivel                                                          
    }

    private Table getAvailableTable()
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

    public void freeTable(int tableNumber)
    {
        Debug.Log("Liberando mesa " + tableNumber);

        foreach (Table table in tables)
        {
            if (table.getTableNumber() == tableNumber)
            {
                table.removeCustomer();
                Debug.Log("Se ha liberado la mesa " + tableNumber);
                return;
            }
        }
        Debug.LogWarning("No existe la mesa " + tableNumber);
    }
}
