using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreen : MonoBehaviour
{
    public void PlayPressed()
    {
        SceneManager.LoadScene(1);
        HighscoreManager.instance.StartTimer();
    }
}
