using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    // Outlets
    public GameObject startMenu;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    // Methods
    void Awake()
    {
        instance = this;

    }

    void Start() {
        startMenu.SetActive(true);
        gameObject.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        Time.timeScale = 0;
        PlayerMovement.instance.isPaused = true;
    }

    public void Show() {
        pauseMenu.SetActive(true);
        gameObject.SetActive(true);
        Time.timeScale = 0;
        PlayerMovement.instance.isPaused = true;
    }

    public void Hide() {
        startMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        gameObject.SetActive(false);
        Time.timeScale = 1;
        if(PlayerMovement.instance != null) {
            PlayerMovement.instance.isPaused = false;
        }
    }

    public void GameOver() {
        startMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        gameObject.SetActive(true);
        Time.timeScale = 0;
        PlayerMovement.instance.isPaused = true;
    }

    public void LoadLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
