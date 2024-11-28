using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Button goBackButton = null;


    [SerializeField] private MenuHandler menuHandler;

    private void Awake()
    {
        goBackButton.onClick.AddListener(GoBack);
    }

    private void GoBack()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if(currentScene == "Menu")
        {
            menuHandler.ShowMainMenu();
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
