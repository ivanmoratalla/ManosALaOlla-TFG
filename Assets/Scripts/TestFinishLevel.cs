using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFinishLevel : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(Input.GetKey(KeyCode.L))
        {

            LevelsManager.Instance.levelCompleted(1);
        }
    }
}
