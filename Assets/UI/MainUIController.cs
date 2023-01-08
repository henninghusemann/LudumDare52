using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainUIController : MonoBehaviour
{
    
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        Button StartButton = root.Q<Button>("start-button");
        Button ExitButton = root.Q<Button>("exit-button");
        StartButton.clicked += StartGame;
        ExitButton.clicked += ExitGame;
    }

    private void StartGame()
    {
        SceneManager.LoadScene("game_scene");
    }

    private void ExitGame()
    {
        Application.Quit();
    }

}
