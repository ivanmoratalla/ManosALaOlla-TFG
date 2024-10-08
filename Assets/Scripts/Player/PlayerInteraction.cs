using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    public GameObject hand;
    private GameObject pickedObject = null;


    void Update()
    {
        if (pickedObject != null && Input.GetKey("f"))
        {
            pickedObject.GetComponent<Rigidbody>().useGravity = true;
            pickedObject.GetComponent<Rigidbody>().isKinematic = false;

            pickedObject.gameObject.transform.SetParent(null);

            pickedObject = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Objeto") && pickedObject == null && Input.GetKey("e"))
        {
            handlePickObject(other);
        }
        else
        {
            // Compruebo si se la colisión es con una mesa
            Table table = other.GetComponent<Table>();
            if (table != null)
            {
                handleTableInteraction(table);
            }
        }
    }

    private void handlePickObject(Collider other)
    {
        other.GetComponent<Rigidbody>().useGravity = false;
        other.GetComponent<Rigidbody>().isKinematic = true;

        other.transform.position = hand.transform.position;
        other.gameObject.transform.SetParent(hand.gameObject.transform);

        pickedObject = other.gameObject;
    }

    private void handleTableInteraction(Table table)
    {
        Food dish;
        // Compruebo si se tiene en la mano un plato y se pulsa el botón de entregar
        if (pickedObject != null && (dish = pickedObject.GetComponent<Food>()) != null && Input.GetKey("q"))
        {
            deliverOrder(table.getTableNumber(), dish.getFoodData().getName());
        }
    }

    private void deliverOrder(int tableNumber, string dish)
    {
        bool res = OrderManager.Instance.ServeDish(tableNumber, dish);

        if (res)
        {
            Destroy(pickedObject);
            pickedObject = null;
        }
        else
        {
            // Aquí incluiré las penalizaciones que sean
        }

    }
}