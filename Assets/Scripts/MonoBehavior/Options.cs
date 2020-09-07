using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{
    public void ToLevelSelect()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
