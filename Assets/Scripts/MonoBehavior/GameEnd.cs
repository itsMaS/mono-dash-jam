using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    void Awake()
    {
        if (PlayerPrefs.GetInt("GameBeaten") == 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            PlayerPrefs.SetInt("GameBeaten", 1);
        }
    }
    private void Start()
    {
        HighscoreManager.instance.StopTimer();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HighscoreManager.instance.Register();
        }
    }
}
