using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string gameScene = "Game";
    [SerializeField] private GameObject audioManager;

    public void PlayGame()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
