using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    // Outlets
    public GameObject mainMenu;

    // Methods
    void Awake()
    {
        instance = this;

    }

    void Start() {
        Show();
    }

    public void Show() {
        mainMenu.SetActive(true);
        gameObject.SetActive(true);
        Time.timeScale = 0;
        PlayerMovement.instance.isPaused = true;
    }

    public void Hide() {
        mainMenu.SetActive(false);
        gameObject.SetActive(false);
        Time.timeScale = 1;
        if(PlayerMovement.instance != null) {
            PlayerMovement.instance.isPaused = false;
        }
    }

    public void LoadLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
