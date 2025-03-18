using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneBtn : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }

    public void GameQuit()
    {
        Application.Quit();
    }
}
