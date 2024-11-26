using System.Collections;
using UnityEngine;
using UnityEngine.AI; // Necesario para NavMeshAgent

public class Customer : MonoBehaviour
{
    private CustomerData data;
    private Table assignedTable;
    private OrderManager orderManager;
    private NavMeshAgent navMeshAgent; // Referencia al NavMeshAgent

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>(); // Obtener el NavMeshAgent
        handleCustomerArrival();
    }

    private void handleCustomerArrival()
    {
        // Moverse hacia la mesa asignada
        goToTable(assignedTable);
    }

    private void goToTable(Table table)
    {
        if (navMeshAgent != null)
        {
            // Establecer el destino del NavMeshAgent a la posición de la mesa
            navMeshAgent.SetDestination(table.transform.position);

            // Opcional: Llamar a createOrder() cuando llegue a la mesa
            StartCoroutine(WaitUntilArrives(table));
        }
        else
        {
            Debug.LogError("NavMeshAgent no encontrado en el cliente.");
        }
    }

    private IEnumerator WaitUntilArrives(Table table)
    {
        yield return new WaitForSeconds(1f);                                        // Espero al principio 1s para evitar que detecte velocidad 0 de cuando justo se instancia y empieza a moverse

        while (navMeshAgent.velocity.sqrMagnitude > 0.01f)                          // Se comprueba si el cliente ha dejado de moverse (ha llegado a su destino)
        {
            yield return null;                                                      // Se espera al siguiente frame para volver a hacer la comprobacon
        }

        createOrder();                                                              // Se crea la orden cuando llega a la mesa
        navMeshAgent.enabled = false;                                               // Con esto evito que se mueva de su sitio una vez llegue a la mesa

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