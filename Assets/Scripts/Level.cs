using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Level : MonoBehaviour
{
    //public static Level Instance { get; private set; }

    [SerializeField] private LevelData levelData;                   // Datos del nivel (clientes junto a los platos que piden)
    [SerializeField] private List<Table> tables;                    // Lista con las distintas mesas que tiene el nivel
    [SerializeField] private GameObject clientPrefab;               // Prefab de los clientes (para poder instanciarlos al llegar al restaurante)

    private OrderManager orderManager;                              // Variable para la clase encargada de manejar las comandas de este nivel


    void Awake()
    {
        orderManager = new OrderManager(this);
    }

    void Start()
    {
        initializeLevel();
    }

    private void Update()
    {
        orderManager.UpdateOrderTimes(Time.deltaTime);
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

    // Método para la inicialización de un nivel. Hay que encontrar las mesas que haya en el nivel y comenzar a sentar a los clientes en las mesas cuando les sea posible
    private void initializeLevel()
    {
        setTables();

        StartCoroutine(seatCustomers());
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

    private IEnumerator seatCustomers()
    {
        Debug.Log("Comienzo de sentar");
        foreach (var customerData in levelData.getCustomers())
        {
            Table assignedTable = null;

            while (assignedTable == null)
            {
                //Debug.Log("Buscando mesa");
                assignedTable = getAvailableTable();

                if (assignedTable != null)                                              // Mesa libre encontrada
                {
                    Debug.Log("Se ha encontrado una mesa libre para el cliente");
                }
                else
                {
                    yield return new WaitForSeconds(1f);                                // Si no hay mesas libres, esperar un segundo antes de volver a comprobar.
                }
            }
            // Aquí ya se ha encontrado una mesa libre para el cliente, por lo que se le sienta
            // Instancia y posiciona los clientes en la escena
            Customer newCustomer = Instantiate(clientPrefab).GetComponent<Customer>();  // clientPrefab es el prefab de un cliente.
            newCustomer.setData(customerData, assignedTable, orderManager);                           // Asigna los datos de cliente

            yield return null;                                                          // Introduzco un retraso para asegurarme que el cliente está completamente instanciado
            assignedTable.seatCustomer(newCustomer);

            Debug.Log("Cliente creado");
        }
        Debug.Log("Todos los clientes se han sentado en alguna mesa");
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
}
