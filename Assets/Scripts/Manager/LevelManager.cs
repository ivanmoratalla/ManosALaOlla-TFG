using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private LevelData levelData; // Asignas el ScriptableObject desde el inspector.
    [SerializeField] private List<Table> tables;
    [SerializeField] private GameObject clientPrefab;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        tables = new List<Table>();

        Table[] foundTables = FindObjectsOfType<Table>();

        foreach (Table table in foundTables)
        {
            tables.Add(table);
        }

        spawnCustomers();
    }

    void spawnCustomers()
    {
        foreach (var customerData in levelData.getCustomers())
        {
            // Instancia y posiciona los clientes en la escena
            Customer newCustomer = Instantiate(clientPrefab).GetComponent<Customer>(); // clientPrefab es el prefab de un cliente.
            newCustomer.setData(customerData); // Asigna los datos de cliente
            Debug.Log("Cliente creado");
        }
        Debug.Log("Todos los clientes creados");
    }

    public Table getAvailableTable()
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
