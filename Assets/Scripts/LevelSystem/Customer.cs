using System.Collections;
using UnityEngine;
using UnityEngine.AI; // Necesario para NavMeshAgent

public class Customer : MonoBehaviour
{
    private CustomerData data;
    private Table assignedTable;
    private OrderManager orderManager;
    private NavMeshAgent navMeshAgent; // Referencia al NavMeshAgent
    private Transform spawnAndDestroyPoint;

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
            navMeshAgent.SetDestination(table.transform.position);

            StartCoroutine(WaitUntilArrives());
        }
        else
        {
            Debug.LogError("NavMeshAgent no encontrado en el cliente.");
        }
    }

    private IEnumerator WaitUntilArrives()
    {
        yield return new WaitForSeconds(1f);                                        // Espero al principio 1s para evitar que detecte velocidad 0 de cuando justo se instancia y empieza a moverse

        while (navMeshAgent.velocity.sqrMagnitude > 0.0001f)                        // Se comprueba si el cliente ha dejado de moverse (ha llegado a su destino)
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

    public void setData(CustomerData data, Table assignedTable, OrderManager od, Transform spawnAndDestroyPoint)
    {
        this.data = data;
        this.assignedTable = assignedTable;
        this.orderManager = od;
        this.spawnAndDestroyPoint = spawnAndDestroyPoint;
    }

    public CustomerData getData()
    {
        return this.data;
    }

    public void LeaveRestaurant()
    {
        navMeshAgent.enabled = true;

        navMeshAgent.SetDestination(spawnAndDestroyPoint.position);

        StartCoroutine(WaitToDestroy());
    }

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(1f);                                        // Espero al principio 1s para evitar que detecte velocidad 0 de cuando justo se instancia y empieza a moverse

        while (navMeshAgent.velocity.sqrMagnitude > 0.0001f)                        // Se comprueba si el cliente ha dejado de moverse (ha llegado a su destino)
        {
            yield return null;                                                      // Se espera al siguiente frame para volver a hacer la comprobacon
        }

        Destroy(this.gameObject);

    }
}