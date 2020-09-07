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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }
}
