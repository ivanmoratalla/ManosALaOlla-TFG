using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level Instance { get; private set; }

    [SerializeField] private LevelData levelData; // Asigno el ScriptableObject desde el inspector.
    [SerializeField] private List<Table> tables;
    [SerializeField] private GameObject clientPrefab;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        initializeLevel();
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
                Debug.Log("Buscando mesa");
                assignedTable = getAvailableTable();

                if (assignedTable != null)
                {
                    // Mesa libre encontrada
                    Debug.Log("Se ha encontrado una mesa libre para el cliente");
                }
                else
                {
                    // No hay mesas libres, esperar un segundo antes de volver a comprobar.
                    Debug.Log("No hay ninguna mesa disponible para el cliente");
                    yield return new WaitForSeconds(1f);
                }
            }
            // Aquí ya se ha encontrado una mesa libre para el cliente, por lo que se le sienta
            // Instancia y posiciona los clientes en la escena
            Customer newCustomer = Instantiate(clientPrefab).GetComponent<Customer>();  // clientPrefab es el prefab de un cliente.
            newCustomer.setData(customerData, assignedTable);                           // Asigna los datos de cliente

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
